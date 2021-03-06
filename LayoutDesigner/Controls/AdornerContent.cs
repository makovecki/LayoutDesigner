﻿using LayoutDesigner.DragingDroping;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;

namespace LayoutDesigner.Controls
{
    public class AdornerContent : Adorner
    {
        private List<UIElement> visuals;
        private ContentPresenter contentPresenter;
   
        public AdornerContent(UIElement adornedElement, IDragSource content, DataTemplate template) : base(adornedElement)
        {
            visuals = new List<UIElement>();
            contentPresenter = new ContentPresenter { Content = content, ContentTemplate=template };
            visuals.Add(contentPresenter);
            this.IsHitTestVisible = false;
        }

        protected override Size MeasureOverride(Size constraint)
        {
            contentPresenter.Measure(constraint);
            return contentPresenter.DesiredSize;
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            contentPresenter.Arrange(new Rect(0, 0, finalSize.Width, finalSize.Height));
            return contentPresenter.RenderSize;
        }

        protected override Visual GetVisualChild(int index)
        {
            return visuals[index];
        }

        protected override int VisualChildrenCount => visuals.Count;

        

        
    }
}
