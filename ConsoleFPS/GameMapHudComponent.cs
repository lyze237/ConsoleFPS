using System;
using SadConsole.Components;
using Console = SadConsole.Console;

namespace ConsoleFPS
{
    public class GameMapHudComponent : DrawConsoleComponent
    {
        private GameEntity game;

        public GameMapHudComponent(GameEntity game)
        {
            SortOrder = 1000;
            this.game = game;
        }

        public override void Draw(Console console, TimeSpan delta)
        {
            for (int y = 0; y < game.Map.Length; y++)
            {
                for (int x = 0; x < game.Map[y].Length; x++)
                {
                    {
                        console.Print(x + 1, y + 1, game.Map[y][x].ToString());
                    }
                }
            }

            console.Print((int) game.Player.Position.Y + 1, (int) game.Player.Position.X + 1, "P");
        }
    }
}