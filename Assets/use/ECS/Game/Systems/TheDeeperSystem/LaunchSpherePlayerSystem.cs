using System.Collections;
using ECS.Core.Utils.ReactiveSystem;
using ECS.Core.Utils.ReactiveSystem.Components;
using ECS.Core.Utils.SystemInterfaces;
using ECS.Game.Components;
using ECS.Game.Components.Flags;
using ECS.Game.Components.TheDeeperComponent;
using ECS.Game.Systems.GameCycle;
using ECS.Utils.Extensions;
using ECS.Views.Impls;
using Game.Utils.MonoBehUtils;
using Leopotam.Ecs;
using UnityEngine;
using Zenject;

namespace ECS.Game.Systems
{
    public class LaunchSpherePlayerSystem :IEcsUpdateSystem
    {
        [Inject] private readonly GetPointFromScene _getPointFromScene;
        
        private readonly EcsFilter<EventInputDownComponent> _eventInputDownComponent;
        private readonly EcsFilter<EventInputUpComponent> _eventInputUpComponent;
        private readonly EcsFilter<CameraComponent, LinkComponent> _filterCamera;
        private readonly EcsFilter<SpherePlayerComponent, LinkComponent> _player;

        private readonly EcsWorld _world;
        
        private readonly LayerMask _pipeLayerMask = LayerMask.GetMask($"Pipe");
        
        private Vector2 _downPos;

        private int _playerCounter = 0;

        private SpherePlayerView _sphereView;
        private EcsEntity _playerEntity;
        private Transform _spawnPoint;
        private PipeView _pipeView;

        public void Run()
        {
            InstantiatePlayer();
        }

        private void InstantiatePlayer()
        {
            if (TryGetPipePointInWorldSpace(out RaycastHit raycastHit, _pipeLayerMask, _downPos))
            {
                foreach (var downInput in _eventInputDownComponent)
                {
                    _downPos = _eventInputDownComponent.Get1(downInput).Down;
                    
                     _pipeView = GetViewFromPipe(raycastHit);
                     _pipeView.BoxCollider.enabled = false;
                     
                     _playerEntity = GetPlayer();
                    
                     _sphereView = (SpherePlayerView) _playerEntity.Get<LinkComponent>().View;
                     _spawnPoint = _getPointFromScene.GetPoint("Player");
                     
                     _sphereView.transform.position = _spawnPoint.position;
                     _sphereView.transform.rotation = Quaternion.Euler(0,180,0);
                     _eventInputDownComponent.GetEntity(downInput).Del<EventInputDownComponent>();
                }
            }

            else
            {
                foreach (var downInput in _eventInputDownComponent)
                {
                    _eventInputDownComponent.GetEntity(downInput).Del<EventInputDownComponent>();
                }
            }
            
            foreach (var inputUp in _eventInputUpComponent)
            {
                _eventInputUpComponent.GetEntity(inputUp).Del<EventInputUpComponent>();
            }
        }
        
        private EcsEntity GetPlayer()
        {
            var player = _world.CreatePlayer();
            
            return player;
        }

        private PipeView GetViewFromPipe(RaycastHit raycastHit)
        {
            var pipeGameObject = raycastHit.transform.gameObject;
            _pipeView = pipeGameObject.GetComponent<PipeView>();
            
            return _pipeView;
        }

        private bool TryGetPipePointInWorldSpace(out RaycastHit raycastHit, LayerMask targetLayer, Vector3 inputPos)
        {
            var actualCamera = GetCameraFromFilter();
            var ray = actualCamera.ScreenPointToRay(inputPos);
            var hasHit = Physics.Raycast(ray, out raycastHit,100f,targetLayer);
            
            return hasHit;
        }
        
        private Camera GetCameraFromFilter()
        {
            Camera actualCamera = null;
            
            foreach (var camera in _filterCamera)
            {
                var cameraView = (CameraView) _filterCamera.Get2(camera).View;
                actualCamera = cameraView.GetCamera();
            }

            return actualCamera != null ? actualCamera : null;
        }
    }
}