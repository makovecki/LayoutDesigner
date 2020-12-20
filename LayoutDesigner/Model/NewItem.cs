using LayoutDesigner.DragingDroping;

namespace LayoutDesigner.Model
{
    public class NewItem:IDragSource, IItem
    {
        public string Name { get; set; }
    }
}
