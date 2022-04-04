using System.Collections.Generic;
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
    public class LocateObjectByTagSystem : IEcsUpdateSystem
    {
        
        [Inject] private readonly GetPointFromScene _getPointFromScene;
        
        private readonly EcsFilter<EventInputHoldAndDragComponent> _eventInputHoldAndDragComponent; // Press
        private readonly EcsFilter<EventInputDownComponent> _eventInputDownComponent; // Down
        private readonly EcsFilter<EventInputUpComponent> _eventInputUpComponent; // Up
        
        private readonly EcsFilter<CameraComponent, LinkComponent, ActiveCameraComponent> _filterCamera;
        
        private readonly EcsFilter<PortalComponent, LinkComponent, ActivePortalComponent> _activePortal;
        private readonly EcsFilter<InActionPortalComponent, LinkComponent> _inActionPortal;
        
        private readonly EcsWorld _world;
        
        private EcsEntity _newPortal;
        private PortalComponent.PortalColor _wallColor;

        private float _deltaPos;
        private Vector2 _prevPos;
        private Vector2 _downPos;
        private Camera _actualCamera;
        
        private SpherePlayerView _sphereView;
        private EcsEntity _playerEntity;
        private Transform _spawnPoint;
        private PipeView _pipeView;

        public void Run()
        {
            TryGetObjectInWorldSpace();
            DragPortal();
        }
        
        //TODO SRP
        private void TryGetObjectInWorldSpace()
        {
            foreach (var downInput in _eventInputDownComponent)
            {
                _downPos = _eventInputDownComponent.Get1(downInput).Down;

                if(GetTagFromPointInWorldSpace(out Vector3 locatePoint, out RaycastHit raycastHit, out string objectTag, _downPos))
                {
                    if (objectTag == "Wall")
                    {
                        _wallColor = GetColorFromWall(raycastHit);

                        if (_wallColor != PortalComponent.PortalColor.Default)
                        {
                            _newPortal = CreateActualPortal(_wallColor);

                            foreach (var activePortal in _activePortal)
                            {
                                if (_activePortal.Get1(activePortal).color == _newPortal.Get<PortalComponent>().color)
                                    _activePortal.GetEntity(activePortal).Get<IsDestroyedComponent>();
                            }

                            var newPosition = new Vector3(locatePoint.x, locatePoint.y, locatePoint.z - 0.51f);

                            _newPortal.Get<ActivePortalComponent>();
                            _newPortal.Get<InActionPortalComponent>();
                            _newPortal.Get<EffectActivationComponent>();
                            _newPortal.Get<SetPositionComponent>().position = newPosition;
                        }
                    }
                    
                    else if (objectTag == "Pipe")
                    {
                        _pipeView = GetViewFromPipe(raycastHit);
                        
                        if (_pipeView.PipeState == PipeComponent.PipeState.Enter)
                        {
                            _pipeView.gameObject.tag = "DeactivePipe";
                     
                            _playerEntity = GetPlayer();
                        
                            _spawnPoint = _getPointFromScene.GetPoint("Player");
                            _playerEntity.Get<SetPositionComponent>().position = _spawnPoint.transform.position;
                        }

                        else
                        {
                            return;
                        }
                       
                    }
                }
                _eventInputDownComponent.GetEntity(downInput).Del<EventInputDownComponent>();
            }
        }
        //TODO SRP
        private bool GetTagFromPointInWorldSpace(out Vector3 locatePoint, out RaycastHit raycastHit, out string objectTag, Vector3 inputPos)
        {
            _actualCamera = GetCameraFromFilter();
                
            var ray = _actualCamera.ScreenPointToRay(inputPos);
            var hasHit = Physics.Raycast(ray, out raycastHit, 100f, LayerMask.GetMask("Pipe","Wall"));
            
            locatePoint = raycastHit.point;
            
            switch (raycastHit.transform.gameObject.tag)
            {
                case "Wall" :
                    objectTag = "Wall";
                    break;
                case "Pipe" :
                    objectTag = "Pipe";
                    break;
                default:
                    objectTag = "Default";
                    break;
            }
                
            return hasHit;
        }
        //TODO SRP
        private void DragPortal()
        {
            foreach (var holdAndDrag in _eventInputHoldAndDragComponent) // перенести
            {
                foreach (var inActionPortal in _inActionPortal)
                {
                    var dragPos = _eventInputHoldAndDragComponent.Get1(holdAndDrag).Drag;
                    var downPos = _eventInputHoldAndDragComponent.Get1(holdAndDrag).Down;

                    var deltaDif = downPos - dragPos;
                    
                    var angle = Vector2.SignedAngle(deltaDif, Vector2.down);

                    _inActionPortal.GetEntity(inActionPortal).Get<SetRotationComponent>().deltaAngle =
                        angle;
                }
                _eventInputHoldAndDragComponent.GetEntity(holdAndDrag).Del<EventInputHoldAndDragComponent>();
            }
            
            foreach (var inputUp in _eventInputUpComponent)
            {
                foreach (var inActionPortal in _inActionPortal)
                {
                    _inActionPortal.GetEntity(inActionPortal).Del<InActionPortalComponent>();
                }
                _eventInputUpComponent.GetEntity(inputUp).Del<EventInputUpComponent>();
            }
        }
        
        private PipeView GetViewFromPipe(RaycastHit raycastHit)
        {
            var pipeGameObject = raycastHit.transform.gameObject;
            _pipeView = pipeGameObject.GetComponent<PipeView>();
            
            return _pipeView;
        }
        
        private EcsEntity GetPlayer()
        {
            var player = _world.CreatePlayer();
            
            return player;
        }
        
        private PortalComponent.PortalColor GetColorFromWall(RaycastHit raycastHit)
        {
            if (raycastHit.transform.gameObject.TryGetComponent(out WallView wallView))
                _wallColor = (PortalComponent.PortalColor) wallView.color;
            
            return _wallColor;
        }
        
        private EcsEntity CreateActualPortal(PortalComponent.PortalColor portalColor)
        {
            var portal = _world.CreatePortal(portalColor);

            return portal;
        }
        //TODO SRP
        private Camera GetCameraFromFilter()
        {
            Camera actualCamera = null;
            
            foreach (var camera in _filterCamera)
            {
                var cameraView = (CameraView) _filterCamera.Get2(camera).View;
                actualCamera = cameraView._camera;
            }

            return actualCamera != null ? actualCamera : null;
        }
    }
}