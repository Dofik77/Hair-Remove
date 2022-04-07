namespace ECS.Game.Components.TheDeeperComponent
{
    public struct WallComponent
    {
        public WallColor color;
        public enum WallColor
        {
            Red,
            Green,
            Blue,
            Purple,
            Default
        }
        //если добавить none - все сломается ( почему??? ) 
    }
}