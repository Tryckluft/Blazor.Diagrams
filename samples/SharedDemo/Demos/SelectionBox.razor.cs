using Blazor.Diagrams.Core.Models;
using Blazor.Diagrams.Core;
using System;
using System.Collections.Generic;
using System.Text;
using Blazor.Diagrams.Core.Geometry;

namespace SharedDemo.Demos
{
    public partial class SelectionBox
    {
        private Diagram _diagram = new Diagram();

        protected override void OnInitialized()
        {
            base.OnInitialized();

            LayoutData.Title = "Selection Box";
            LayoutData.Info = "";
            LayoutData.DataChanged();

            InitializeDiagram();
        }

        private void InitializeDiagram()
        {
            _diagram.Options.SelectionBox.MouseButton = MouseButtonEnum.Left;
            _diagram.Options.SelectionBox.MouseModifierKey = ModifierKeyEnum.None;
            _diagram.Options.Panning.MouseButton = MouseButtonEnum.Left;
            _diagram.Options.Panning.MouseModifierKey = ModifierKeyEnum.Shift;

            var node1 = new NodeModel(new Point(80, 80), shape: Shapes.Rectangle);
            var node2 = new NodeModel(new Point(280, 80), shape: Shapes.Rectangle);
            var node3 = new NodeModel(new Point(80, 280), shape: Shapes.Rectangle);
            var node4 = new NodeModel(new Point(280, 280), shape: Shapes.Rectangle);
            _diagram.Nodes.Add(node1);
            _diagram.Nodes.Add(node2);
            _diagram.Nodes.Add(node3);
            _diagram.Nodes.Add(node4);
            _diagram.Links.Add(new LinkModel(node1, node2)
            {
                SourceMarker = LinkMarker.Arrow,
                TargetMarker = LinkMarker.Arrow
            });
        }
    }
}
