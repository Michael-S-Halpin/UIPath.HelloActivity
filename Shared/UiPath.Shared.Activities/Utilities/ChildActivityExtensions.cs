using System.Activities;

namespace UiPath.Shared.Activities.Utilities;

/// <summary>Provides the standard functionality across activities.</summary>
public static class ChildActivityExtensions
{
   /// <summary>
   /// Get object from context.UserState object. 
   /// According to documentation, this is the place to store object for multithreaded access
   /// </summary>
   public static T GetFromUserState<T>(this AsyncCodeActivityContext context) where T : class
   {
      if (context.UserState is T obj)
         return obj;
      else return null;
   }

   /// <summary>
   /// Set context.UserState to desired value
   /// </summary>
   /// <typeparam name="T"></typeparam>
   /// <param name="context"></param>
   /// <param name="value"></param>
   public static void SetToUserState<T>(this AsyncCodeActivityContext context, T value) where T : class
   {
      context.UserState = value;
   }

   /// <summary>
   /// Get object from Context properties
   /// </summary>
   /// <param name="context"></param>
   /// <param name="propertyName"></param>
   /// <returns></returns>
   public static T GetFromContext<T>(this ActivityContext context, string propertyName) where T : class
   {
      var property = context.DataContext.GetProperties()[propertyName];
      var obj = property!.GetValue(context.DataContext) as T;
      return obj;
   }
}