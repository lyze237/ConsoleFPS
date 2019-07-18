using System;
using Microsoft.Xna.Framework.Input;
using SadConsole.Components;
using Console = SadConsole.Console;
using Keyboard = SadConsole.Input.Keyboard;

namespace ConsoleFPS
{
    public class GameKeyboardInputComponent : KeyboardConsoleComponent
    {
        private GameEntity game;

        public GameKeyboardInputComponent(GameEntity game)
        {
            this.game = game;
        }

        public override void ProcessKeyboard(Console console, Keyboard info, out bool handled)
        {
            game.Keys.ForwardPressed = info.IsKeyDown(Keys.W);
            game.Keys.BackwardPressed = info.IsKeyDown(Keys.S);
            
            game.Keys.LeftPressed = info.IsKeyDown(Keys.A);
            game.Keys.RightPressed = info.IsKeyDown(Keys.D);
            
            game.Keys.StrafeLeftPressed = info.IsKeyDown(Keys.Q);
            game.Keys.StrafeRightPressed = info.IsKeyDown(Keys.E);

            if (info.IsKeyDown(Keys.Space))
            {
                console.Print(1, 1, "SPACE");
            }
            handled = true;
        }
    }
}