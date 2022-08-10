using Microsoft.VisualBasic.Activities;
using System;
using System.Activities;
using System.Activities.Expressions;
using System.Activities.Presentation.Model;
using System.Globalization;
using System.Windows.Data;

namespace UiPath.Shared.Activities.Design.Converters; 

public class StringArgumentToComboboxConverter : IValueConverter
{
   public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
   {
      // Convert InArgument<string> to Combo box value
      var itemContent = string.Empty;
      var modelItem = value as ModelItem;

      if ( value == null ) return itemContent;
      
      if (modelItem!.GetCurrentValue() is InArgument<string> inArgument)
      {
         itemContent = inArgument.Expression switch
         {
            //to avoid extra double quotation marks
            VisualBasicValue<string> vbExpression when vbExpression!.ExpressionText.StartsWith( "\"" ) &&
                                                       vbExpression.ExpressionText.EndsWith( "\"" ) => vbExpression
               .ExpressionText.Replace( "\"", string.Empty ),
            VisualBasicValue<string> vbExpression => vbExpression.ExpressionText,
            Literal<string> literal => literal!.Value,
            _ => itemContent
         };
      }
      
      return itemContent;
   }

   public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
   {
      // Convert combo box value to InArgument<string>  
      var itemContent = "\"" + (string)value + "\"";
      var vbArgument = new VisualBasicValue<string>(itemContent);
      var inArgument = new InArgument<string>(vbArgument);
      
      return inArgument;
   }
}