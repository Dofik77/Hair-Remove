using ECS.Core.Utils.ReactiveSystem;
using ECS.Core.Utils.ReactiveSystem.Components;
using ECS.Game.Components;
using ECS.Game.Components.TheDeeperComponent;
using ECS.Views.Impls;
using Leopotam.Ecs;
using Runtime.DataBase.Objects.Impl;
using UnityEngine;
using Zenject;

namespace ECS.Game.Systems
{
    public class WallColoringSystem :  ReactiveSystem<EventAddComponent<WallComponent>>
    {
        
        [Inject] private readonly MaterialBase _materialBase;
        protected override EcsFilter<EventAddComponent<WallComponent>> ReactiveFilter { get; }
        protected override void Execute(EcsEntity entity)
        {
            var viewOfWall = (WallView) entity.Get<LinkComponent>().View;
            Material _colorOfWall = _materialBase.Get(viewOfWall.color.ToString());
            viewOfWall.gameObject.GetComponent<Renderer>().material = _colorOfWall;
        }
    }
}