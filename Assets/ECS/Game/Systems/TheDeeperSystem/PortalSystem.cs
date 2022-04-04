using System;
using ECS.Core.Utils.ReactiveSystem;
using ECS.Core.Utils.ReactiveSystem.Components;
using ECS.Core.Utils.SystemInterfaces;
using ECS.Game.Components;
using ECS.Game.Components.Flags;
using ECS.Game.Components.TheDeeperComponent;
using ECS.Utils.Extensions;
using ECS.Views.Impls;
using Game.Utils.MonoBehUtils;
using Leopotam.Ecs;
using ModestTree;
using PdUtils;
using UnityEngine;
using UnityEngine.PlayerLoop;
using Zenject;

namespace ECS.Game.Systems
{
    public class PortalSystem : ReactiveSystem<EventAddComponent<PortalComponent>>
    {
        [Inject] private readonly GetPointFromScene _getPointFromScene;
        protected override EcsFilter <EventAddComponent<PortalComponent>> ReactiveFilter { get; }
        private EcsFilter<PortalComponent> _portals;
        private EcsFilter<SpherePlayerComponent, LinkComponent> _sphere;
        private EcsFilter<ActivePortalComponent> _activeComponent;

        private PortalView _portalView;

        private readonly EcsWorld _world;
        private EcsEntity _sphereParticle;
        
        protected override void Execute(EcsEntity entity)
        {
            _portalView = entity.Get<LinkComponent>().View as PortalView;
            _portalView.OnSphereTrigger += FindAnotherPortal;
        }
        
        
        void FindAnotherPortal(Uid id, PortalComponent.PortalColor enumColor, Transform enterPortalTransform)
        {
            foreach (var i in _portals)
            {
                var exitPortalEntity = _portals.GetEntity(i);
                var exitPortalColor = exitPortalEntity.Get<PortalComponent>().color; 

                if (enumColor == exitPortalColor 
                    && id != exitPortalEntity.Get<UIdComponent>().Value)
                {
                    TeleportSphere(exitPortalEntity, enterPortalTransform);
                } 
            }
        }

        //TODO SRP
        private void TeleportSphere(EcsEntity exitPortalEntity, Transform enterPortalTransform)
        {
            foreach (var i in _sphere)
            {
                var sphereView = _sphere.Get2(i).View as SpherePlayerView;
                var exitPortalView = (PortalView) exitPortalEntity.Get<LinkComponent>().View;

                if (sphereView != null)
                {
                    var sphereVelocity = sphereView.Rigidbody.velocity;

                    // var enterPortalAngle = -enterPortalTransform.localEulerAngles +
                    //                        exitPortalView.transform.localEulerAngles + new Vector3(0,0,180);
                    
                    //разворот относительно нового портала ( инверсия поворота ) 
                    
                    // x1=x*cos(angle) - y*sin(angle); 
                    // y1=y*cos(angle) + x*sin(angle);
                    // разворот вектора - жесть 
                    //TODO goodcase
                    
                    var enterPortalAngle = exitPortalView.transform.localEulerAngles + 
                                           new Vector3(0,0,Vector2.SignedAngle(sphereVelocity, Vector2.up));
                    
                    var newSphereVelocity = new Vector3();
                    
                    newSphereVelocity.x = sphereVelocity.x * Mathf.Cos(enterPortalAngle.z * Mathf.Deg2Rad) -
                                          sphereVelocity.y * Mathf.Sin(enterPortalAngle.z * Mathf.Deg2Rad);
                    
                    newSphereVelocity.y = sphereVelocity.y * Mathf.Cos(enterPortalAngle.z * Mathf.Deg2Rad) +
                                          sphereVelocity.x * Mathf.Sin(enterPortalAngle.z * Mathf.Deg2Rad);

                    var sphereTransform = sphereView.Rigidbody;
                    var exitPortalTransform = exitPortalView.transform;
                    var exitPortalPoint = exitPortalView.PointToLocate.transform;
                    
                    //In Player System ( Maybe another ) 
                    
                    _world.CreateParticle(enterPortalTransform.position, new Quaternion(),"SphereEffect");
                    
                    sphereTransform.position =
                        exitPortalPoint.position; //Poscomp
                    
                    _world.CreateParticle(exitPortalTransform.position, new Quaternion(),"SphereEffect");

                    sphereView.Rigidbody.velocity =
                         newSphereVelocity;
                }
            }
        }
    }
}