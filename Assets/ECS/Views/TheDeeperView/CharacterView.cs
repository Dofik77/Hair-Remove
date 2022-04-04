using System;
using DataBase.Game;
using DG.Tweening;
using Ecs.Views.Linkable.Impl;
using Leopotam.Ecs;
using NaughtyAttributes;
using UnityEngine;

namespace ECS.Views.Impls
{
    public class CharacterView : LinkableView
    {
        [SerializeField] private Animator animator;
        [SerializeField] private Transform modelRoot;
        [SerializeField] private Transform camera;
        [SerializeField] private GameObject[] models;
        
        
        public Transform ModelRootTransform => modelRoot;
        private static readonly int Walk = Animator.StringToHash("Walk");

        public void SetStage(int value)
        {
            animator.SetFloat(Walk, value);
            for (var i = 0; i < models.Length; i++)
            {
                var model = models[i];
                model.SetActive(i == (value <= 0 ? 0 : value - 1));
                modelRoot.DOLocalRotate(new Vector3(0, value == 4 ? 180 : 0, 0), 1);
            }
        }

        public void FinalCamera(Action onComplete = null)
        {
            camera.DOLocalMove(new Vector3(0.1f,9.5f,-21.8f), 0.5f).OnComplete(()=> onComplete?.Invoke());
            camera.DOLocalRotate(new Vector3(0,360,0), 0.5f);
        }

        public void SetStage(EGameResult result) => animator.SetBool(result == EGameResult.Win ? "Win" : "Fail", true);
    }
}