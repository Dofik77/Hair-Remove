using DataBase.Game;
using ECS.Game.Components;
using ECS.Game.Components.Flags;
using ECS.Game.Components.Hair_Remove_Component;
using ECS.Game.Systems.GameCycle;
using ECS.Utils.Extensions;
using Game.SceneLoading;
using Leopotam.Ecs;
using Runtime.Game.Ui.Windows.InGameMenu;
using Runtime.Services.CommonPlayerData;
using Runtime.Services.CommonPlayerData.Data;
using Signals;
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

            _signalBus.GetStream<SignalUpdateImpact>().Subscribe(x => OnImpactUpdate(x.Value)).AddTo(View);
            _signalBus.GetStream<SignalJoystickUpdate>().Subscribe(x => View.UpdateJoystick(ref x)).AddTo(View);

            // _signalBus.GetStream<SignalHpBarUpdate>().Subscribe(x => View.UpdateHpBar(ref x)).AddTo(View);
            // _signalBus.GetStream<SignalLifeCountUpdate>().Subscribe(x => View.UpdateLifeCount(ref x)).AddTo(View);
            // _signalBus.GetStream<SignalScoreUpdate>().Subscribe(x => View.UpdateScore(ref x)).AddTo(View);
            // _signalBus.GetStream<SignalLevelEnd>().Subscribe(x => View.SetFinishBtn(ref x.Value)).AddTo(View);
            // _signalBus.GetStream<SignalUpdateCurrency>().Subscribe(x => View.UpdateCurrency(ref x.Value)).AddTo(View);
        }
        
        private void OnImpactUpdate(int value) => View.ProgressBar.Repaint(value.Remap01(1000));
        
        public override void OnShow()
        {
            View.Show(_commonPlayerData.GetData());
            
            View.ProgressBar.gameObject.SetActive(true);
            View.ProgressBar.SetFillAmount(_world.GetEntity<PlayerComponent>().Get<ImpactComponent>().Value.Remap01(1000));
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