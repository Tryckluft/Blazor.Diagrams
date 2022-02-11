using Blazor.Diagrams.Core;
using Blazor.Diagrams.Core.Geometry;
using Blazor.Diagrams.Core.Models.Base;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using System;

namespace Blazor.Diagrams.Components
{
    public partial class SelectionBoxWidget : IDisposable
    {
        private Point _initialClientPoint;
        private Point _selectionBoxTopLeft;
        private Size _selectionBoxSize;

        [CascadingParameter]
        public Diagram Diagram { get; set; }

        [Parameter]
        public string Background { get; set; } = "rgb(110 159 212 / 25%);";

        protected override void OnInitialized()
        {
            Diagram.MouseDown += OnMouseDown;
            Diagram.MouseMove += OnMouseMove;
            Diagram.MouseUp += OnMouseUp;
            Diagram.TouchStart += OnTouchStart; //TODO: Test SelectionBox touch controls
            Diagram.TouchMove += OnTouchmove;
            Diagram.TouchEnd += OnTouchEnd;
        }

        private void OnTouchStart(Model model, TouchEventArgs e)
        {
            if (model != null)
                return;

            var options = Diagram.Options.Panning;

            switch (options.TouchModifierKey)
            {
                case ModifierKeyEnum.None:
                    if (e.CtrlKey || e.ShiftKey || e.AltKey)
                        return;
                    break;
                case ModifierKeyEnum.Ctrl:
                    if (!e.CtrlKey)
                        return;
                    break;
                case ModifierKeyEnum.Shift:
                    if (!e.ShiftKey)
                        return;
                    break;
                case ModifierKeyEnum.Alt:

                    if (!e.AltKey)
                        return;
                    break;
            }

            Start(e.ChangedTouches[0].ClientX, e.ChangedTouches[0].ClientY);
        }

        private void OnTouchmove(Model model, TouchEventArgs e)
            => Move(e.ChangedTouches[0].ClientX, e.ChangedTouches[0].ClientY);

        private void OnTouchEnd(Model model, TouchEventArgs e) => End();

        private void OnMouseDown(Model model, MouseEventArgs e)
        {
            if (model != null)
                return;

            var options = Diagram.Options.SelectionBox;

            if (e.Button != (long)options.MouseButton)
                return;

            switch (options.MouseModifierKey)
            {
                case ModifierKeyEnum.None:
                    if (e.CtrlKey || e.ShiftKey || e.AltKey)
                        return;
                    break;
                case ModifierKeyEnum.Ctrl:
                    if (!e.CtrlKey)
                        return;
                    break;
                case ModifierKeyEnum.Shift:
                    if (!e.ShiftKey)
                        return;
                    break;
                case ModifierKeyEnum.Alt:
                    if (!e.AltKey)
                        return;
                    break;
            }

            Start(e.ClientX, e.ClientY);
        }

        private void OnMouseMove(Model model, MouseEventArgs e)
        {
            Move(e.ClientX, e.ClientY);
        }


        private void OnMouseUp(Model model, MouseEventArgs e)
        {
            End();
        }

        private void Start(double clientX, double clientY)
        {
            _initialClientPoint = new Point(clientX, clientY);
        }

        private void Move(double clientX, double clientY)
        {
            if (_initialClientPoint == null)
                return;

            SetSelectionBoxInformation(clientX, clientY);

            var start = Diagram.GetRelativeMousePoint(_initialClientPoint.X, _initialClientPoint.Y);
            var end = Diagram.GetRelativeMousePoint(clientX, clientY);
            (var sX, var sY) = (Math.Min(start.X, end.X), Math.Min(start.Y, end.Y));
            (var eX, var eY) = (Math.Max(start.X, end.X), Math.Max(start.Y, end.Y));
            var bounds = new Rectangle(sX, sY, eX, eY);

            foreach (var node in Diagram.Nodes)
            {
                if (bounds.Overlap(node.GetBounds()))
                {
                    Diagram.SelectModel(node, false);
                }
                else if (node.Selected)
                {
                    Diagram.UnselectModel(node);
                }
            }

            StateHasChanged();
        }

        private void End()
        {
            _initialClientPoint = null;
            _selectionBoxTopLeft = null;
            _selectionBoxSize = null;
            StateHasChanged();
        }
        private string GenerateStyle()
            => FormattableString.Invariant($"position: absolute; background: {Background}; top: {_selectionBoxTopLeft.Y}px; left: {_selectionBoxTopLeft.X}px; width: {_selectionBoxSize.Width}px; height: {_selectionBoxSize.Height}px;");

        private void SetSelectionBoxInformation(double clientX, double clientY)
        {
            var start = Diagram.GetRelativePoint(_initialClientPoint.X, _initialClientPoint.Y);
            var end = Diagram.GetRelativePoint(clientX, clientY);
            (var sX, var sY) = (Math.Min(start.X, end.X), Math.Min(start.Y, end.Y));
            (var eX, var eY) = (Math.Max(start.X, end.X), Math.Max(start.Y, end.Y));
            _selectionBoxTopLeft = new Point(sX, sY);
            _selectionBoxSize = new Size(eX - sX, eY - sY);
        }

        public void Dispose()
        {
            Diagram.MouseDown -= OnMouseDown;
            Diagram.MouseMove -= OnMouseMove;
            Diagram.MouseUp -= OnMouseUp;
            Diagram.TouchStart -= OnTouchStart;
            Diagram.TouchMove -= OnTouchmove;
            Diagram.TouchEnd -= OnTouchEnd;
        }
    }
}
