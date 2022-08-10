using System.Windows;

namespace UiPath.Shared.Activities.Design.Editors;

public partial class EditorTemplates
{
   private static ResourceDictionary _resources;

   internal static ResourceDictionary ResourceDictionary => _resources ??= new EditorTemplates();

   public EditorTemplates()
   {
      InitializeComponent();
   }
}