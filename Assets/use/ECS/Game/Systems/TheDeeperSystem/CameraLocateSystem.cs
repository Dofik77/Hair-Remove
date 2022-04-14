using ECS.Core.Utils.ReactiveSystem;
using ECS.Core.Utils.ReactiveSystem.Components;
using ECS.Game.Components;
using ECS.Game.Components.Flags;
using ECS.Game.Components.TheDeeperComponent;
using ECS.Utils;
using ECS.Views;
using ECS.Views.GameCycle;
using ECS.Views.Impls;
using Game.Utils.MonoBehUtils;
using Leopotam.Ecs;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using Zenject;

namespace ECS.Game.Systems
{
    public class CameraLocateSystem : ReactiveSystem<EventAddComponent<CameraComponent>>
    {
        [Inject] private readonly GetPointFromScene _getPointFromScene;
        protected override EcsFilter<EventAddComponent<CameraComponent>> ReactiveFilter { get; }
        protected EcsFilter<PlayerComponent, LinkComponent> _player { get; }

        private PlayerView _playerView;
        protected override void Execute(EcsEntity entity)
        {
            foreach (var player in _player)
            {
               _playerView = _player.Get2(player).View as PlayerView;

               var cameraPoint = _playerView.GetCameraPoint();
               
               var cameraView = entity.Get<LinkComponent>().View as CameraView;
               
               entity.Get<LinkComponent>().View.Transform.position = cameraPoint.position;
               entity.Get<LinkComponent>().View.Transform.rotation = cameraPoint.rotation;
               
               cameraView.transform.SetParent(_playerView.transform);
               
              
               var cameraData = cameraView.GetCamera().GetUniversalAdditionalCameraData();
               
               foreach (var uIcamera in GameObject.FindGameObjectsWithTag("UiCamera"))
                   if (uIcamera.transform.parent == null)
                   {
                       cameraData.cameraStack.Add(uIcamera.GetComponent<Camera>());
                       break;
                   }
            }
        }
    }
}