using System;
using UnityEngine;

namespace ECS.Game.Components.TheDeeperComponent
{
    public struct PortalComponent
    {
        public PortalColor color;
        public enum PortalColor
        {
            Red,
            Green,
            Blue,
            Purple,
            Default
        }
    }
}