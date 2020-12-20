using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace LayoutDesigner.Controls
{
    public class GroupPanel: Panel
    {


        public Orientation Orientation
        {
            get { return (Orientation)GetValue(OrientationProperty); }
            set { SetValue(OrientationProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Orientation.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty OrientationProperty =
            DependencyProperty.Register("Orientation", typeof(Orientation), typeof(GroupPanel), new PropertyMetadata(Orientation.Vertical));


        protected override Size MeasureOverride(Size availableSize)
        {
            double childHeight = 0.0;
            double childWidth = 0.0;
            var size = new Size(0, 0);
            foreach (UIElement child in Children)
            {
                child.Measure(new Size(availableSize.Width, availableSize.Height));
                if (Orientation == Orientation.Horizontal)
                {
                    childWidth += child.DesiredSize.Width;
                    if (child.DesiredSize.Height > childHeight) childHeight = child.DesiredSize.Height;
                }
                if (Orientation == Orientation.Vertical)
                {
                    childHeight += child.DesiredSize.Height;
                    if (child.DesiredSize.Width > childWidth) childWidth = child.DesiredSize.Width;
                }
                
            }

            size.Width = childWidth;
            size.Height = childHeight;
            if (Children.Count == 0)
            {
                size.Width = 200;
                size.Height = 200;
            };
            return size;
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            if (Orientation == Orientation.Horizontal)
            {
                double HorizontalRemainingSpace = finalSize.Width - Children.OfType<FrameworkElement>().Where(x =>  !double.IsNaN(x.Width)).Sum(x => x.DesiredSize.Width);
                int numberOfHorizontalStrethed = Children.OfType<FrameworkElement>().Count(f => f.HorizontalAlignment == HorizontalAlignment.Stretch && double.IsNaN(f.Width));
                double spacePerControlForHorizontalStretch = HorizontalRemainingSpace / numberOfHorizontalStrethed;
                if (spacePerControlForHorizontalStretch <= 0 || double.IsInfinity(spacePerControlForHorizontalStretch)) spacePerControlForHorizontalStretch = 0;

                double xOffset = 0;
                foreach (FrameworkElement child in Children.OfType<FrameworkElement>())
                {
                    double width = (child.HorizontalAlignment == HorizontalAlignment.Stretch && double.IsNaN(child.Width)) ? spacePerControlForHorizontalStretch : child.DesiredSize.Width;
                    child.Arrange(new Rect(xOffset, 0, width, (double.IsNaN(child.Height) && child.VerticalAlignment== VerticalAlignment.Stretch)?finalSize.Height: child.DesiredSize.Height));
                    xOffset += width;
                }
            }
            else
            {
                double VerticalRemainingSpace = finalSize.Height - Children.OfType<FrameworkElement>().Where(x => !double.IsNaN(x.Height)).Sum(x => x.DesiredSize.Height);
                int numberOfVerticalStrethed = Children.OfType<FrameworkElement>().Count(f => f.VerticalAlignment == VerticalAlignment.Stretch && double.IsNaN(f.Height));
                double spacePerControlForVerticalStretch = VerticalRemainingSpace / numberOfVerticalStrethed;
                if (spacePerControlForVerticalStretch <= 0 || double.IsInfinity(spacePerControlForVerticalStretch)) spacePerControlForVerticalStretch = 0;

                double yOffset = 0;
                foreach (FrameworkElement child in Children.OfType<FrameworkElement>())
                {
                    double height = (child.VerticalAlignment == VerticalAlignment.Stretch && double.IsNaN(child.Height)) ? spacePerControlForVerticalStretch : child.DesiredSize.Height;
                    child.Arrange(new Rect(0, yOffset, (double.IsNaN(child.Width) && child.HorizontalAlignment == HorizontalAlignment.Stretch)?finalSize.Width: child.DesiredSize.Width, height));
                    yOffset += height;
                }
            }

            

            return finalSize;
        }
    }
}
