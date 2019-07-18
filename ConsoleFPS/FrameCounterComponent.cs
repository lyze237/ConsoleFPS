using System;
using SadConsole.Components;
using Console = SadConsole.Console;

namespace ConsoleFPS
{
    public class FrameCounterComponent : DrawConsoleComponent
    {
        private int frameCnt;
        
        public override void Draw(Console console, TimeSpan delta)
        {
            console.Print(0, 0, (frameCnt++).ToString());
        }
    }
}