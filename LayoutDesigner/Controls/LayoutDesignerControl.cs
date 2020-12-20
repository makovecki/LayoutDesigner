using LayoutDesigner.Model;
using LayoutDesigner.Utils;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;

namespace LayoutDesigner.Controls
{
    
    public class LayoutDesignerControl : Control
    {
        
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
        Debouncer eventDebouncer = new Debouncer();
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





        





    }
}
