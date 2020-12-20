using LayoutDesigner.Model;
using LayoutDesigner.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;

namespace LayoutDesigner.DragingDroping
{
    public static class DragingDroping
    {

        public static DragSourceInfo DragSource { get; set; }

        public static bool GetIsDragSource(DependencyObject obj)
        {
            return (bool)obj.GetValue(IsDragSourceProperty);
        }

        public static void SetIsDragSource(DependencyObject obj, bool value)
        {
            obj.SetValue(IsDragSourceProperty, value);
        }

        // Using a DependencyProperty as the backing store for IsDragSource.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsDragSourceProperty =
            DependencyProperty.RegisterAttached("IsDragSource", typeof(bool), typeof(DragingDroping), new PropertyMetadata(false, IsDragSourceChanged));

        private static void IsDragSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            UIElement element = (UIElement)d;
            if ((bool)e.NewValue)
            {
                element.MouseLeftButtonDown += SourceOnMouseLeftButtonDown;
                element.MouseLeftButtonUp += SourceOnMouseLeftButtonUp;
                element.MouseMove += SourceOnMouseMove;
                element.PreviewGiveFeedback += SourceOnPreviewGiveFeedback;
            }
            else
            {
                element.MouseLeftButtonDown -= SourceOnMouseLeftButtonDown;
                element.MouseLeftButtonUp -= SourceOnMouseLeftButtonUp;
                element.MouseMove -= SourceOnMouseMove;
                element.PreviewGiveFeedback -= SourceOnPreviewGiveFeedback;
            }
        }

        private static void SourceOnMouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            DragSource = null;
        }

        private static void SourceOnMouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed && DragSource != null && sender is Visual v)
            {
                var adLayer = AdornerLayer.GetAdornerLayer(v);                
                adLayer.Add(DragSource.Adorner);

                DataObject dragData = new DataObject("Item", DragSource.Source);
                DragDrop.DoDragDrop(sender as DependencyObject, dragData, DragDropEffects.Copy); //When doDragDrop OnMouseMoveNotFired !!

                adLayer.Remove(DragSource.Adorner);
            }
        }

        private static void SourceOnPreviewGiveFeedback(object sender, GiveFeedbackEventArgs e)
        {
            DragSource?.Adorner.Arrange(new Rect(DragSource.Root.PointFromScreen(NativeMethods.GetMousePosition()), DragSource.Adorner.DesiredSize));
        }

        private static void SourceOnMouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            DragSource = DragSourceFinder.GetDragSource(e.OriginalSource,XAMLHelper.GetDataTemplate((DependencyObject)e.OriginalSource));
            if (DragSource != null) e.Handled = true;
        }



        public static bool GetIsDragTarget(DependencyObject obj)
        {
            return (bool)obj.GetValue(IsDragTargetProperty);
        }

        public static void SetIsDragTarget(DependencyObject obj, bool value)
        {
            obj.SetValue(IsDragTargetProperty, value);
        }

        // Using a DependencyProperty as the backing store for IsDragTarget.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsDragTargetProperty =
            DependencyProperty.RegisterAttached("IsDragTarget", typeof(bool), typeof(DragingDroping), new PropertyMetadata(false, IsDragTargetPropertyChanged));

        private static void IsDragTargetPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            UIElement element = (UIElement)d;
            if ((bool)e.NewValue)
            {
                element.AllowDrop = true;
                element.Drop += TargetDrop;
                element.DragEnter += TargetDragEnter;
                element.DragLeave += TargetDragLeave;
                element.DragOver += TargetDragOver;
                
            }
            else
            {
                element.Drop -= TargetDrop;
                element.DragEnter -= TargetDragEnter;
                element.DragLeave -= TargetDragLeave;
                element.DragOver -= TargetDragOver;
            }
        }

        

        private static void TargetDragOver(object sender, DragEventArgs e)
        {
            if (sender is IDragTarget target && sender is FrameworkElement element) target.ShowNearestInsertionPlace(e.GetPosition(sender as IInputElement), new Rect(0,0,element.ActualWidth,element.ActualHeight));
        }

        private static void TargetDragLeave(object sender, DragEventArgs e)
        {
            if (sender is IDragTarget target) target.HideInsertionPlacers();
            
        }

        private static void TargetDragEnter(object sender, DragEventArgs e)
        {
            
            
        }

        private static void TargetDrop(object sender, DragEventArgs e)
        {
            if (sender is IDragTarget target) target.Drop(e.Data.GetData("Item") as IItem);
        }
    }
}
