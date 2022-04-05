using DataBase.Game;
using ECS.Core.Utils.ReactiveSystem.Components;
using ECS.DataSave;
using ECS.Game.Components;
using ECS.Game.Components.Events;
using ECS.Game.Components.Flags;
using ECS.Game.Components.TheDeeperComponent;
using ECS.Game.Systems.GameCycle;
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
            CreateCamera();
            CreateTimer();
            CreateDistanceTriggers();
        }

        private void CreatePotal()
        {
            var portalOnScene = UnityEngine.Object.FindObjectsOfType<PortalView>(); 
            
            foreach (var view in portalOnScene)
            {
                var entity = _world.NewEntity();
                entity.Get<PortalComponent>().color = view.color;//из вьюшки
                entity.Get<EventAddComponent<PortalComponent>>();
                entity.Get<UIdComponent>().Value = UidGenerator.Next();
                entity.Get<LinkComponent>().View = view;
                view.Link(entity);
                view.Holders.SetActive(true);
                //find all set by gamedis portal
            }
        }
        
        private void CreateWall()
        {
            var wallOnScene = UnityEngine.Object.FindObjectsOfType<WallView>(); 
            foreach (var view in wallOnScene)
            {
                var entity = _world.NewEntity();
                entity.Get<UIdComponent>().Value = UidGenerator.Next();
                entity.Get<LinkComponent>().View = view;
                entity.Get<EventAddComponent<WallComponent>>();
                view.Link(entity);
                //find all set by gamedis wall
            }
        }

        private void CreatePipes()
        {
            var pipeOnScene = UnityEngine.Object.FindObjectsOfType<PipeView>(); 
            foreach (var view in pipeOnScene)
            {
                var entity = _world.NewEntity();
                entity.Get<PipeComponent>();
                entity.Get<EventAddComponent<PipeComponent>>();
                entity.Get<LinkComponent>().View = view;
            }
        }

        private void CreateCamera()
        {
            var entity = _world.NewEntity();
            entity.Get<CameraComponent>();
            entity.Get<PrefabComponent>().Value = "MainCamera";
            entity.Get<EventAddComponent<PrefabComponent>>();
            entity.Get<EventAddComponent<CameraComponent>>();
            entity.Get<ActiveCameraComponent>();

            //if have PrefabComponent, we have to Get EventAddComponent<PrefabComp>
            //maybe relacate in Extension and just find camera on scee and get some
            //component
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

        private void CreatePlayer()
        {
            var entity = _world.NewEntity();
            entity.Get<PlayerComponent>();
            entity.Get<PrefabComponent>().Value = "Player";
            entity.Get<EventAddComponent<PrefabComponent>>();
            entity.Get<EventAddComponent<PlayerComponent>>();
        }
    }
}