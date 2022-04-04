using ECS.Core.Utils.ReactiveSystem;
using ECS.Core.Utils.ReactiveSystem.Components;
using ECS.Game.Components;
using ECS.Game.Components.Flags;
using ECS.Game.Components.TheDeeperComponent;
using ECS.Utils;
using ECS.Views;
using ECS.Views.Impls;
using Game.Utils.MonoBehUtils;
using Leopotam.Ecs;
using UnityEngine;
using Zenject;

namespace ECS.Game.Systems
{
    public class CameraLocateSystem : ReactiveSystem<EventAddComponent<CameraComponent>>
    {
        [Inject] private readonly GetPointFromScene _getPointFromScene;
        protected override EcsFilter<EventAddComponent<CameraComponent>> ReactiveFilter { get; }
        protected override void Execute(EcsEntity entity)
        {
            var point = _getPointFromScene.GetPoint("Camera");
            entity.Get<LinkComponent>().View.Transform.position = point.position;
            entity.Get<LinkComponent>().View.Transform.rotation = point.rotation;
        }
    }
}