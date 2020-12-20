using LayoutDesigner.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace LayoutDesigner.DragingDroping
{
    interface IDragTarget
    {
        
        void HideInsertionPlacers();
        void ShowNearestInsertionPlace(Point point, Rect controlDimension);
        void Drop(IItem item);
    }
}
