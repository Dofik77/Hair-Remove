using Ecs.Views.Linkable.Impl;
using UnityEngine;

namespace ECS.Views.Impls
{
    public class ParticleView : LinkableView
    {
        [SerializeField] public ParticleSystem _thisParticle;
    }
}