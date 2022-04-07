    using DG.Tweening;
    using ECS.Core.Utils.SystemInterfaces;
    using ECS.Game.Components;
    using ECS.Game.Components.Events;
    using ECS.Game.Components.Flags;
    using ECS.Views.GameCycle;
    using Leopotam.Ecs;
    using UnityEngine;
    using Zenject;

    namespace ECS.Game.Systems.Hair_Remove_System
{
    public class GrassDeactivateSystem : IEcsUpdateSystem
    {
        [Inject] private SignalBus _signalBus;
        
        private readonly EcsFilter<PlayerComponent, LinkComponent> _player;
        private readonly EcsFilter<GrassComponent, LinkComponent> _grasses;
        
        private PlayerView _playerView;
        private Collider _scissors;
        private GrassView _grassView;
        private EcsEntity _grassEntity;
        
        public void Run()
        {
            foreach (var player in _player)
            {
                _playerView = _player.Get2(player).View as PlayerView;
                if (_playerView != null) _scissors = _playerView.GetScissors();

                foreach (var grass in _grasses)
                {
                    _grassView = _grasses.Get2(grass).View as GrassView;

                    if (Vector3.Distance(_scissors.transform.position, _grassView.transform.position) < 0.5f)
                    {
                        _grassEntity = _grasses.GetEntity(grass);
                        
                        _grassView.Transform.DOLocalMove(Vector3.one, 0.4f).SetEase(Ease.Linear).SetRelative(true).OnComplete(() =>
                        {
                            _grassEntity.Get<AddImpactEventComponent>();
                            _grassEntity.Get<IsDestroyedComponent>(); 
                        });
                    }
                }
            }
        }
    }


    public struct GrassComponent : IEcsIgnoreInFilter
    {
        
    }
    
}