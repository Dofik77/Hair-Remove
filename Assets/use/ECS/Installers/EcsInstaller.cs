using ECS.Game.Systems;
using ECS.Game.Systems.GameCycle;
using ECS.Game.Systems.GameDay;
using ECS.Game.Systems.Hair_Remove_System;
using ECS.Game.Systems.Linked;
using ECS.Game.Systems.Move;
using ECS.Game.Systems.TheDeeperSystem;
using Game.Utils.MonoBehUtils;
using Leopotam.Ecs;
using Runtime.DataBase.Objects.Impl;
using UnityEngine;
using use.ECS.Game.Systems.Hair_Remove_System;
using Zenject;

namespace ECS.Installers
{
    public class EcsInstaller : MonoInstaller
    {
        [SerializeField] private GetPointFromScene _getPointFromScene;
        [SerializeField] private MaterialBase _materiarBase;
        [SerializeField] private ScenePath _pathRoot;
        public override void InstallBindings()
        {
            Container.Bind<GetPointFromScene>().FromInstance(_getPointFromScene).AsSingle();
            Container.Bind<ScenePath>().FromInstance(_pathRoot).AsSingle();
            Container.BindInterfacesAndSelfTo<EcsWorld>().AsSingle().NonLazy();
            BindSystems();
            Container.BindInterfacesTo<EcsMainBootstrap>().AsSingle();

            Container.Bind<MaterialBase>().FromInstance(_materiarBase).AsSingle();
        }

        private void BindSystems()
        {
            //tech system
            Container.BindInterfacesAndSelfTo<IsAvailableSetViewSystem>().AsSingle();
            Container.BindInterfacesAndSelfTo<GameInitializeSystem>().AsSingle();
            Container.BindInterfacesAndSelfTo<InstantiateSystem>().AsSingle();
            Container.BindInterfacesAndSelfTo<GameTimerSystem>().AsSingle();
            Container.BindInterfacesAndSelfTo<PositionSystem>().AsSingle();
            Container.BindInterfacesAndSelfTo<RotateSystem>().AsSingle();
            
            //game process system
            Container.BindInterfacesAndSelfTo<CameraResizeSystem>().AsSingle();
            Container.BindInterfacesAndSelfTo<PlayerSystem>().AsSingle();
            Container.BindInterfacesAndSelfTo<AddImpactToPlayerSystem>().AsSingle();
            Container.BindInterfacesAndSelfTo<HairsDeactivateSystem>().AsSingle();
            Container.BindInterfacesAndSelfTo<PlayerMovementSystem>().AsSingle();
            Container.BindInterfacesAndSelfTo<OutOfCameraBorderSystem>().AsSingle();
            Container.BindInterfacesAndSelfTo<PortalEffectActivationSystem>().AsSingle();
            Container.BindInterfacesAndSelfTo<ParticleControllSystem>().AsSingle();
            Container.BindInterfacesAndSelfTo<PositionRotationTranslateSystem>().AsSingle();
            Container.BindInterfacesAndSelfTo<TriggersDistanceSystem>().AsSingle();
            Container.BindInterfacesAndSelfTo<ChangeModelPlayerSystem>().AsSingle();
            Container.BindInterfacesAndSelfTo<CameraLocateSystem>().AsSingle();

            //tech system
            Container.BindInterfacesAndSelfTo<GamePauseSystem>().AsSingle();
            Container.BindInterfacesAndSelfTo<LevelEndSystem>().AsSingle();
            Container.BindInterfacesAndSelfTo<SaveGameSystem>().AsSingle();
            Container.BindInterfacesAndSelfTo<StartGameSystem>().AsSingle();
            //Container.BindInterfacesAndSelfTo<EndGameSystem>().AsSingle();
            Container.BindInterfacesAndSelfTo<GameStageSystem>().AsSingle();        //always must been last
            Container.BindInterfacesAndSelfTo<CleanUpSystem>().AsSingle();          //must been latest than last!
        }       
    }
}