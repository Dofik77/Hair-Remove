using Leopotam.Ecs;

namespace ECS.Game.Components.TheDeeperComponent
{
    public struct PipeComponent : IEcsIgnoreInFilter
    {
        public PipeState pipeState;
        public enum PipeState
        {
            Enter,
            Exit
        }
        
    }
}