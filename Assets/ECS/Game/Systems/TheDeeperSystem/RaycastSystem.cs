using ECS.Core.Utils.SystemInterfaces;
using ECS.Game.Components;
using ECS.Game.Components.TheDeeperComponent;
using ECS.Views.Impls;
using Leopotam.Ecs;
using UnityEngine;

namespace ECS.Game.Systems
{
    public class RaycastSystem : IEcsUpdateSystem
    {   
        
        private readonly EcsFilter<CameraComponent, LinkComponent, ActiveCameraComponent> _filterCamera;
        
        public void Run()
        {
            
        }
        
        // private void TryGetPipePointInWorldSpace(out Vector3 locatePoint ,out RaycastHit raycastHit, LayerMask targetLayer, Vector3 inputPos)
        // {
        //     var actualCamera = GetCameraFromFilter();
        //     var ray = actualCamera.ScreenPointToRay(inputPos);
        //     var hasHit = Physics.Raycast(ray, out raycastHit,100f,targetLayer);
        //     // return hasHit;
        // }
        
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