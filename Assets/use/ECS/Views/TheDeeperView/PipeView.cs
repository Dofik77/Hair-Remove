using System;
using ECS.Game.Components;
using ECS.Game.Components.TheDeeperComponent;
using Ecs.Views.Linkable.Impl;
using Leopotam.Ecs;
using PdUtils;
using UnityEngine;

namespace ECS.Views.Impls
{
    public class PipeView : LinkableView
    {
        [SerializeField] public PipeComponent.PipeState PipeState;
        [SerializeField] public BoxCollider BoxCollider;
        [SerializeField] public Transform ParticleActivationPoint;
        
        public event Action<PipeView> OnSphereTrigger;
        
        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("Sphere"))
            {
                OnSphereTrigger?.Invoke(this);
            }
        }
    }
}