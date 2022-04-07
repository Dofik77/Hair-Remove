using System;
using Ecs.Views.Linkable.Impl;
using UnityEngine;

namespace ECS.Views.Impls
{
    public class SpherePlayerView : LinkableView
    {
        [SerializeField] public Rigidbody Rigidbody;
        [SerializeField] public GameObject SimpleEmoji;
        [SerializeField] public GameObject ScareEmoji;
        [SerializeField] public GameObject CrushedEmoji;
        [SerializeField] public GameObject RotationEmoji;

        public Vector3 CollisionForce;

        private void OnCollisionEnter(Collision collision)
        {
            CollisionForce = collision.impulse;
            Debug.Log(CollisionForce);
        }
    }
}