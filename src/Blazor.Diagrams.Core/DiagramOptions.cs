﻿using Blazor.Diagrams.Core.Models;
using Blazor.Diagrams.Core.Models.Base;
using Microsoft.AspNetCore.Components.Web;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Blazor.Diagrams.Core
{
    public class DiagramOptions
    {
        [Description("Key code for deleting entities")]
        public string DeleteKey { get; set; } = "Delete";
        [Description("The default component for nodes")]
        public Type? DefaultNodeComponent { get; set; }
        [Description("The grid size (grid-based snaping")]
        public int? GridSize { get; set; }
        [Description("Whether to allow users to select multiple nodes at once using CTRL or not")]
        public bool AllowMultiSelection { get; set; } = true;
        [Description("Whether to allow panning or not")]
        public bool AllowPanning { get; set; } = true;
        [Description("Only render visible nodes")]
        public bool EnableVirtualization { get; set; } = true;
        [Description("Mouse buttons used for selecting nodes.")]
        public List<MouseButtonEnum> SelectionMouseButtons { get; set; } = new List<MouseButtonEnum> { MouseButtonEnum.Left, MouseButtonEnum.Right };

        public DiagramZoomOptions Zoom { get; set; } = new DiagramZoomOptions();
        public DiagramLinkOptions Links { get; set; } = new DiagramLinkOptions();
        public DiagramGroupOptions Groups { get; set; } = new DiagramGroupOptions();
        public DiagramConstraintsOptions Constraints { get; set; } = new DiagramConstraintsOptions();
        public DiagramSelectionBoxOptions SelectionBox { get; set; } = new DiagramSelectionBoxOptions();
        public DiagramPanningOptions Panning { get; set; } = new DiagramPanningOptions();
    }

    /// <summary>
    /// All the options regarding links.
    /// </summary>
    public class DiagramLinkOptions
    {
        [Description("The default component for links")]
        public Type? DefaultLinkComponent { get; set; }
        [Description("The default color for links")]
        public string DefaultColor { get; set; } = "black";
        [Description("The default color for selected links")]
        public string DefaultSelectedColor { get; set; } = "rgb(110, 159, 212)";
        [Description("Default Router for links")]
        public Router DefaultRouter { get; set; } = Routers.Normal;
        [Description("Default PathGenerator for links")]
        public PathGenerator DefaultPathGenerator { get; set; } = PathGenerators.Smooth;
        [Description("Whether to enable link snapping")]
        public bool EnableSnapping { get; set; }
        [Description("Link snapping radius")]
        public double SnappingRadius { get; set; } = 50;
        [Description("Link model factory")]
        public LinkFactory Factory { get; set; } = (diagram, sourcePort) => new LinkModel(sourcePort);
    }

    /// <summary>
    /// All the options regarding zooming.
    /// </summary>
    public class DiagramZoomOptions
    {
        [Description("Whether to allow zooming or not")]
        public bool Enabled { get; set; } = true;
        [Description("Whether to inverse the zoom direction or not")]
        public bool Inverse { get; set; } = true;
        [Description("Minimum value allowed")]
        public double Minimum { get; set; } = 0.1;
        [Description("Maximum value allowed")]
        public double Maximum { get; set; } = 2;
        [Description("Zoom Scale Factor. Should be between 1.01 and 2.  Default is 1.05.")]
        public double ScaleFactor { get; set; } = 1.05;
    }

    /// <summary>
    /// All the options regarding groups.
    /// </summary>
    public class DiagramGroupOptions
    {
        [Description("Whether to allow users to group/ungroup nodes")]
        public bool Enabled { get; set; }
        [Description("Keyboard shortcut (CTRL+ALT+G by default)")]
        public Func<KeyboardEventArgs, bool> KeyboardShortcut { get; set; } = e => e.CtrlKey && e.AltKey && e.Key == "g";
        [Description("Group model factory")]
        public GroupFactory Factory { get; set; } = (diagram, children) => new GroupModel(children);
    }

    /// <summary>
    /// All the options regarding diagram constraints, such as deciding whether to delete a node or not.
    /// </summary>
    public class DiagramConstraintsOptions
    {
        [Description("Decide if a node can/should be deleted")]
        public Func<NodeModel, bool> ShouldDeleteNode { get; set; } = _ => true;
        [Description("Decide if a link can/should be deleted")]
        public Func<BaseLinkModel, bool> ShouldDeleteLink { get; set; } = _ => true;
        [Description("Decide if a group can/should be deleted")]
        public Func<GroupModel, bool> ShouldDeleteGroup { get; set; } = _ => true;
    }

    /// <summary>
    /// Options regarding the SelectionBoxWidget
    /// </summary>
    public class DiagramSelectionBoxOptions
    {
        [Description("Mouse button used for creating a selection box.")]
        public MouseButtonEnum MouseButton { get; set; } = MouseButtonEnum.Left;
        [Description("Modifier key for creating a selection box.")]
        public ModifierKeyEnum MouseModifierKey { get; set; } = ModifierKeyEnum.None;
        [Description("Touch modifier key for creating a selection box.")]
        public ModifierKeyEnum TouchModifierKey { get; set; } = ModifierKeyEnum.Shift;
    }

    /// <summary>
    /// Options regarding the PanBehavior
    /// </summary>
    public class DiagramPanningOptions
    {
        [Description("Mouse button used for panning.")]
        public MouseButtonEnum MouseButton { get; set; } = MouseButtonEnum.Middle;
        [Description("Modifier key for panning")]
        public ModifierKeyEnum MouseModifierKey { get; set; } = ModifierKeyEnum.None;
        [Description("Touch modifier key for panning")]
        public ModifierKeyEnum TouchModifierKey { get; set; } = ModifierKeyEnum.None;

    }

    #region Enums
    public enum MouseButtonEnum
    {
        Left = 0,
        Middle = 1,
        Right = 2,
    }
    public enum ModifierKeyEnum
    {
        None,
        Ctrl,
        Shift,
        Alt,
    }
    #endregion
}
