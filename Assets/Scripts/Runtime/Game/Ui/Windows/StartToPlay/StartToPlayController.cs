using DataBase.Game;
using DG.Tweening;
using ECS.Game.Components.Flags;
using ECS.Game.Systems.GameCycle;
using ECS.Utils.Extensions;
using Leopotam.Ecs;
using Runtime.Services.AnalyticsService;
using SimpleUi.Abstracts;
using SimpleUi.Signals;
using UniRx;
using UnityEngine;
using Utils.UiExtensions;
using Zenject;

namespace Runtime.Game.Ui.Windows.StartToPlay 
{
    public class StartToPlayController : UiController<StartToPlayView>, IInitializable
    {
        [Inject] private IAnalyticsService _analyticsService;
        private readonly SignalBus _signalBus;
        private readonly EcsWorld _world;

        public StartToPlayController(SignalBus signalBus, EcsWorld world)
        {
            _signalBus = signalBus;
            _world = world;
        }
        
        public void Initialize()
        {
            View.StartToPlay.OnClickAsObservable().Subscribe(x => OnStart()).AddTo(View.StartToPlay);
        }

        private void OnStart()
        {
            _world.SetStage(EGameStage.Play);
            _signalBus.OpenWindow<GameHudWindow>();
            _analyticsService.SendRequest("level_start");
        }
    }
}