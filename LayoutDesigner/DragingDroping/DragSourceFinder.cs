using LayoutDesigner.Controls;
using LayoutDesigner.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace LayoutDesigner.DragingDroping
{
    public static class DragSourceFinder
    {
        internal static DragSourceInfo GetDragSource(object frameworkElement, DataTemplate template)
        {
            if (frameworkElement is FrameworkElement element && element.DataContext is IDragSource item)
            {
                var root = XAMLHelper.FindParent<LayoutDesignerControl>(element);
                return new DragSourceInfo(item, new AdornerContent(root, item, template),root);
            }

            return null;
        }
    }
}
