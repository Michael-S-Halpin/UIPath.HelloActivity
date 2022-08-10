using System;
using System.Activities;
using System.Activities.Expressions;
using System.Activities.Presentation;
using System.Activities.Presentation.Model;
using System.Activities.Presentation.Services;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Microsoft.VisualBasic.Activities;

namespace UiPath.Shared.Activities.Design.Services;

public static class ModelItemExtensions
{
   public static T GetInArgumentValue<T>(this ModelItem modelItem, string propertyName)
   {
      var argument = ((InArgument<T>)modelItem.Properties[propertyName]?.Value?.GetCurrentValue())?.Expression;
      switch ( argument )
      {
         case Literal<T> literal:
            return literal.Value;
         case VisualBasicValue<T> variable:
         {
            var argName = variable.ExpressionText;

            return GetDefaultVariableValue<T>(modelItem, argName);
         }
         default:
            return default(T);
      }
   }

   public static bool ReplaceModelItem(this ModelItem toFind, object replaceWith) =>
       ReplaceModelItem(toFind.Parent, toFind, replaceWith);

   public static ModelItem GetParent<T>(this ModelItem modelItem) where T : Activity
   {
      if (modelItem == null)
         return null;

      var item = modelItem;
      var typeToFind = typeof(T);
      while (item.Parent != null && item.ItemType != typeToFind)
      {
         item = item.Parent;
      }

      return item?.ItemType == typeToFind ? item : null;
   }

   public static IEnumerable<ModelItem> GetChildren<T>(this ModelItem root) where T : Activity
   {
      var type = typeof(T);
      return FindBreadthFirst(root, m => type == m.ItemType).ToList();
   }

   public static ModelItem GetClosestBefore<T>(this ModelItem root, ModelItem refModelItem) =>
       GetClosestBefore(root, typeof(T), refModelItem);

   public static ModelItem GetClosestBefore(this ModelItem root, Type matchingType, ModelItem refModelItem) =>
       FindBefore(root, m => m.ItemType == matchingType, m => m == refModelItem).LastOrDefault();

   public static bool Is<T>(this ModelItem modelItem) where T : Activity
       => modelItem?.ItemType == typeof(T);

   public static void UpdatePropertyForAll<T>(EditingContext context, Type modelItemType,
                                       string propertyName, object value, Func<ModelItem, bool> excluded)
   {
      var modelService = context.Services.GetService<ModelService>();
      modelService.Root.UpdatePropertyForAll<T>(context, modelItemType, propertyName, value, excluded);
   }

   public static void UpdatePropertyForAll<T>(this ModelItem rootItem, EditingContext context, Type modelItemType,
                                       string propertyName, object value, Func<ModelItem, bool> excluded)
   {
      var modelService = context.Services.GetService<ModelService>();
      var items = modelService.Find(rootItem, modelItemType).ToList();
      if (excluded != null)
         items = items.Where(i => !excluded(i)).ToList();

      foreach (var item in items)
      {
         var property = item.Properties[propertyName];
         if (property == null)
            continue;

         var crtValue = property.Value?.GetCurrentValue();
         if (PropertyEquals<T>(value, crtValue))
            continue;

         var valueToSet = GetValueToSet<T>(value);
         property.SetValue(valueToSet);
      }
   }

   public static void CopyProperty<T>(this ModelItem source, ModelItem destination, string propertyName)
   {
      var srcProperty = source.Properties[propertyName];
      if (srcProperty == null)
         return;

      var value = srcProperty.Value?.GetCurrentValue();

      var property = destination.Properties[propertyName];
      if (property == null)
         return;

      var crtValue = property.Value?.GetCurrentValue();
      if (PropertyEquals<T>(value, crtValue))
         return;

      var valueToSet = GetValueToSet<T>(value);
      property.SetValue(valueToSet);
   }

   private static object GetValueToSet<T>(object value)
   {
      return value switch
      {
         InArgument<T> inArgument => new InArgument<T>( ( Activity<T> )GetValueToSet<T>( inArgument.Expression ) ),
         Literal<T> literal => new Literal<T>( literal.Value ),
         VisualBasicValue<T> vbValue => new VisualBasicValue<T>( vbValue.ExpressionText ),
         ActivityDelegate => Xaml.Clone( ( T )value ),
         _ => value
      };
   }

   private static bool PropertyEquals<T>(object value1, object value2)
   {
      return value1 switch
      {
         InArgument<T> inArgument1 when value2 is InArgument<T> inArgument2 => PropertyEquals<T>(
            inArgument1.Expression, inArgument2.Expression ),
         Literal<T> literal1 when value2 is Literal<T> literal2 => Equals( literal1.Value, literal2.Value ),
         VisualBasicValue<T> vbValue1 when value2 is VisualBasicValue<T> vbValue2 => vbValue1.ExpressionText ==
            vbValue2.ExpressionText,
         _ => false
      };
   }

