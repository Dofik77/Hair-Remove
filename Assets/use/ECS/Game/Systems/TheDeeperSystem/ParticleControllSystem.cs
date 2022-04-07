using ECS.Core.Utils.SystemInterfaces;
using ECS.Game.Components;
using ECS.Game.Components.Flags;
using ECS.Game.Components.TheDeeperComponent;
using ECS.Views.Impls;
using Leopotam.Ecs;
using DG.Tweening;
using UnityEngine;

namespace ECS.Game.Systems.TheDeeperSystem
{
    public class ParticleControllSystem : IEcsUpdateSystem
    {
        private EcsFilter<ParticleComponent> _particle;
        
        private readonly EcsWorld _world;

        private EcsEntity _particleEffectEntity;
        private ParticleView _particleView;
        private float delay;
        public void Run()
        {
            CheckParticle();
        }
        

        public void CheckParticle()
        {
            foreach (var particle in _particle)
            {
                delay += Time.deltaTime;
                _particleEffectEntity = _particle.GetEntity(particle);
                _particleView = (ParticleView) _particleEffectEntity.Get<LinkComponent>().View;

                if (delay > 3f)
                {
                    _particle.GetEntity(particle).Get<IsDestroyedComponent>();
                    delay = 0f;
                }
            }
        }
    }
}