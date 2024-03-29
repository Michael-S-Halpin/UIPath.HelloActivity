using System;
using System.Activities;
using System.Activities.Presentation.View;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using UiPath.HelloActivity.Enums;
using UiPath.Shared.Activities.Design.Controls;

namespace UiPath.HelloActivity.Activities.Design.Designers;

/// <summary>
/// Interaction logic for TestActivityDesigner.xaml
/// </summary>
public partial class TestActivityDesigner
{
    internal static IEnumerable<TestEnum> ComboSelectionEnums => Enum.GetValues(typeof(TestEnum)).Cast<TestEnum>();

    internal static IEnumerable<string> OptionSelections => new List<string>( new[] { "Michael", "Joel", "Stephanie", "Samantha" } );

    /// <summary>
    /// Default constructor for TestActivityDesigner.
    /// </summary>
    public TestActivityDesigner()
    {
        InitializeComponent();
    }
    
    private void OptionSelection_SelectionChanged( object sender, RoutedEventArgs routedEventArgs )
    {
        var cbo = ( ComboboxControl )sender;
        var name = cbo.PropertyName.Replace( " ", "" );
        var selectedValue = ((routedEventArgs.OriginalSource as System.Windows.Controls.ComboBox)!).SelectedValue;
        
        if ( selectedValue == null ) return;
        
        ModelItem.Properties[name].SetValue(new InArgument<string>(selectedValue.ToString()));
    }

    private void TypeSelection_PropertyChanged( object sender, PropertyChangedEventArgs propertyChangedEventArgs )
    {
        var typePresenter = (TypePresenter)sender;
        var name = typePresenter.Label.Replace( " ", "" );
        
        ModelItem.Properties[name].SetValue(typePresenter.Type);
    }

    private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
    {
        var fpd = new FolderPicker
        {
            InputPath = @"c:\windows\system32"
        };
        
        if (fpd.ShowDialog() == true)
        {
            ModelItem.Properties["FolderPath"].SetValue( new InArgument<string>( fpd.ResultPath ) );
        }
    }
}