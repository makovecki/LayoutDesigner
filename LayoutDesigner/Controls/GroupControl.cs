using LayoutDesigner.DragingDroping;
using LayoutDesigner.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Automation.Peers;
using System.Windows.Controls;

namespace LayoutDesigner.Controls
{
    public class GroupControl: Control, IDragTarget
    {
        public GroupControl()
        {
            DefaultStyleKey = typeof(GroupControl);
            Items = new ObservableCollection<IItem>();
        }



        public Orientation Orientation
        {
            get { return (Orientation)GetValue(OrientationProperty); }
            set { SetValue(OrientationProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Orientation.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty OrientationProperty =
            DependencyProperty.Register("Orientation", typeof(Orientation), typeof(GroupControl), new PropertyMetadata(Orientation.Horizontal));

        public void HideInsertionPlacers()
        {
            ShowLeftInsertionPoint = false;
            ShowRightInsertionPoint = false;
            ShowTopInsertionPoint = false;
            ShowBottomInsertionPoint = false;
        }

        public void ShowNearestInsertionPlace(Point point, Rect controlDimension)
        {
            HideInsertionPlacers();
            if (!Items.Any()) ShowLeftInsertionPoint = true;
            if (Items.Any() && Orientation == Orientation.Horizontal)
            {
                if (controlDimension.Width > 60 && point.X > 0 && point.X < 30) ShowLeftInsertionPoint = true;
                if (controlDimension.Width > 60 && point.X > controlDimension.Width-30) ShowRightInsertionPoint = true;
            }
        }

        void IDragTarget.Drop(IItem item)
        {
            if (Items.Count == 1 && (ShowLeftInsertionPoint || ShowRightInsertionPoint)) Orientation = Orientation.Horizontal;
            if (Items.Count == 1 && (ShowTopInsertionPoint || ShowBottomInsertionPoint)) Orientation = Orientation.Vertical;
            if (ShowLeftInsertionPoint || ShowTopInsertionPoint)
            {
                Items.Insert(0, item);
            }
            if (ShowRightInsertionPoint || ShowBottomInsertionPoint)
            {
                Items.Add(item);
            }
            HideInsertionPlacers();
        }

        public ObservableCollection<IItem> Items
        {
            get { return (ObservableCollection<IItem>)GetValue(ItemsProperty); }
            set { SetValue(ItemsProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Items.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ItemsProperty =
            DependencyProperty.Register("Items", typeof(ObservableCollection<IItem>), typeof(GroupControl), new PropertyMetadata(null));



        public bool ShowLeftInsertionPoint
        {
            get { return (bool)GetValue(ShowLeftInsertionPointProperty); }
            set { SetValue(ShowLeftInsertionPointProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ShowLeftInsertionPoint.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ShowLeftInsertionPointProperty =
            DependencyProperty.Register("ShowLeftInsertionPoint", typeof(bool), typeof(GroupControl), new PropertyMetadata(false));

        public bool ShowRightInsertionPoint
        {
            get { return (bool)GetValue(ShowRightInsertionPointProperty); }
            set { SetValue(ShowRightInsertionPointProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ShowRightInsertionPoint.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ShowRightInsertionPointProperty =
            DependencyProperty.Register("ShowRightInsertionPoint", typeof(bool), typeof(GroupControl), new PropertyMetadata(false));

        public bool ShowTopInsertionPoint
        {
            get { return (bool)GetValue(ShowTopInsertionPointProperty); }
            set { SetValue(ShowTopInsertionPointProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ShowTopInsertionPoint.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ShowTopInsertionPointProperty =
            DependencyProperty.Register("ShowTopInsertionPoint", typeof(bool), typeof(GroupControl), new PropertyMetadata(false));


        public bool ShowBottomInsertionPoint
        {
            get { return (bool)GetValue(ShowBottomInsertionPointProperty); }
            set { SetValue(ShowBottomInsertionPointProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ShowBottomInsertionPoint.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ShowBottomInsertionPointProperty =
            DependencyProperty.Register("ShowBottomInsertionPoint", typeof(bool), typeof(GroupControl), new PropertyMetadata(false));



    }
}
