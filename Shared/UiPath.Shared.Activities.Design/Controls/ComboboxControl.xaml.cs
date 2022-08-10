using System;
using System.Activities.Presentation.Model;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace UiPath.Shared.Activities.Design.Controls;

/// <summary>
/// Interaction logic for ComboboxControl.xaml
/// </summary>

public partial class ComboboxControl : INotifyPropertyChanged
{
   public static readonly DependencyProperty ModelItemProperty = DependencyProperty.Register("ModelItem", typeof(ModelItem), typeof(ComboboxControl));

   public ModelItem ModelItem
   {
      get => GetValue(ModelItemProperty) as ModelItem;
      set => SetValue(ModelItemProperty, value);
   }

   public static readonly DependencyProperty SelectedItemProperty = DependencyProperty.Register("SelectedItem", typeof(ModelItem), typeof(ComboboxControl));

   public ModelItem SelectedItem
   {
      get => GetValue(SelectedItemProperty) as ModelItem;
      set => SetValue(SelectedItemProperty, value);
   }

   public static readonly DependencyProperty PropertyNameProperty = DependencyProperty.Register("PropertyName", typeof(string), typeof(ComboboxControl));

   public string PropertyName
   {
      get => GetValue(PropertyNameProperty) as string;
      set => SetValue(PropertyNameProperty, value);
   }

   public static readonly DependencyProperty ItemsSourceProperty = DependencyProperty.Register("ItemsSource", typeof(List<string>), typeof(ComboboxControl));

   public List<string> ItemsSource
   {
      get => GetValue(ItemsSourceProperty) as List<string>;
      set => SetValue(ItemsSourceProperty, value);
   }

   public static readonly DependencyProperty HintTextProperty = DependencyProperty.Register("HintText", typeof(string), typeof(ComboboxControl), new PropertyMetadata("Text must be quoted"));

   public string HintText
   {
      get => GetValue(HintTextProperty) as string;
      set => SetValue(HintTextProperty, value);
   }

   private Geometry _buttonIcon;

   public Geometry ButtonIcon
   {
      get => _buttonIcon;
      set
      {
         _buttonIcon = value;
         NotifyOfPropertyChange(nameof(ButtonIcon));
      }
   }

   public static readonly RoutedEvent SelectionChangedEvent = EventManager.RegisterRoutedEvent("SelectionChanged", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(ComboboxControl));

   public event RoutedEventHandler SelectionChanged
   {
      add => AddHandler(SelectionChangedEvent, value);
      remove => RemoveHandler(SelectionChangedEvent, value);
   }

   public static readonly RoutedEvent DropDownOpenedEvent = EventManager.RegisterRoutedEvent("DropDownOpened", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(ComboboxControl));

   public event RoutedEventHandler DropDownOpened
   {
      add => AddHandler(DropDownOpenedEvent, value);
      remove => RemoveHandler(DropDownOpenedEvent, value);
   }

   public event PropertyChangedEventHandler PropertyChanged;

   protected virtual void NotifyOfPropertyChange(string propertyName)
   {
      PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
   }

   public ComboboxControl()
   {
      InitializeComponent();
      ButtonIcon = (Geometry)this.Resources["DownArrowGeometry"];
      Loaded += (s, e) =>
      {
         ComboboxInputBox.HintText = HintText;
      };
   }

   private void PropertiesComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
   {
      if ( e.AddedItems.Count == 0 ) return;
      
      var args = new RoutedEventArgs(SelectionChangedEvent)
      {
         Source = PropertiesComboBox
      };
      RaiseEvent(args);
      PropertiesComboBox.SelectedIndex = -1;
   }

   private void PropertiesComboBox_OnDropDownOpened(object sender, EventArgs e)
   {
      var args = new RoutedEventArgs(DropDownOpenedEvent) { Source = PropertiesComboBox };
      RaiseEvent(args);
   }

   private void Button_Click(object sender, RoutedEventArgs e)
   {
      PropertiesComboBox.IsDropDownOpen = true;
   }
}