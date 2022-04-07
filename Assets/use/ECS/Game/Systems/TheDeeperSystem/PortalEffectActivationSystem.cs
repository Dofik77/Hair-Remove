using ECS.Core.Utils.SystemInterfaces;
using ECS.Game.Components;
using ECS.Game.Components.TheDeeperComponent;
using ECS.Views.Impls;
using Leopotam.Ecs;

namespace ECS.Game.Systems
{
    public class PortalEffectActivationSystem : IEcsUpdateSystem
    {
        private readonly EcsFilter<PortalComponent, LinkComponent, EffectActivationComponent, UIdComponent> _effectsFilter;
        private EcsFilter<PortalComponent, LinkComponent, UIdComponent> _portals;

        private EcsEntity _portalWithEffectsComponentEntity;
        private PortalView _portalWithEffectsComponentView;
        private PortalComponent.PortalColor _portalColorWithEffectsComponent;
        
        private EcsEntity _portalEntity;
        private PortalView _portalView;
        private PortalComponent.PortalColor _portalColor;

        public void Run()
        {
            foreach (var effects in _effectsFilter)
            {
                _portalWithEffectsComponentEntity = _effectsFilter.GetEntity(effects);
                _portalColorWithEffectsComponent = _portalWithEffectsComponentEntity.Get<PortalComponent>().color;
                _portalWithEffectsComponentView = (PortalView) _portalWithEffectsComponentEntity.Get<LinkComponent>().View;
                
                var id = _portalWithEffectsComponentEntity.Get<UIdComponent>().Value;

                foreach (var portal in _portals)    
                {
                    _portalEntity = _portals.GetEntity(portal);
                    _portalView = (PortalView) _portalEntity.Get<LinkComponent>().View;
                    _portalColor = _portalView.color;

                    if (_portalColorWithEffectsComponent == _portalColor && id != _portalEntity.Get<UIdComponent>().Value)
                    {
                        _portalWithEffectsComponentView.PortalEffect.SetActive(true);
                        _portalView.PortalEffect.SetActive(true);
                    }
                }
            }
        }
    }
}