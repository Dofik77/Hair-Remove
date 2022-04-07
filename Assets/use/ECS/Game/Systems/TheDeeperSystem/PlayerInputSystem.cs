using DataBase.Game;
using ECS.Core.Utils.SystemInterfaces;
using ECS.Game.Components;
using ECS.Game.Components.Flags;
using ECS.Game.Components.Input;
using ECS.Game.Components.TheDeeperComponent;
using ECS.Views.Impls;
using Leopotam.Ecs;

using UnityEngine;
namespace ECS.Game.Systems.GameCycle
{
    public class PlayerInputSystem : IEcsUpdateSystem
    {
#pragma warning disable 
        private readonly EcsWorld _world;
        private readonly EcsFilter<GameStageComponent> _gameStage;
        private readonly EcsFilter<PointerDownComponent> _pointerDown;
        private readonly EcsFilter<PointerUpComponent> _pointerUp;
        private readonly EcsFilter<PointerDragComponent> _pointerDrag;
        private readonly EcsFilter<CameraComponent, LinkComponent, ActiveCameraComponent> _cameraF;
#pragma warning restore 649
        private CameraView _cameraView;
        private Camera _camera;
        private EcsEntity _cameraEntity;
        
        private bool _pressed;
        private bool _released = true;
        private Vector2 _pointerDownValueScreen;
        private Vector2 _pointerDragValueScreen;
        private Vector2 _pointerUpValueScreen;
        private Vector2 _movement;
        private Vector3 _tempPos;
        private float _angle;
        private float _clampedAngle;
        
        public void Run()
        {
            if (_gameStage.Get1(0).Value != EGameStage.Play) return;
            foreach (var i in _cameraF)
            {
                _cameraView = _cameraF.Get2(i).View as CameraView;
                _camera = _cameraView.GetCamera();
                _cameraEntity = _cameraF.GetEntity(i);
            }
            foreach (var i in _pointerDown)
            {
                _pressed = true;
                _released = false;
                _pointerDownValueScreen = _pointerDown.Get1(i).Position;
                _pointerDragValueScreen = _pointerDownValueScreen;
                HandlePress();
            }
            foreach (var i in _pointerUp)
            {
                _pressed = false;
                _pointerUpValueScreen = _pointerUp.Get1(i).Position;
            }
            if (!_pressed && !_released)
            {
                HandleRelease();
                _released = true;
            }
            if (!_pressed)
            {
                return;
            }
            
            foreach (var i in _pointerDrag)
            {
                _pointerDragValueScreen = _pointerDrag.Get1(i).Position;
            }
            HandleHoldAndDrag();
        }
        private void HandleHoldAndDrag()
        {
            _cameraEntity.Get<EventInputHoldAndDragComponent>().Drag = _pointerDragValueScreen;   
            _cameraEntity.Get<EventInputHoldAndDragComponent>().Down = _pointerDownValueScreen;  
        }
        private void HandleRelease() => _cameraEntity.Get<EventInputUpComponent>().Up = _pointerUpValueScreen;
        
        private void HandlePress() => _cameraEntity.Get<EventInputDownComponent>().Down = _pointerDownValueScreen;
    }
    public struct EventInputHoldAndDragComponent
    {
        public Vector2 Down;
        public Vector2 Drag;
    }
    
    public struct EventInputDownComponent
    {
        public Vector2 Down;
    }
    
    public struct EventInputUpComponent
    {
        public Vector2 Up;
    }
}