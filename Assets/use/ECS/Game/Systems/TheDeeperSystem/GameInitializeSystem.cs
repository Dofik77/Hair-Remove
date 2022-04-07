using DataBase.Game;
using ECS.Core.Utils.ReactiveSystem.Components;
using ECS.DataSave;
using ECS.Game.Components;
using ECS.Game.Components.Events;
using ECS.Game.Components.Flags;
using ECS.Game.Components.TheDeeperComponent;
using ECS.Game.Systems.GameCycle;
using ECS.Game.Systems.Hair_Remove_System;
using ECS.Utils.Extensions;
using ECS.Utils.Impls;
using ECS.Views.GameCycle;
using ECS.Views.Impls;
using Game.Utils.MonoBehUtils;
using Leopotam.Ecs;
using Runtime.DataBase.General.CommonParamsBase;
using Runtime.DataBase.General.GameCFG;
using Runtime.Services.AnalyticsService;
using Runtime.Services.CommonPlayerData;
using Runtime.Services.CommonPlayerData.Data;
using Runtime.Services.GameStateService;
using Services.Uid;
using Signals;
using UnityEngine;
using use.ECS.Game.Systems.Hair_Remove_System;
using Zenject;
using Object = System.Object;

namespace ECS.Game.Systems
{
    public class GameInitializeSystem : IEcsInitSystem
    {
        [Inject] private readonly IGameStateService<GameState> _generalState;
        [Inject] private readonly GetPointFromScene _getPointFromScene;
        [Inject] private readonly ICommonPlayerDataService<CommonPlayerData> _playerData;
        [Inject] private IAnalyticsService _analyticsService;
        
        private readonly EcsWorld _world;
        public void Init()
        {
            if (LoadGame()) return;
            _analyticsService.SendRequest("level_started");
            CreatePlayer();
            FindCamera();
            FindGrasses();
            CreateTimer();
            CreateDistanceTriggers();
        }
        
        private void CreatePlayer()
        {
            var entity = _world.NewEntity();
            entity.Get<PlayerComponent>();
            entity.Get<PrefabComponent>().Value = "Player";
            entity.Get<EventAddComponent<PrefabComponent>>();
            entity.Get<EventAddComponent<PlayerComponent>>();
            entity.Get<ImpactComponent>().Value = 100;
        }


        private void FindGrasses()
        {
            var grasessOnScene = UnityEngine.Object.FindObjectsOfType<GrassView>(true); 
            foreach (var view in grasessOnScene)
            {
                var entity = _world.NewEntity();
                entity.Get<InteractableComponent>();
                entity.Get<HairComponent>();
                view.Link(entity);
                entity.Get<LinkComponent>().View = view;
                entity.Get<UIdComponent>().Value = UidGenerator.Next();
            }
        }

        private void FindCamera()
        {
            var view = UnityEngine.Object.FindObjectOfType<CameraView>(true);
            var entity = _world.NewEntity();
            entity.Get<UIdComponent>().Value = UidGenerator.Next();
            entity.Get<CameraComponent>();
            entity.Get<LinkComponent>().View = view;
            view.Link(entity);
        }

        public void CreateDistanceTriggers()
        {
            var views = UnityEngine.Object.FindObjectsOfType<DistanceTriggerView>(true);
            foreach (var view in views)
            {
                var entity = _world.NewEntity();
                entity.Get<UIdComponent>().Value = UidGenerator.Next();
                entity.Get<DistanceTriggerComponent>();
                entity.Get<LinkComponent>().View = view;
                view.Link(entity);
            }
        }


        private bool LoadGame()
        {
            _world.NewEntity().Get<GameStageComponent>().Value = EGameStage.Play;
            var gState = _generalState.GetData();
            if (gState.SaveState.IsNullOrEmpty()) return false;
            foreach (var state in gState.SaveState)
            {
                var entity =_world.NewEntity();
                state.ReadState(entity);
            }
            return true;
        }
        
        private void CreateTimer()
        {
            var entity = _world.NewEntity();
            entity.Get<TimerComponent>();
            entity.Get<UIdComponent>().Value = UidGenerator.Next();
        }
    }
}