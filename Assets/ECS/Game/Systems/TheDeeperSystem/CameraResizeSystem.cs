using ECS.Core.Utils.SystemInterfaces;
using ECS.Game.Components;
using ECS.Game.Components.Flags;
using ECS.Game.Components.TheDeeperComponent;
using ECS.Views.Impls;
using Leopotam.Ecs;
using UnityEngine;
namespace ECS.Game.Systems.GameCycle
{
    public class CameraResizeSystem : IEcsUpdateSystem
    {
        private readonly EcsFilter<CameraComponent, LinkComponent> _cameraF;
        private CameraView _cameraView;
        public void Run()
        {
            foreach (var i in _cameraF)
            {
                _cameraView = (CameraView) _cameraF.Get2(i).View;
                if (_cameraView.GetCamera().orthographic)
                {
                    _cameraView.GetCamera().orthographicSize = Mathf.Lerp(
                        _cameraView.GetCamera().orthographicSize *
                        (_cameraView.GetTargetAspect() / _cameraView.GetCamera().aspect),
                        _cameraView.GetCamera().orthographicSize, _cameraView.WidthOrHeight);
                }
                else
                {
                    _cameraView.GetCamera().fieldOfView =
                        Mathf.Lerp(
                            _cameraView.CalcVerticalFov(_cameraView.GetHorizontalFov(), _cameraView.GetCamera().aspect),
                            _cameraView.GetCamera().fieldOfView, _cameraView.WidthOrHeight);
                }
            }
        }
    }
}