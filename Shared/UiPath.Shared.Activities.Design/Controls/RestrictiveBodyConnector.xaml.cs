﻿using System;
using System.Windows;
using System.Activities.Presentation;
using System.Activities.Presentation.Hosting;
using System.Windows.Media.Animation;

namespace UiPath.Shared.Activities.Design.Controls
{
   /// <summary>
   /// Interaction logic for RestrictiveBodyConnector.xaml
   /// </summary>
   public partial class RestrictiveBodyConnector
   {
      public static readonly DependencyProperty AllowedItemTypeProperty = DependencyProperty.Register("AllowedItemType", typeof(Type), typeof(RestrictiveBodyConnector), new UIPropertyMetadata(typeof(object)));
      public static readonly DependencyProperty ContextProperty = DependencyProperty.Register("Context", typeof(EditingContext), typeof(RestrictiveBodyConnector));

      public RestrictiveBodyConnector()
      {
         InitializeComponent();
      }

      private void CheckAnimate(DragEventArgs e, string storyboardResourceName)
      {
         if ( e.Handled ) return;
         
         if (Context != null && !Context.Items.GetValue<ReadOnlyState>().IsReadOnly && DragDropHelper.AllowDrop(e.Data, Context))
         {
            BeginStoryboard((Storyboard)Resources[storyboardResourceName]);
         }
         else
         {
            e.Effects = DragDropEffects.None;
            e.Handled = true;
         }
      }

      protected override void OnDragEnter(DragEventArgs e)
      {
         CheckAnimate(e, "Expand");
         dropTarget.Visibility = Visibility.Visible;
      }

      protected override void OnDragLeave(DragEventArgs e)
      {
         CheckAnimate(e, "Collapse");
         dropTarget.Visibility = Visibility.Collapsed;
      }

      protected override void OnDrop(DragEventArgs e)
      {
         dropTarget.Visibility = Visibility.Collapsed;
         base.OnDrop(e);
      }

      // Properties
      public Type AllowedItemType
      {
         get => (Type)GetValue(AllowedItemTypeProperty);
         set => SetValue(AllowedItemTypeProperty, value);
      }

      public EditingContext Context
      {
         get => (EditingContext)GetValue(ContextProperty);
         set => SetValue(ContextProperty, value);
      }
   }
}