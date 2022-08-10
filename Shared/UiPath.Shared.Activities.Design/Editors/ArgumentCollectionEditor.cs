using System.Activities.Presentation;
using System.Activities.Presentation.Converters;
using System.Activities.Presentation.Model;
using System.Activities.Presentation.PropertyEditing;
using System.Windows;
using UiPath.Shared.Localization;

namespace UiPath.Shared.Activities.Design.Editors;

public class ArgumentCollectionEditor : DialogPropertyValueEditor
{
   private static readonly DataTemplate EditorTemplate = (DataTemplate)new EditorTemplates()["ArgumentDictionaryEditor"];

   public ArgumentCollectionEditor()
   {
      this.InlineEditorTemplate = EditorTemplate;
   }

   public override void ShowDialog(PropertyValue propertyValue, IInputElement commandSource)
   {
      var propertyDisplayName = propertyValue.ParentProperty.DisplayName;
      var propertyName = propertyValue.ParentProperty.PropertyName;

      var ownerActivity = (new ModelPropertyEntryToOwnerActivityConverter()).Convert(propertyValue.ParentProperty, typeof(ModelItem), false, null) as ModelItem;

      ShowDialog(propertyName, propertyDisplayName, ownerActivity);
   }

   public static void ShowDialog(string propertyName, string propertyDisplayName, ModelItem ownerActivity)
   {
      var options = new DynamicArgumentDesignerOptions()
      {
         Title = propertyDisplayName
      };

      ModelItem modelItem = ownerActivity.Properties[propertyName].Collection;

      using var change = modelItem.BeginEdit(propertyDisplayName + SharedResources.Editing);
      
      if (DynamicArgumentDialog.ShowDialog(ownerActivity, modelItem, ownerActivity.GetEditingContext(), ownerActivity.View, options))
      {
         change.Complete();
      }
      else
      {
         change.Revert();
      }
   }
}