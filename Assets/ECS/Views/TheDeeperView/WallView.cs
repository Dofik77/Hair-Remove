using System;
using ECS.Game.Components.TheDeeperComponent;
using Ecs.Views.Linkable.Impl;
using PdUtils;
using UnityEngine;

namespace ECS.Views.Impls
{
    public class WallView : LinkableView
    {
        [SerializeField] public WallComponent.WallColor color;
    }
}