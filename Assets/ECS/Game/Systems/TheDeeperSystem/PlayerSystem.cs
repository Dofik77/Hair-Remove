using ECS.Core.Utils.ReactiveSystem;
using ECS.Core.Utils.ReactiveSystem.Components;
using ECS.Game.Components;
using ECS.Game.Components.Flags;
using ECS.Views.GameCycle;
using ECS.Views.Impls;
using Game.Utils.MonoBehUtils;
using Leopotam.Ecs;
using UnityEngine;
using Zenject;

namespace ECS.Game.Systems
{
    public class PlayerSystem : ReactiveSystem<EventAddComponent<PlayerComponent>>
    {
        protected override EcsFilter<EventAddComponent<PlayerComponent>> ReactiveFilter { get; }
        [Inject] private readonly GetPointFromScene _getPointFromScene;
        protected override void Execute(EcsEntity entity)
        {
            var point = _getPointFromScene.GetPoint("Player");
            var playerView = (PlayerView) entity.Get<LinkComponent>().View;
            playerView.Transform.position = point.position;
        }
    }
}