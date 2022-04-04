using DataBase.Game;
using ECS.Game.Systems.GameCycle;
using ECS.Utils.Extensions;
using Game.SceneLoading;
using Leopotam.Ecs;
using Runtime.Game.Ui.Windows.InGameMenu;
using Runtime.Services.CommonPlayerData;
using Runtime.Services.CommonPlayerData.Data;
using SimpleUi.Abstracts;
using SimpleUi.Signals;
using UniRx;
using UnityEngine;
using Utils.UiExtensions;
using Zenject;

namespace Runtime.Game.Ui.Windows.InGameButtons
{
    public class InGameButtonsController : UiController<InGameButtonsView>, IInitializable
    {
        [Inject] private readonly ICommonPlayerDataService<CommonPlayerData> _commonPlayerData;
        [Inject] private readonly ISceneLoadingManager _sceneLoadingManager;
        private readonly SignalBus _signalBus;
        private EcsWorld _world; 

        private int _lastPrice = 0;
        private bool isFree = true;

        public InGameButtonsController(SignalBus signalBus, EcsWorld world)
        {
            _signalBus = signalBus;
            _world = world;
        }

        public void Initialize()
        {
            View.InGameMenuButton.OnClickAsObservable().Subscribe(x => OnGameMenu()).AddTo(View.InGameMenuButton);
            View.RestartGameButton.OnClickAsObservable().Subscribe(x => OnRestart()).AddTo(View.RestartGameButton);
            
            // _signalBus.GetStream<SignalJoystickUpdate>().Subscribe(x => View.UpdateJoystick(ref x)).AddTo(View);
            // _signalBus.GetStream<SignalHpBarUpdate>().Subscribe(x => View.UpdateHpBar(ref x)).AddTo(View);
            // _signalBus.GetStream<SignalLifeCountUpdate>().Subscribe(x => View.UpdateLifeCount(ref x)).AddTo(View);
            // _signalBus.GetStream<SignalScoreUpdate>().Subscribe(x => View.UpdateScore(ref x)).AddTo(View);
            // _signalBus.GetStream<SignalLevelEnd>().Subscribe(x => View.SetFinishBtn(ref x.Value)).AddTo(View);
            // _signalBus.GetStream<SignalUpdateCurrency>().Subscribe(x => View.UpdateCurrency(ref x.Value)).AddTo(View);
        }
        
        public override void OnShow()
        {
            View.Show(_commonPlayerData.GetData());
        }

        private void OnGameMenu()
        {
            _signalBus.OpenWindow<InGameMenuWindow>();
            _world.SetStage(EGameStage.Pause);
        }

        private void OnFinish()
        {
            _world.SetStage(EGameStage.Complete);
        }
        
        private void OnRestart()
        {
            _sceneLoadingManager.ReloadScene();
        }
    }
}