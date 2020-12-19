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
            foreach (UIElement child in Children)
                child.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));

            return new Size(0, 0);
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

            //double remainingSpace = Math.Max(0.0, finalSize.Height - Children.Cast<UIElement>().Sum(c => c.DesiredSize.Height));
            //var extraSpace = remainingSpace / Children.Count;
            //double offset = 0.0;

            //foreach (UIElement child in Children)
            //{
            //    double height = child.DesiredSize.Height + extraSpace;
            //    child.Arrange(new Rect(0, offset, finalSize.Width, height));
            //    offset += height;
            //}

            return finalSize;
        }
    }
}
