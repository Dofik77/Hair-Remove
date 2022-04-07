using ECS.Core.Utils.SystemInterfaces;
using ECS.Game.Components;
using ECS.Game.Components.TheDeeperComponent;
using Leopotam.Ecs;

namespace ECS.Game.Systems
{
    public class PositionSystem : IEcsUpdateSystem 
    {
        private readonly EcsFilter<SetPositionComponent, LinkComponent> _portalPosition;
        public void Run()
        {
            foreach (var position in _portalPosition)
            {
                _portalPosition.Get2(position).View.Transform.position =
                    _portalPosition.Get1(position).position;
                
                _portalPosition.GetEntity(position).Del<SetPositionComponent>();
            }
        }
        
    }
}