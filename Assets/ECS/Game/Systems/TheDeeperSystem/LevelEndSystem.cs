using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using DataBase.Game;
using DG.Tweening;
using ECS.Core.Utils.ReactiveSystem;
using ECS.Game.Components;
using ECS.Game.Components.Events;
using ECS.Game.Components.TheDeeperComponent;
using ECS.Views.Impls;
using Game.SceneLoading;
using Leopotam.Ecs;
using Runtime.Game.Ui.Windows.LevelComplete;
using Runtime.Services.AnalyticsService;
using Runtime.Services.CommonPlayerData;
using Runtime.Services.CommonPlayerData.Data;
using Runtime.Services.CommonPlayerData.Impl;
using SimpleUi.Abstracts;
using UnityEngine;
using Zenject;
#pragma warning disable 649
namespace ECS.Game.Systems.GameCycle
{
    [SuppressMessage("ReSharper", "UnassignedGetOnlyAutoProperty")]
    [SuppressMessage("ReSharper", "PossibleNullReferenceException")]
    public class LevelEndSystem : ReactiveSystem<ChangeStageComponent>
    {
        // [Inject] private readonly ScreenVariables _screenVariables;
        [Inject] private ICommonPlayerDataService<CommonPlayerData> _commonPlayerData;
        [Inject] private readonly SignalBus _signalBus;
        [Inject] private IAnalyticsService _analyticsService;

        [Inject] private ISceneLoadingManager _sceneLoadingManager;
        // [Inject] private readonly IVibrationService _vibrationService;
        private readonly EcsWorld _world;
        private readonly EcsFilter<CameraComponent, LinkComponent> _cameraF;
        // private readonly EcsFilter<BoxComponent, LinkComponent> _box;
        // private readonly EcsFilter<PutableComponent, LinkComponent> _putables;
        protected override EcsFilter<ChangeStageComponent> ReactiveFilter { get; }
        protected override bool DeleteEvent => false;
        private bool disable;

        protected override void Execute(EcsEntity entity)
        {
            if (disable)
                return;
            switch (entity.Get<ChangeStageComponent>().Value)
            {
                case EGameStage.Lose:
                    HandleLevelLose();
                    disable = true;
                    break;
                case EGameStage.Complete:
                    HandleLevelComplete();
                    disable = true;
                    break;
            }
        }

        [SuppressMessage("ReSharper", "Unity.InefficientPropertyAccess")]
        private void HandleLevelComplete()
        {
            _analyticsService.SendRequest("level_complete");
            
            var data = _commonPlayerData.GetData();

            if (data.Level >= Enum.GetValues(typeof(EScene)).Cast<EScene>().Last())
            {
                data.Level = EScene.Game0_1;
            }
            else
                data.Level++;

            _commonPlayerData.Save(data);

            foreach (var camera in _cameraF)
            {
                var cameraView = (CameraView) _cameraF.Get2(camera).View;
                
                cameraView.Transform.DOMoveY(0, 1.5f).SetEase(Ease.Linear).SetRelative(true).OnComplete(() =>
                {
                    Debug.Log((int)_commonPlayerData.GetData().Level);
                    _sceneLoadingManager.LoadScene(_commonPlayerData.GetData().Level);
                });
            }
        }

        private void HandleLevelLose()
        {
            _analyticsService.SendRequest("level_lose");
            foreach (var camera in _cameraF)
            {
                var cameraView = (CameraView) _cameraF.Get2(camera).View;
                //TODO goodcase
                cameraView.Transform.DOMoveY(0, 1.5f).SetEase(Ease.Linear).SetRelative(true).OnComplete(() =>
                {
                    _sceneLoadingManager.ReloadScene();
                });
            }
            
        }
    }
}
       