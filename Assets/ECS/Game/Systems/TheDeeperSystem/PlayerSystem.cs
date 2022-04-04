using ECS.Core.Utils.ReactiveSystem;
using ECS.Core.Utils.ReactiveSystem.Components;
using ECS.Game.Components;
using ECS.Game.Components.Flags;
using ECS.Views.Impls;
using Game.Utils.MonoBehUtils;
using Leopotam.Ecs;
using UnityEngine;
using Zenject;

namespace ECS.Game.Systems
{
    public class PlayerSystem : ReactiveSystem<EventAddComponent<SpherePlayerComponent>>
    {
        protected override EcsFilter<EventAddComponent<SpherePlayerComponent>> ReactiveFilter { get; }
        //filtr for tap on pipe
        [Inject] private readonly GetPointFromScene _getPointFromScene;
        protected override void Execute(EcsEntity entity)
        {
            var point = _getPointFromScene.GetPoint("Player");
            var sphereView = (SpherePlayerView) entity.Get<LinkComponent>().View;
            sphereView.Transform.position = point.position; 
            sphereView.transform.rotation = Quaternion.Euler(0,180,0);
        }
    }
}