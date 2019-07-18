
using SadConsole;

namespace ConsoleFPS
{
    public static class Program
    {
        static void Main()
        {
            Game.Create(120, 40);
            Game.OnInitialize = OnInitialize;
            Game.Instance.Run();
            Game.Instance.Dispose();
        }


        private static void OnInitialize()
        {
            var map = new[]
            {
                  "#########.......",
                  "#...............",
                  "#.......########",
                  "#..............#",
                  "#......##......#",
                  "#......##......#",
                  "#..............#",
                  "###............#",
                  "##.............#",
                  "#......####..###",
                  "#......#.......#",
                  "#......#.......#",
                  "#..............#",
                  "#......#########",
                  "#..............#",
                  "################"
            };

            var player = new PlayerEntity(14.7f,5.09f);
            var game = new GameEntity(player, map);

            var console = new Console(120, 40) { IsFocused = true };
            console.Components.Add(new GameMapHudComponent(game));
            console.Components.Add(new GameStatusHudComponent(game));
            console.Components.Add(new GameLoopComponent(game));
            console.Components.Add(new GameKeyboardInputComponent(game));

            Global.CurrentScreen = console;
        }
    }
}
