using System.Diagnostics.CodeAnalysis;
using DataBase.Game;
using ECS.Core.Utils.SystemInterfaces;
using ECS.Game.Components;
using ECS.Game.Components.Flags;
using ECS.Game.Components.Hair_Remove_Component;
using ECS.Game.Components.Input;
using ECS.Game.Components.TheDeeperComponent;
using ECS.Views.GameCycle;
using ECS.Views.Impls;
using Leopotam.Ecs;
using UnityEngine;
using Zenject;

namespace ECS.Game.Systems.Hair_Remove_System
{
    [SuppressMessage("ReSharper", "UnusedVariable")]
    [SuppressMessage("ReSharper", "PossibleNullReferenceException")]
    [SuppressMessage("ReSharper", "Unity.InefficientPropertyAccess")]
    [SuppressMessage("ReSharper", "UnassignedGetOnlyAutoProperty")]
    public class PlayerMovementSystem : IEcsUpdateSystem
    {
        [Inject] private SignalBus _signalBus;

#pragma warning disable 649
        private readonly EcsFilter<PlayerComponent, LinkComponent> _player;

        private readonly EcsFilter<GameStageComponent> _gameStage;
        private readonly EcsFilter<PointerDownComponent> _pointerDown;
        private readonly EcsFilter<PointerUpComponent> _pointerUp;
        private readonly EcsFilter<PointerDragComponent> _pointerDrag;
        private readonly EcsFilter<CameraComponent, LinkComponent> _cameraF;
#pragma warning restore 649

        private PlayerView _playerView;
        private EcsEntity _ballEntity;
        private CameraView _cameraView;
        private Camera _camera;
        private bool _pressed;
        private bool _released = true;
        private Vector2 _pointerDownValueScreen;
        private Vector2 _pointerDragValueScreen;
        private Vector2 _pointerDownValueViewport;
        private Vector2 _pointerDragValueViewport;
        private Vector2 _aspectCorrection;
        private Vector2 _movement;
        private Vector3 _tempPos;
        private readonly float _cameraRotationDeg = 51f;//0 - for camera ( check camera z )
        //0 - for camera ( check camera z )
        private float _sin = Mathf.Sin( 0 * Mathf.Deg2Rad);
        private float _cos = Mathf.Cos(0 * Mathf.Deg2Rad);

        private SignalJoystickUpdate _signalJoystickUpdate =
            new SignalJoystickUpdate(false, Vector2.zero, Vector2.zero);

        private float calculatedSpeed;

        public void Run()
        {
            if (_gameStage.Get1(0).Value != EGameStage.Play) return;

            foreach (var i in _cameraF)
            {
                _cameraView = (CameraView) _cameraF.Get2(i).View;
                _camera = _cameraView.GetCamera();
                _aspectCorrection = new Vector2(1f, _camera.aspect);
            }

            foreach (var i in _pointerDown)
            {
                _pressed = true;
                _released = false;
                _pointerDownValueScreen = _pointerDown.Get1(i).Position;
                _pointerDownValueViewport = _camera.ScreenToViewportPoint(_pointerDownValueScreen);
                _pointerDragValueScreen = _pointerDownValueScreen;
                _pointerDragValueViewport = _camera.ScreenToViewportPoint(_pointerDragValueScreen);
                foreach (var j in _player)
                    _player.GetEntity(j).Get<IsMovingComponent>();;
            }

            foreach (var i in _pointerUp)
            {
                _pressed = false;
                foreach (var j in _player)
                    _player.GetEntity(j).Del<IsMovingComponent>();
            }

            if (!_pressed && !_released)
            {
                HandleRelease();
                _released = true;
            }

            if (!_pressed)
            {
                SendSignalJoystickUpdate(false, Vector2.zero, Vector2.zero);
                return;
            }
            
            foreach (var i in _pointerDrag)
            {
                _pointerDragValueScreen = _pointerDrag.Get1(i).Position;
                _pointerDragValueViewport = _camera.ScreenToViewportPoint(_pointerDragValueScreen);
            }

            HandleHoldAndDrag();
            SendSignalJoystickUpdate(true,
                _camera.ViewportToScreenPoint(_pointerDownValueViewport + _movement * _aspectCorrection),
                _pointerDownValueScreen);
        }

        private void HandleHoldAndDrag()
        {
            foreach (var i in _player)
            {
                _playerView = _player.Get2(i).View as PlayerView;
                _movement = _pointerDragValueViewport - _pointerDownValueViewport;

                _movement.x = Mathf.Clamp(_movement.x, -_playerView.GetMovementLimit(), _playerView.GetMovementLimit());
                _movement.y = Mathf.Clamp(_movement.y, -_playerView.GetMovementLimit(), _playerView.GetMovementLimit());
                _movement.x *= Mathf.Abs(_movement.normalized.x);
                _movement.y *= Mathf.Abs(_movement.normalized.y);

                calculatedSpeed = _playerView.GetMovementSpeed() * _playerView.GetSensitivity() * Time.deltaTime;

                _tempPos = new Vector3(
                    (_movement.x * _cos - _movement.y * _sin) * calculatedSpeed
                    , 0
                    , (_movement.x * _sin + _movement.y * _cos) * calculatedSpeed);

                _playerView.GetRigidbody().AddForce(_tempPos, ForceMode.VelocityChange);
                _playerView.GetRoot().localRotation = Quaternion.Euler(_playerView.GetRoot().localRotation.x,
                    0 + Mathf.Atan2(_movement.x, _movement.y) * 180 / Mathf.PI,
                    _playerView.GetRoot().localRotation.z);
                //0 - for camera ( check camera z )
            }
        }

        private void HandleRelease()
        {
            
        }
        
        private void HandleWTap()
        {
            
        }
        
        private void HandlePress()
        {
            foreach (var i in _player)
            {
                _playerView = _player.Get2(i).View as PlayerView;
                _playerView.GetPushTrigger().enabled = false;
            }
        }

        private void SendSignalJoystickUpdate(bool isPressed, Vector2 buttonPosition, Vector2 originPosition)
        {
            _signalJoystickUpdate.IsPressed = isPressed;
            _signalJoystickUpdate.ButtonPosition = buttonPosition;
            _signalJoystickUpdate.OriginPosition = originPosition;
            _signalBus.Fire(_signalJoystickUpdate);
        }
    }
}