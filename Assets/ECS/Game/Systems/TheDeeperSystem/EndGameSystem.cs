using System;
using DataBase.Game;
using ECS.Core.Utils.ReactiveSystem;
using ECS.Game.Components;
using ECS.Game.Components.Events;
using ECS.Game.Components.Flags;
using ECS.Utils.Extensions;
using ECS.Views.Impls;
using Game.Ui.BlackScreen;
using Leopotam.Ecs;
using Signals;
using SimpleUi.Signals;
using UnityEngine;
using Zenject;

namespace ECS.Game.Systems
{
    public class EndGameSystem : ReactiveSystem<ChangeStageComponent>
    {
        [Inject] private readonly SignalBus _signalBus;
        private readonly EcsFilter<PlayerComponent, GameResultComponent, LinkComponent, ImpactComponent> _player;
        private readonly EcsFilter<FinishViewComponent, LinkComponent> _finish;
        protected override EcsFilter<ChangeStageComponent> ReactiveFilter { get; }
        protected override bool DeleteEvent => false;
        protected override void Execute(EcsEntity entity)
        {
            if(entity.Get<ChangeStageComponent>().Value != EGameStage.GameEnd) return;
            var playerEntity = _player.GetEntity(0);
            playerEntity.Del<TargetPositionComponent>();
            playerEntity.Del<TargetRotationComponent>();
            var result = _player.Get2(0).Value;
            var playerLink = (CharacterView) _player.Get3(0).View;
            playerLink.SetStage(result);
            switch (result)
            {
                case EGameResult.Fail:
                    _signalBus.Fire(new SignalGameEnd(EGameResult.Fail));
                    break;
                case EGameResult.Win:
                    _signalBus.OpenWindow<BlackScreenWindow>(EWindowLayer.Project);
                    _signalBus.Fire(new SignalBlackScreen(true, () => {
                        var playerImpact = _player.Get4(0).Value;
                        var finishView = (FinishVisualView) _finish.Get2(0).View;
                        // finishView.ShowFinish(playerImpact.ComparePlayerStage()-1);
                        playerLink.ModelRootTransform.localPosition = Vector3.zero;
                        _signalBus.Fire(new SignalBlackScreen(false, () => {
                            playerLink.FinalCamera(() => _signalBus.Fire(new SignalGameEnd(EGameResult.Win)));
                        }, 0.5f, Color.white));
                    }, 0.5f, Color.white));
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}