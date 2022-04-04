using ECS.Core.Utils.ReactiveSystem;
using ECS.Core.Utils.ReactiveSystem.Components;
using ECS.Game.Components;
using ECS.Game.Components.Flags;
using ECS.Game.Components.Input;
using ECS.Views.Impls;
using Leopotam.Ecs;

namespace ECS.Game.Systems
{
    public class RemapOnAddSystem : ReactiveSystem<EventAddComponent<RemapPointComponent>>
    {
        protected override EcsFilter<EventAddComponent<RemapPointComponent>> ReactiveFilter { get; }
        private readonly EcsFilter<PlayerComponent, LinkComponent> _player;
        protected override void Execute(EcsEntity entity)
        {
            foreach (var i in _player)
            {
                var playerView = (CharacterView) _player.Get2(i).View;
                entity.Get<RemapPointComponent>().ModelPos = playerView.ModelRootTransform.localPosition;
            }
        }
    }
}