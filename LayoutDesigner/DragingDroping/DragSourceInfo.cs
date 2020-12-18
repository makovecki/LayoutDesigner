using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;

namespace LayoutDesigner.DragingDroping
{
    public class DragSourceInfo
    {
        public DragSourceInfo(IDragSource source, Adorner adorner, FrameworkElement root)
        {
            Source = source;
            Adorner = adorner;
            Root = root;
        }
        public IDragSource Source { get; private set; }
        public Adorner Adorner { get; private set; }
        public FrameworkElement Root { get; private set; }
    }
}
