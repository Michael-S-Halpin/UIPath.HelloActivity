using System.Activities.Presentation;
using System.Activities.Presentation.Converters;
using System.Activities.Presentation.Model;
using System.Activities.Presentation.PropertyEditing;
using System.Windows;

namespace UiPath.Shared.Activities.Design.Editors;

public class ArgumentDictionaryEditor : DialogPropertyValueEditor
{
   private static readonly DataTemplate EditorTemplate = (DataTemplate)new EditorTemplates()["ArgumentDictionaryEditor"];

   public ArgumentDictionaryEditor()
   {
      InlineEditorTemplate = EditorTemplate;
   }

   public override void ShowDialog(PropertyValue propertyValue, IInputElement commandSource)
   {
      var propertyName = propertyValue.ParentProperty.PropertyName;

      var ownerActivity = (new ModelPropertyEntryToOwnerActivityConverter()).Convert(
          propertyValue.ParentProperty, typeof(ModelItem), false, null) as ModelItem;

      var options = new DynamicArgumentDesignerOptions()
      {
         Title = propertyName
      };

      ModelItem modelItem = ownerActivity!.Properties[propertyName].Dictionary;

      using var change = modelItem.BeginEdit(propertyName + "Editing");
      
      if (DynamicArgumentDialog.ShowDialog(ownerActivity, modelItem, ownerActivity.GetEditingContext(), ownerActivity.View, options))
      {
         change.Complete();
      }
      else
      {
         change.Revert();
      }
   }
   public static void ShowDialog(string propertyName, ModelItem ownerActivity)
   {
      var options = new DynamicArgumentDesignerOptions()
      {
         Title = propertyName
      };

      var modelItem = ownerActivity.Properties[propertyName].Value;

      using var change = modelItem.BeginEdit(propertyName + "Editing");
      
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