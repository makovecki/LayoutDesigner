using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace LayoutDesigner.Controls
{
    public class LayoutDesignerControl:Control
    {
        public LayoutDesignerControl()
        {
            DefaultStyleKey = typeof(LayoutDesignerControl);
        }



        public bool ShowAvailableItems
        {
            get { return (bool)GetValue(ShowAvailableItemsProperty); }
            set { SetValue(ShowAvailableItemsProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ShowAvailableItems.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ShowAvailableItemsProperty =
            DependencyProperty.Register("ShowAvailableItems", typeof(bool), typeof(LayoutDesignerControl), new PropertyMetadata(false,ShowAvailableItemsPropertyChanged));

        private static void ShowAvailableItemsPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as LayoutDesignerControl;
            VisualStateManager.GoToState(control, (e.NewValue as bool?)==true?"ListOpen": "ListClosed", true);
        }
    }
}
