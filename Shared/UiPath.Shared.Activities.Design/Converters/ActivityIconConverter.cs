using System;
using System.Activities.Presentation.Model;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace UiPath.Shared.Activities.Design.Converters;

public class ActivityIconConverter : IValueConverter
{
   public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
   {
      try
      {
         if (value == null)
         {
            return null;
         }
         var activityType = ((value as ModelItem)!).ItemType;
         var resourceName = activityType.Name;

         if (activityType.IsGenericType)
         {
            resourceName = resourceName.Split('`')[0];
         }
         resourceName += "Icon";

         var iconsSource = new ResourceDictionary { Source = new Uri((parameter as string)!) };

         var icon = iconsSource[resourceName] as DrawingBrush ?? Application.Current.Resources[resourceName] as DrawingBrush;
         icon ??= Application.Current.Resources["GenericLeafActivityIcon"] as DrawingBrush;

         return icon!.Drawing;
      }
      catch
      {
         return null;
      }
   }

   public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
   {
      return Binding.DoNothing;
   }
}