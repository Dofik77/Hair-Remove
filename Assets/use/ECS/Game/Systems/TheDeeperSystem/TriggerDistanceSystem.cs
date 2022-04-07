using System.Diagnostics.CodeAnalysis;
using DataBase.Game;
using ECS.Core.Utils.SystemInterfaces;
using ECS.Game.Components;
using ECS.Game.Components.Flags;
using ECS.Game.Components.TheDeeperComponent;
using ECS.Views.GameCycle;
using ECS.Views.Impls;
using Leopotam.Ecs;
using UnityEngine;
namespace ECS.Game.Systems.GameCycle
{
    public class TriggersDistanceSystem : IEcsUpdateSystem
    {
#pragma warning disable 649
        private readonly EcsFilter<GameStageComponent> _gameStage;
        private readonly EcsFilter<DistanceTriggerComponent, LinkComponent> _triggers;
        private EcsFilter<ActivePortalComponent, LinkComponent> _activePortal;
        private EcsFilter<SpherePlayerComponent, LinkComponent> _sphere;
#pragma warning restore 649
        private EcsEntity _triggerEntity;
        private PortalView _portalView;
        private SpherePlayerView _spherePlayerView;
        private DistanceTriggerView _distanceTriggerView;
        [SuppressMessage("ReSharper", "PossibleNullReferenceException")]
        public void Run()
        {
            if (_gameStage.Get1(0).Value != EGameStage.Play) return;
            foreach (var i in _activePortal)
            {
                _portalView = (PortalView) _activePortal.Get2(i).View;

                foreach (var j in _triggers)
                {
                    _triggerEntity = _triggers.GetEntity(j);
                    _distanceTriggerView = (DistanceTriggerView) _triggers.Get2(j).View;

                    if (!_distanceTriggerView.gameObject.activeSelf ||
                        !_distanceTriggerView.gameObject.activeInHierarchy)
                        continue;

                    if (Vector3.Distance(_portalView.Transform.position, _distanceTriggerView.Transform.position) >
                        _distanceTriggerView.GetTriggerDistance())
                        continue;

                    foreach (var unlockable in _distanceTriggerView.GetUnlockable())
                        unlockable?.SetActive(true);
                    foreach (var lockable in _distanceTriggerView.GetLockable())
                        lockable?.SetActive(false);
                    _triggerEntity.Get<IsDestroyedComponent>();
                }
            }
            
            foreach (var k in _sphere)
            {
                _spherePlayerView = (SpherePlayerView) _sphere.Get2(k).View;
                
                foreach (var j in _triggers)
                {
                    _triggerEntity = _triggers.GetEntity(j);
                    _distanceTriggerView = (DistanceTriggerView) _triggers.Get2(j).View;

                    if (!_distanceTriggerView.gameObject.activeSelf ||
                        !_distanceTriggerView.gameObject.activeInHierarchy)
                        continue;

                    if (Vector3.Distance(_spherePlayerView.Transform.position, _distanceTriggerView.Transform.position) >
                        _distanceTriggerView.GetTriggerDistance())
                        continue;

                    foreach (var unlockable in _distanceTriggerView.GetUnlockable())
                        unlockable?.SetActive(true);
                    foreach (var lockable in _distanceTriggerView.GetLockable())
                        lockable?.SetActive(false);
                    _triggerEntity.Get<IsDestroyedComponent>();
                }
            }
        }
    }
    
    public struct DistanceTriggerComponent : IEcsIgnoreInFilter
    {
        
    }
}