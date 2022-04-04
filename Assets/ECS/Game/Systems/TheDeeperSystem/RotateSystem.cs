using DG.Tweening;
using ECS.Core.Utils.SystemInterfaces;
using ECS.Game.Components;
using ECS.Game.Components.TheDeeperComponent;
using ECS.Views.Impls;
using Leopotam.Ecs;
using UnityEngine;

namespace ECS.Game.Systems
{
    public class RotateSystem : IEcsUpdateSystem
    {
        private readonly EcsFilter<SetRotationComponent, LinkComponent> _portalRotation;

        private Camera _camera;
        
        public void Run()
        {
            foreach (var rotation in _portalRotation)
            {
                var portalView = (PortalView) _portalRotation.Get2(rotation).View;
                var angle = _portalRotation.Get1(rotation).deltaAngle;

                portalView.transform.rotation = Quaternion.Euler(0, 0, -angle);

                _portalRotation.GetEntity(rotation).Del<SetRotationComponent>();
            }
        }
    }
}