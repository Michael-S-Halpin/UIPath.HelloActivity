using Microsoft.Win32;
using System;
using System.Activities;
using System.Activities.Presentation.Model;
using System.IO;
using System.Windows;

namespace UiPath.Shared.Activities.Design.Controls;

/// <summary>
/// Interaction logic for FilePathControl.xaml
/// </summary>
public partial class FilePathControl
{
   public static readonly DependencyProperty ModelItemProperty = DependencyProperty.Register("ModelItem", typeof(ModelItem), typeof(FilePathControl));

   public ModelItem ModelItem
   {
      get => GetValue(ModelItemProperty) as ModelItem;
      set => SetValue(ModelItemProperty, value);
   }

   public static readonly DependencyProperty ExpressionProperty = DependencyProperty.Register("Expression", typeof(ModelItem), typeof(FilePathControl));

   public ModelItem Expression
   {
      get => GetValue(ExpressionProperty) as ModelItem;
      set => SetValue(ExpressionProperty, value);
   }

   public static readonly DependencyProperty PropertyNameProperty = DependencyProperty.Register("PropertyName", typeof(string), typeof(FilePathControl));

   public string PropertyName
   {
      get => GetValue(PropertyNameProperty) as string;
      set => SetValue(PropertyNameProperty, value);
   }

   public static readonly DependencyProperty HintTextProperty = DependencyProperty.Register("HintText", typeof(string), typeof(FilePathControl), new PropertyMetadata("Text must be quoted"));
   public string HintText
   {
      get => GetValue(HintTextProperty) as string;
      set => SetValue(HintTextProperty, value);
   }
   public static readonly DependencyProperty FileNameProperty = DependencyProperty.Register("FileName", typeof(string), typeof(FilePathControl));
   public string FileName
   {
      get => GetValue(FileNameProperty) as string;
      set => SetValue(FileNameProperty, value);
   }
   public static readonly DependencyProperty FilterProperty = DependencyProperty.Register("Filter", typeof(string), typeof(FilePathControl));
   public string Filter
   {
      get => GetValue(FilterProperty) as string;
      set => SetValue(FilterProperty, value);
   }
   public static readonly DependencyProperty TitleProperty = DependencyProperty.Register("Title", typeof(string), typeof(FilePathControl));
   public string Title
   {
      get => GetValue(TitleProperty) as string;
      set => SetValue(TitleProperty, value);
   }
   public static readonly RoutedEvent OpenEvent = EventManager.RegisterRoutedEvent("Open", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(FilePathControl));
   public event RoutedEventHandler Open
   {
      add => AddHandler(OpenEvent, value);
      remove => RemoveHandler(OpenEvent, value);
   }
   public static readonly RoutedEvent FileSelectedEvent = EventManager.RegisterRoutedEvent("FileSelected", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(FilePathControl));
   public event RoutedEventHandler FileSelected
   {
      add => AddHandler(FileSelectedEvent, value);
      remove => RemoveHandler(FileSelectedEvent, value);
   }
   public bool IsSaveDialog { get; set; }
   public bool? ValidateNames { get; set; }
   public bool? CheckFileExists { get; set; }
   public bool? CheckPathExists { get; set; }
   public FilePathControl()
   {
      Title = "Select File";
      Loaded += (s, e) =>
      {
         FileNameTextBox!.HintText = HintText;
      };
      InitializeComponent();
   }
   private void LoadButton_Click(object sender, RoutedEventArgs e)
   {
      var args = new RoutedEventArgs(OpenEvent)
      {
         Source = LoadButton
      };
      RaiseEvent(args);
      if (args.Handled) return;

      FileDialog fileDialog;

      if (IsSaveDialog)
      {
         fileDialog = new SaveFileDialog() { OverwritePrompt = false };
      }
      else
      {
         fileDialog = new OpenFileDialog();
      }

      Filter ??= "All files (*.*)|*.*";

      fileDialog.Filter = Filter;
      fileDialog.FileName = FileName;
      fileDialog.Title = Title;

      if (ValidateNames != null)
      {
         fileDialog.ValidateNames = ValidateNames.Value;
      }

      if (CheckFileExists != null)
      {
         fileDialog.CheckFileExists = CheckFileExists.Value;
      }

      if (CheckPathExists != null)
      {
         fileDialog.CheckPathExists = CheckPathExists.Value;
      }

      var workspacePath = Directory.GetCurrentDirectory();
      fileDialog.InitialDirectory = workspacePath;

      var result = fileDialog.ShowDialog();
      if (result.HasValue && result.Value)
      {
         var workflowRelativePath = GetRelativePath(workspacePath, fileDialog.FileName);

         RoutedEventArgs fileArgs = new FileSelectedRoutedEventArgs(FileSelectedEvent, workflowRelativePath);
         fileArgs.Source = LoadButton;
         RaiseEvent(fileArgs);

         if (!fileArgs.Handled && PropertyName != null)
         {
            ModelItem.Properties[PropertyName].SetValue(new InArgument<string>(workflowRelativePath));
         }
      }
   }

   public static string GetRelativePath(string basePath, string path)
   {
      if (string.IsNullOrWhiteSpace(basePath)) return path;

      var workflowRelativePath = path;
      try
      {
         var workspaceUri = new Uri(Path.Combine(basePath, Path.PathSeparator.ToString()));
         var workflowUri = new Uri(path);

         // the workflow is in the current workspace
         if (workspaceUri.IsBaseOf(workflowUri))
         {
            workflowRelativePath = Uri.UnescapeDataString(workspaceUri.MakeRelativeUri(workflowUri).OriginalString).Replace('/', Path.DirectorySeparatorChar);
         }
      }
      catch { }

      return workflowRelativePath;
   }
}

public class FileSelectedRoutedEventArgs : RoutedEventArgs
{
   public string Path { get; internal set; }

   public FileSelectedRoutedEventArgs(RoutedEvent routedEvent, string path)
       : base(routedEvent)
   {
      Path = path;
   }
}