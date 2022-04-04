using DataBase.Game;
using ECS.Core.Utils.SystemInterfaces;
using ECS.Game.Components;
using ECS.Game.Components.Flags;
using ECS.Game.Components.TheDeeperComponent;
using ECS.Utils.Extensions;
using ECS.Views.Impls;
using Leopotam.Ecs;
using UnityEngine;

namespace ECS.Game.Systems.TheDeeperSystem
{
    public class OutOfCameraBorderSystem : IEcsUpdateSystem
    {
        private readonly EcsFilter<CameraComponent, LinkComponent, ActiveCameraComponent> _filterCamera;
        private readonly EcsFilter<SpherePlayerComponent, LinkComponent> _sphere;

        private Camera _camera;

        private EcsEntity _playerEntity;
        private SpherePlayerView _playerView;
        private Vector3 _scaleOfPlayer;
        
        private readonly EcsWorld _world;
        
        public void Run()
        {
            CheckPlayerPosition();
        }

        private void CheckPlayerPosition()
        {
            foreach (var sphere in _sphere)
            {
                _playerEntity = _sphere.GetEntity(sphere);
                _playerView = (SpherePlayerView) _playerEntity.Get<LinkComponent>().View;
                
                if (!InCameraBorder(_playerView.transform))
                {
                    _world.SetStage(EGameStage.Lose);
                }
            }
        }

        private bool InCameraBorder(Transform playerTransform)
        {
            _camera = GetCameraFromFilter();
            _scaleOfPlayer = playerTransform.localScale;
            var position = playerTransform.position;
            
            //TODO goodcase
            Vector3 screenPoint = _camera.WorldToViewportPoint(position);
            bool onScreen = screenPoint.x + 0.4 > 0 
                            && screenPoint.x - 0.4 < 1 
                            && screenPoint.y  + 0.4 > 0 
                            && screenPoint.y - 0.4 < 1;

            return onScreen;
        }
        
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