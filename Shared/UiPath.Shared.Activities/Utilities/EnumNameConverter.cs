using System;
using System.ComponentModel;
using System.Globalization;

namespace UiPath.Shared.Activities.Utilities;

public class EnumNameConverter<T> : EnumConverter
{
   public EnumNameConverter() : base(typeof(T))
   {

   }

   public EnumNameConverter(Type type) : base(type)
   {
   }

   public override bool CanConvertTo(ITypeDescriptorContext context, Type destType)
   {
      return destType == typeof(string);
   }

   public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destType)
   {
      var fieldInfo = EnumType.GetField(Enum.GetName(EnumType, value!)!);
      var description = (DescriptionAttribute)Attribute.GetCustomAttribute(fieldInfo!, typeof(DescriptionAttribute));

      return description != null ? description.Description : value.ToString();
   }

   public override bool CanConvertFrom(ITypeDescriptorContext context, Type srcType)
   {
      return srcType == typeof(string);
   }

   public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
   {
      foreach (var fieldInfo in EnumType.GetFields())
      {
         var description = (DescriptionAttribute)Attribute.GetCustomAttribute(fieldInfo, typeof(DescriptionAttribute));

         if (description != null && (string)value == description.Description)
            return Enum.Parse(EnumType, fieldInfo.Name);
      }

      return Enum.Parse(EnumType, ((string)value)!);
   }
}