using Ecs.Views.Linkable.Impl;
using Leopotam.Ecs;
using UnityEngine;
namespace ECS.Views.Impls
{
    [RequireComponent(typeof(Camera))]
    public class CameraView : LinkableView
    {
        [SerializeField] private Vector2 _defaultResolution = new Vector2(1080, 1920);
        [Range(0f, 1f)] [SerializeField] private float _widthOrHeight = 0;
        public Vector2 DefaultResolution => _defaultResolution;
        public float WidthOrHeight => _widthOrHeight;
        public Camera _camera;
        private float _targetAspect;
        public ref float GetTargetAspect() => ref _targetAspect;
        private float _horizontalFov = 120f;
        public ref float GetHorizontalFov() => ref _horizontalFov;


        public override void Link(EcsEntity entity)
        {
            base.Link(entity);
            _camera = GetComponent<Camera>();
            _targetAspect = DefaultResolution.x / DefaultResolution.y;
            _horizontalFov = CalcVerticalFov(_camera.fieldOfView, 1 / _targetAspect);
        }
        public ref Camera GetCamera()
        {
            return ref _camera;
        }
        
        public float CalcVerticalFov(float hFovInDeg, float aspectRatio)
        {
            return 2 * Mathf.Atan(Mathf.Tan(hFovInDeg * Mathf.Deg2Rad / 2) / aspectRatio) * Mathf.Rad2Deg;
        }
    }
}

    
