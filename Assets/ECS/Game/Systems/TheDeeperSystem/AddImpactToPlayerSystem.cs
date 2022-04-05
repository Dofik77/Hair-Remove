using DataBase.Game;
using ECS.Core.Utils.ReactiveSystem;
using ECS.Game.Components;
using ECS.Game.Components.Events;
using ECS.Game.Components.Flags;
using ECS.Utils.Extensions;
using ECS.Views.Impls;
using Leopotam.Ecs;
using Signals;
using Zenject;

namespace ECS.Game.Systems
{
    public class AddImpactToPlayerSystem : ReactiveSystem<AddImpactEventComponent>
    {
        [Inject] private readonly SignalBus _signalBus;
        private readonly EcsFilter<PlayerComponent, ImpactComponent, LinkComponent> _player;
        private readonly EcsFilter<GameStageComponent> _gameStage;
        protected override bool DeleteEvent => false;
        protected override EcsFilter<AddImpactEventComponent> ReactiveFilter { get; }
        protected override void Execute(EcsEntity entity)
        {
            var impact = entity.Get<ImpactComponent>().Value;
            ref var playerImpact = ref _player.Get2(0).Value;
            if (playerImpact + impact > 1000)
                playerImpact = 1000;
            else if (playerImpact + impact < 0)
            {
                _player.GetEntity(0).Get<GameResultComponent>().Value = EGameResult.Fail;
                _gameStage.GetEntity(0).Get<ChangeStageComponent>().Value = EGameStage.GameEnd;
                return;
            }
            else playerImpact += impact;
            _signalBus.Fire(new SignalUpdateImpact(playerImpact));
            var playerLink = (CharacterView) _player.Get3(0).View;
            // playerLink.SetStage(playerImpact.ComparePlayerStage());
            entity.Destroy();
            
            
        }
    }
}