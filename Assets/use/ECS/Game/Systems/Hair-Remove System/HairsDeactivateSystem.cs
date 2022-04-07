using DG.Tweening;
using ECS.Core.Utils.SystemInterfaces;
using ECS.Game.Components;
using ECS.Game.Components.Events;
using ECS.Game.Components.Flags;
using ECS.Views.GameCycle;
using Leopotam.Ecs;
using UnityEngine;
using Zenject;

namespace use.ECS.Game.Systems.Hair_Remove_System
{
    public class HairsDeactivateSystem : IEcsUpdateSystem
    {
        [Inject] private SignalBus _signalBus;
        
        private readonly EcsFilter<PlayerComponent, LinkComponent> _player;
        private readonly EcsFilter<HairComponent, LinkComponent> _hairs;
        private readonly EcsFilter<HairUnderRazorComponent, LinkComponent> _hairUnderRazor;
        
        private PlayerView _playerView;
        private Collider _razor;
        private GrassView _grassView;
        private EcsEntity _grassEntity;
        
        public void Run()
        {
            foreach (var player in _player)
            {
                _playerView = _player.Get2(player).View as PlayerView;
                if (_playerView != null) _razor = _playerView.GetRazor();

                foreach (var grass in _hairs)
                {
                    _grassView = _hairs.Get2(grass).View as GrassView;

                    if (Vector3.Distance(_razor.transform.position, _grassView.transform.position) < 1f)
                    {
                        _grassEntity = _hairs.GetEntity(grass);
                        
                        _grassEntity.Get<HairUnderRazorComponent>();

                        foreach (var hairToShave in _hairUnderRazor)
                        {
                            _grassView.Transform.DOLocalMove(Vector3.one, 0.4f).SetEase(Ease.Linear).SetRelative(true).OnComplete(() =>
                            {
                                _hairUnderRazor.GetEntity(hairToShave).Get<AddImpactEventComponent>();
                                _hairUnderRazor.GetEntity(hairToShave).Get<IsDestroyedComponent>();
                            });
                        }
                    }
                }
            }
        }
    }


    public struct HairComponent : IEcsIgnoreInFilter
    {
        
    }

    public struct HairUnderRazorComponent : IEcsIgnoreInFilter
    {
        
    }
    
}