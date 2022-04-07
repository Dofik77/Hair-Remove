using System;
using ECS.Game.Components;
using Ecs.Views.Linkable.Impl;
using Leopotam.Ecs;
using UnityEngine;

namespace ECS.Views.GameCycle
{
    public class GrassView : LinkableView
    {
        [SerializeField] private int impact;
        private Action _onTriggerEnter;
        private Action _onCollisionEnter;

        public override void Link(EcsEntity entity)
        {
            base.Link(entity);
            entity.Get<ImpactComponent>().Value = impact;
        }

        public void SetTriggerAction(Action onTriggerEnter) => _onTriggerEnter = onTriggerEnter;
        protected virtual void OnTriggerEnter(Collider other)
        {
            gameObject.SetActive(false);
            _onTriggerEnter?.Invoke();
        }

        protected virtual void OnCollisionEnter(Collision collision)
        {
            _onCollisionEnter?.Invoke();
        }
    }
}