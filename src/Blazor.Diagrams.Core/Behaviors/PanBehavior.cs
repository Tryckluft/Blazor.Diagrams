using Blazor.Diagrams.Core.Geometry;
using Blazor.Diagrams.Core.Models.Base;
using Microsoft.AspNetCore.Components.Web;
using System;

namespace Blazor.Diagrams.Core.Behaviors
{
    public class PanBehavior : Behavior
    {
        private Point? _initialPan;
        private double _lastClientX;
        private double _lastClientY;

        public PanBehavior(Diagram diagram) : base(diagram)
        {
            Diagram.MouseDown += OnMouseDown;
            Diagram.MouseMove += OnMouseMove;
            Diagram.MouseUp += OnMouseUp;
            Diagram.TouchStart += OnTouchStart;
            Diagram.TouchMove += OnTouchmove;
            Diagram.TouchEnd += OnTouchEnd;
        }

        private void OnTouchStart(Model model, TouchEventArgs e)
        {
            if (!Diagram.Options.AllowPanning)
                return;

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
            if (!Diagram.Options.AllowPanning)
                return;

            if (model != null)
                return;

            var options = Diagram.Options.Panning;

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

        private void OnMouseMove(Model model, MouseEventArgs e) => Move(e.ClientX, e.ClientY);

        private void OnMouseUp(Model model, MouseEventArgs e) => End();

        private void Start(double clientX, double clientY)
        {
            _initialPan = Diagram.Pan;
            _lastClientX = clientX;
            _lastClientY = clientY;
        }

        private void Move(double clientX, double clientY)
        {
            if (!Diagram.Options.AllowPanning || _initialPan == null)
                return;

            var deltaX = clientX - _lastClientX - (Diagram.Pan.X - _initialPan.X);
            var deltaY = clientY - _lastClientY - (Diagram.Pan.Y - _initialPan.Y);
            Diagram.UpdatePan(deltaX, deltaY);
        }

        private void End()
        {
            if (!Diagram.Options.AllowPanning)
                return;

            _initialPan = null;
        }

        public override void Dispose()
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
