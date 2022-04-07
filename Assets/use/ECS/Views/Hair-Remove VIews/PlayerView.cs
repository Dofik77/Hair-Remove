﻿using System.Diagnostics.CodeAnalysis;
using ECS.Game.Systems.GameCycle;
using ECS.Views.Impls;
using Ecs.Views.Linkable.Impl;
using Leopotam.Ecs;
using UnityEngine;
using Zenject;

namespace ECS.Views.GameCycle
{
    [SuppressMessage("ReSharper", "Unity.InefficientPropertyAccess")]
    public class PlayerView : LinkableView
    {
        [SerializeField] private Collider _razor;
        [SerializeField] private Animator _animator;
        [SerializeField] private Rigidbody _rigidbody;
        [SerializeField] private Transform _root;
        [SerializeField] private Collider _pushTrigger;
        [SerializeField] private SkinnedMeshRenderer _renderer;
        [SerializeField] private Material _damagedMaterial;
        [SerializeField] private Transform _shackle;
        
        [SerializeField] private float _interactionDistance = 2.5f;
        [SerializeField] private float _interactionDuration = 0.4f;

        [SerializeField] private float _sensitivity = 0.4f;
        [SerializeField] private float _movementLimit = 0.045f;
        [SerializeField] private float _movementSpeed;
        // [SerializeField] private float _movementSpeedToAnim = 1.46f;
        [SerializeField] private float _rigidbodyPushForceMultiplier;
        
        [SerializeField] private int _hp = 100;
        [SerializeField] private int _maxHp = 100;
        [SerializeField] private float _afterDamageInvincibleDuration = 0.5f;

#pragma warning disable 414
        private readonly int Idle = 0;
        private readonly int Walk = 1;
        private readonly int JumpToBall = 2;
        private readonly int Attack = 3;
        private readonly int TakeHit = 4;
        private readonly int Death = 5;
        private readonly int Stage = Animator.StringToHash("Stage");
        private readonly string WalkMultiplier = "WalkMultiplier";
#pragma warning restore 414

        private Material _originMaterial;
        
        public ref Transform GetRoot() => ref _root;

        public ref float GetSensitivity() => ref _sensitivity;
        
        public ref float GetMovementLimit() => ref _movementLimit;
        
        public ref Collider GetRazor() => ref _razor;

        public ref Rigidbody GetRigidbody() => ref _rigidbody;
       
        public ref float GetMovementSpeed() => ref _movementSpeed;
       
        public ref Collider GetPushTrigger() =>  ref _pushTrigger;
      
        
        public ref float GetRigidbodyPushForceMultiplier()
        {
            return ref _rigidbodyPushForceMultiplier;
        }


        public ref float GetInteractionDistance()
        {
            return ref _interactionDistance;
        }
        
        public ref float GetInteractionDuration()
        {
            return ref _interactionDuration;
        }

        public void SetWalkAnimation()
        {
            if (_animator.GetInteger(Stage) == Walk)
                return;
            _animator.SetInteger(Stage, Walk);
        }

        public void SetIdleAnimation()
        {
            if (_animator.GetInteger(Stage) == Idle)
                return;
            _animator.SetInteger(Stage, Idle);
        }
        
        public void SetJumpToBallAnimation()
        {
            _animator.SetInteger(Stage, JumpToBall);
        }
        
        public void SetDeathAnimation()
        {
            _animator.SetInteger(Stage, Death);
        }
        
        public void SetTakeHitAnimation()
        {
            _animator.SetInteger(Stage, TakeHit);
        }
       
      

        public ref float GetAfterDamageInvincibleDuration()
        {
            return ref _afterDamageInvincibleDuration;
        }
        
        public ref Material GetOriginMaterial()
        {
            return ref _originMaterial;
        }
        
        public ref Material GetDamagedMaterial()
        {
            return ref _damagedMaterial;
        }
        
        public ref SkinnedMeshRenderer GetRenderer()
        {
            return ref _renderer;
        }
        
        public ref Transform GetShackle()
        {
            return ref _shackle;
        }
        
        public ref int GetMaxHp()
        {
            return ref _maxHp;
        }
    }
}