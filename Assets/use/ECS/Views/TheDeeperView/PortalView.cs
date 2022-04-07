using System;
using ECS.Game.Components;
using ECS.Game.Components.TheDeeperComponent;
using Ecs.Views.Linkable.Impl;
using Leopotam.Ecs;
using PdUtils;
using Services.Uid;
using UnityEngine;
using Object = System.Object;

namespace ECS.Views.Impls
{
    public class PortalView : LinkableView
    {
        public event Action<Uid, PortalComponent.PortalColor, Transform> OnSphereTrigger;

        [SerializeField] public PortalComponent.PortalColor color;
        [SerializeField] public GameObject PointToLocate;
        [SerializeField] public BoxCollider StopCollider;
        [SerializeField] public GameObject PortalEffect;
        [SerializeField] public GameObject Holders;

        private void OnTriggerStay(Collider other)
        {
            if (other.gameObject.CompareTag("Sphere"))
            {
                OnSphereTrigger?.Invoke(Entity.Get<UIdComponent>().Value, 
                    Entity.Get<PortalComponent>().color, transform);
                StopCollider.enabled = false;
                //srp - translate not view, transfer id View
            }
        }
        
        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject.CompareTag("Sphere"))
            {
                StopCollider.enabled = true;
            }
        }
    }
}