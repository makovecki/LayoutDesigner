using LayoutDesigner.Model;
using LayoutDesigner.Utils;
using System.Collections.ObjectModel;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;

namespace LayoutDesigner.Controls
{
    
    public class LayoutDesignerControl : Control
    {
        Debouncer eventDebouncer = new Debouncer();
        public LayoutDesignerControl()
        {
            DefaultStyleKey = typeof(LayoutDesignerControl);
            NewItems = new ObservableCollection<NewItem> { new NewItem { Name = "Skupina" } };
        }

        
        


        public ObservableCollection<NewItem> NewItems { get; set; }

        public bool ShowAvailableItems
        {
            get { return (bool)GetValue(ShowAvailableItemsProperty); }
            set { SetValue(ShowAvailableItemsProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ShowAvailableItems.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ShowAvailableItemsProperty =
            DependencyProperty.Register("ShowAvailableItems", typeof(bool), typeof(LayoutDesignerControl), new PropertyMetadata(false, ShowAvailableItemsPropertyChanged));

        private static void ShowAvailableItemsPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as LayoutDesignerControl;
            control.eventDebouncer.Debounce(50, () => {
                control.Dispatcher.Invoke(() => {
                    VisualStateManager.GoToState(control, (e.NewValue as bool?) == true ? "ListOpen" : "ListClosed", true);
                });
            });

        }



        public bool IsInEditMode
        {
            get { return (bool)GetValue(IsInEditModeProperty); }
            set { SetValue(IsInEditModeProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsInEditMode.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsInEditModeProperty =
            DependencyProperty.Register("IsInEditMode", typeof(bool), typeof(LayoutDesignerControl), new PropertyMetadata(false));

        private AdornerContent adorner;
        private FrameworkElement source;

        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                var point = e.GetPosition(this);
                
                var adLayer = AdornerLayer.GetAdornerLayer(this);
                adorner = new AdornerContent(this, new TextBlock { Text = "bbbbb" }); 
                adLayer.Add(adorner);

                DataObject dragData = new DataObject("myFormat", new NewItem() { Name = "aaaaa" });
                DragDrop.DoDragDrop(this, dragData, DragDropEffects.Copy);

                adLayer.Remove(adorner);
                
            }
            base.OnMouseMove(e);
        }
        private struct PInPoint
        {
            public int X;
            public int Y;
            public PInPoint(int x, int y)
            {
                X = x; Y = y;
            }
            public PInPoint(double x, double y)
            {
                X = (int)x; Y = (int)y;
            }
            public Point GetPoint(double xOffset = 0, double yOffet = 0)
            {
                return new Point(X + xOffset, Y + yOffet);
            }
            public Point GetPoint(Point offset)
            {
                return new Point(X + offset.X, Y + offset.Y);
            }
        }

        [DllImport("user32.dll")]
        static extern void GetCursorPos(ref PInPoint p);


        private PInPoint pointRef = new PInPoint();
        protected override void OnPreviewGiveFeedback(GiveFeedbackEventArgs e)
        {
            base.OnPreviewGiveFeedback(e);
            GetCursorPos(ref pointRef);
            Point relPos = this.PointFromScreen(pointRef.GetPoint(adorner.CenterOffset));
            adorner.Arrange(new Rect(relPos, adorner.DesiredSize));
        }

        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            if (e.OriginalSource is FrameworkElement element && element.DataContext is NewItem item)
            {
                source = element;
                e.Handled = true;
                

            }
            base.OnMouseLeftButtonDown(e);
        }

        protected override void OnMouseLeftButtonUp(MouseButtonEventArgs e)
        {
            
            base.OnMouseLeftButtonUp(e);
        }
        protected override void OnMouseLeave(MouseEventArgs e)
        {
            
            base.OnMouseLeave(e);
        }

        
    }
}
