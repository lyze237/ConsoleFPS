using System;
using SadConsole.Components;
using Console = SadConsole.Console;

namespace ConsoleFPS
{
    public class GameStatusHudComponent : DrawConsoleComponent
    {
        private GameEntity game;
        private long frame;

        public GameStatusHudComponent(GameEntity game)
        {
            this.game = game;
            SortOrder = 1000;
        }
        
        public override void Draw(Console console, TimeSpan delta)
        {
            var fElapsedTime = delta.TotalSeconds;
            
            var status = $"X={game.Player.Position.X:000.00}, Y={game.Player.Position.Y:000.00}, A={game.Player.Angle:000.00} FPS={1.0f / fElapsedTime:000.00} Frame={frame++}";
            console.Print(0, 0, status);
        }
    }
}