namespace ConsoleFPS
{
    public class GameEntity
    {
        public PlayerEntity Player { get; set; }
        public KeysEntity Keys { get; set; }
        public string[] Map { get; set; }
        public float RenderDepth { get; set; } = 16.0f;

        public GameEntity(PlayerEntity player, string[] map)
        {
            Player = player;
            Map = map;
            
            Keys = new KeysEntity();
        }
    }
}