   private static bool ReplaceModelItem(ModelItem parent, ModelItem toFind, object replaceWith)
   {
      if (parent == null)
         return false;

      // search in properties
      var prop = parent.Properties.FirstOrDefault(p => p.Value == toFind);
      if (prop != null)
      {
         parent.Properties[prop.Name].SetValue(replaceWith);
         return true;
      }

      // search in collection properties
      foreach (var collection in parent.Properties
          .Where(p => p.IsCollection && p.Collection != null)
          .Select(p => p.Collection))
      {
         var idx = collection.IndexOf(toFind);
         if (idx == -1)
            continue;

         // ensure collection update is done as a single transaction
         // such that Undo in the designer will undo both changes at once
         using var scope = collection.BeginEdit();
         
         collection.Insert(idx, replaceWith);
         collection.Remove(toFind);

         scope.Complete();

         return true;
      }

      return ReplaceModelItem(parent.Parent, toFind, replaceWith);
   }

   private static T GetDefaultVariableValue<T>(ModelItem modelItem, string variableName)
   {
      foreach (var collection in modelItem.Properties
                                        .Where(p => p.IsCollection && p.PropertyType == typeof(Collection<Variable>))
                                        .Select(p => p.ComputedValue as Collection<Variable>)
                                        .Where(c => c != null))
      {
         if (collection.FirstOrDefault(v => v.Name == variableName) is Variable<T> { Default: Literal<T> variableDefault } )
            return variableDefault.Value;
      }

      var parent = modelItem.Parent;
      return parent == null ? default(T) : GetDefaultVariableValue<T>(parent, variableName);
   }

   private static IEnumerable<ModelItem> FindBreadthFirst(ModelItem startingItem,
                                                          Func<ModelItem, bool> matcher,
                                                          Func<ModelItem, bool> exitCondition = null)
   {
      var foundItems = new List<ModelItem>();
      var modelItems = new Queue<ModelItem>();
      modelItems.Enqueue(startingItem);
      var alreadyVisited = new HashSet<ModelItem>();
      while (modelItems.Count > 0)
      {
         var currentModelItem = modelItems.Dequeue();
         if (currentModelItem == null)
         {
            continue;
         }

         if (exitCondition?.Invoke(currentModelItem) == true)
            break;

         if (matcher(currentModelItem))
         {
            foundItems.Add(currentModelItem);
         }

         var children = GetChildren(currentModelItem);

         foreach ( var child in children.Where( child => !alreadyVisited.Contains(child) ) )
         {
            alreadyVisited.Add(child);
            modelItems.Enqueue(child);
         }
      }

      return foundItems;
   }

   private static IEnumerable<ModelItem> GetChildren(ModelItem modelItem)
   {
      var neighbors = new List<ModelItem>();

      // do not search through Type and its derivatives
      if (typeof(Type).IsAssignableFrom(modelItem.ItemType))
      {
         return neighbors;
      }

      if (modelItem is ModelItemCollection collection)
      {
         neighbors.AddRange(collection.Where(m => m != null));
      }

      var dictionary = modelItem as ModelItemDictionary;
      if (dictionary != null)
      {
         foreach (var (miKey, miValue) in dictionary)
         {
            if (miKey != null)
            {
               neighbors.Add(miKey);
            }

            if (miValue != null)
            {
               neighbors.Add(miValue);
            }
         }
      }

      neighbors.AddRange( from property in modelItem.Properties where dictionary == null || ! string.Equals( property.Name, "ItemsCollection" ) where ! typeof( Type ).IsAssignableFrom( property.PropertyType ) && ! property.PropertyType.IsValueType where property.Value != null select property.Value );

      return neighbors;
   }

   private static IEnumerable<ModelItem> FindBefore(ModelItem startingItem,
                                                    Func<ModelItem, bool> matcher,
                                                    Func<ModelItem, bool> before)
   {
      var lst = new List<ModelItem>();
      FindBeforeInternal(startingItem, matcher, before, new HashSet<ModelItem>(), lst);
      return lst;
   }

   private static bool FindBeforeInternal(ModelItem startingItem,
                                            Func<ModelItem, bool> matcher,
                                            Func<ModelItem, bool> toFind,
                                            ISet<ModelItem> visited,
                                            List<ModelItem> matchingItems)
   {
      visited.Add(startingItem);

      foreach ( var child in GetChildren(startingItem).Where( child => ! visited.Contains(child) ) )
      {
         if (toFind(child))
            return true;

         if (matcher(child))
            matchingItems.Add(child);

         var innerMatches = new List<ModelItem>();
         
         if ( ! FindBeforeInternal( child, matcher, toFind, visited, innerMatches ) ) continue;
         
         matchingItems.AddRange(innerMatches);
         return true;
      }

      return false;
   }
}