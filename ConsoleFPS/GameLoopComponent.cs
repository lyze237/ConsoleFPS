using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using SadConsole.Components;
using Console = SadConsole.Console;

namespace ConsoleFPS
{
    public class GameLoopComponent : LogicConsoleComponent
    {
        private GameEntity game;

        public GameLoopComponent(GameEntity game)
        {
            this.game = game;
        }

        public override void Update(Console console, TimeSpan delta)
        {
            var fElapsedTime = delta.TotalSeconds;

            // Handle CCW Rotation
            if (game.Keys.LeftPressed)
                game.Player.Angle -= (float) (game.Player.Speed * 0.75f * fElapsedTime);

            // Handle CW Rotation
            if (game.Keys.RightPressed)
                game.Player.Angle += (float) (game.Player.Speed * 0.75f * fElapsedTime);

            // Handle Forwards movement & collision
            if (game.Keys.ForwardPressed)
            {
                game.Player.Position += new Vector2((float) (Math.Sin(game.Player.Angle) * game.Player.Speed * fElapsedTime), 0);
                game.Player.Position += new Vector2(0, (float) (Math.Cos(game.Player.Angle) * game.Player.Speed * fElapsedTime));
                
                if (game.Map[(int) game.Player.Position.X][(int) game.Player.Position.Y] == '#')
                {
                    game.Player.Position -= new Vector2((float) (Math.Sin(game.Player.Angle) * game.Player.Speed * fElapsedTime), 0);
                    game.Player.Position -= new Vector2(0, (float) (Math.Cos(game.Player.Angle) * game.Player.Speed * fElapsedTime));
                }
            }

            // Handle backwards movement & collision
            if (game.Keys.BackwardPressed)
            {
                game.Player.Position -= new Vector2((float) (Math.Sin(game.Player.Angle) * game.Player.Speed * fElapsedTime), 0);
                game.Player.Position -= new Vector2(0, (float) (Math.Cos(game.Player.Angle) * game.Player.Speed * fElapsedTime));
                if (game.Map[(int) game.Player.Position.X][(int) game.Player.Position.Y] == '#')
                {
                    game.Player.Position += new Vector2((float) (Math.Sin(game.Player.Angle) * game.Player.Speed * fElapsedTime), 0);
                    game.Player.Position += new Vector2(0, (float) (Math.Cos(game.Player.Angle) * game.Player.Speed * fElapsedTime));
                }
            }

            // Handle Strafe Right movement & collision
            if (game.Keys.StrafeRightPressed)
            {
                game.Player.Position += new Vector2((float) (Math.Cos(game.Player.Angle) * game.Player.Speed * fElapsedTime), 0);
                game.Player.Position -= new Vector2(0, (float) (Math.Sin(game.Player.Angle) * game.Player.Speed * fElapsedTime));
                if (game.Map[(int) game.Player.Position.X][(int) game.Player.Position.Y] == '#')
                {
                    game.Player.Position -= new Vector2((float) (Math.Cos(game.Player.Angle) * game.Player.Speed * fElapsedTime), 0);
                    game.Player.Position += new Vector2(0, (float) (Math.Sin(game.Player.Angle) * game.Player.Speed * fElapsedTime));
                }
            }

            // Handle Strafe Right movement & collision
            if (game.Keys.StrafeLeftPressed)
            {
                game.Player.Position -= new Vector2((float) (Math.Cos(game.Player.Angle) * game.Player.Speed * fElapsedTime), 0);
                game.Player.Position += new Vector2(0, (float) (Math.Sin(game.Player.Angle) * game.Player.Speed * fElapsedTime));
                if (game.Map[(int) game.Player.Position.X][(int) game.Player.Position.Y] == '#')
                {
                    game.Player.Position += new Vector2((float) (Math.Cos(game.Player.Angle) * game.Player.Speed * fElapsedTime), 0);
                    game.Player.Position -= new Vector2(0, (float) (Math.Sin(game.Player.Angle) * game.Player.Speed * fElapsedTime));
                }
            }
        }

        public override void Draw(Console console, TimeSpan delta)
        {
            for (var x = 0; x < console.Width; x++)
            {
                // For each column, calculate the projected ray angle into world space
                var rayAngle = game.Player.Angle - game.Player.FOV / 2.0f + x / (float) console.Width * game.Player.FOV;

                // Find distance to wall
                var stepSize = 0.1f; // Increment size for ray casting, decrease to increase										
                var distanceToWall = 0.0f; //                                      resolution

                var wallHit = false; // Set when ray hits wall block
                var boundary = false; // Set when ray hits boundary between two wall blocks

                var eyeX = (float) Math.Sin(rayAngle); // Unit vector for ray in player space
                var eyeY = (float) Math.Cos(rayAngle);

                // Incrementally cast ray from player, along ray angle, testing for 
                // intersection with a block
                while (!wallHit && distanceToWall < game.RenderDepth)
                {
                    distanceToWall += stepSize;
                    var testX = (int) (game.Player.Position.X + eyeX * distanceToWall);
                    var testY = (int) (game.Player.Position.Y + eyeY * distanceToWall);

                    // Test if ray is out of bounds
                    if (testX < 0 || testX >= game.Map[0].Length || testY < 0 || testY >= game.Map[0].Length)
                    {
                        wallHit = true; // Just set distance to maximum depth
                        distanceToWall = game.RenderDepth;
                    }
                    else
                    {
                        // Ray is inbounds so test to see if the ray cell is a wall block
                        if (game.Map[testX][testY] == '#')
                        {
                            // Ray has hit wall
                            wallHit = true;

                            // To highlight tile boundaries, cast a ray from each corner
                            // of the tile, to the player. The more coincident this ray
                            // is to the rendering ray, the closer we are to a tile 
                            // boundary, which we'll shade to add detail to the walls
                            var p = new List<Vector2>();

                            // Test each corner of hit tile, storing the distance from
                            // the player, and the calculated dot product of the two rays
                            for (var tx = 0; tx < 2; tx++)
                            {
                                for (var ty = 0; ty < 2; ty++)
                                {
                                    // Angle of corner to eye
                                    var vx = (float) testX + tx - game.Player.Position.X;
                                    var vy = (float) testY + ty - game.Player.Position.Y;
                                    var d = (float) Math.Sqrt(vx * vx + vy * vy);
                                    var dot = eyeX * vx / d + eyeY * vy / d;
                                    p.Add(new Vector2(d, dot));
                                }
                            }

                            // Sort Pairs from closest to farthest
                            p = p.OrderBy(tuple => tuple.X).ToList();

                            // First two/three are closest (we will never see all four)
                            var fBound = 0.01f;
                            if (Math.Acos(p[0].Y) < fBound) boundary = true;
                            if (Math.Acos(p[1].Y) < fBound) boundary = true;
                            if (Math.Acos(p[2].Y) < fBound) boundary = true;
                        }
                    }
                }

                // Calculate distance to ceiling and floor
                var nCeiling = (int) ((float) (console.Height / 2.0) - console.Height / distanceToWall);
                var nFloor = console.Height - nCeiling;

                // Shader walls based on distance
                char nShade;
                if (distanceToWall <= game.RenderDepth / 4.0f) nShade = (char) 219; // '█'; // Very close	
                else if (distanceToWall < game.RenderDepth / 3.0f) nShade = (char) 178; //'▓';
                else if (distanceToWall < game.RenderDepth / 2.0f) nShade = (char) 177; // '▒';
                else if (distanceToWall < game.RenderDepth) nShade = (char) 176; // '░';
                else nShade = ' '; // Too far away

                if (boundary) nShade = ' '; // Black it out

                for (var y = 0; y < console.Height; y++)
                {
                    // Each Row
                    if (y <= nCeiling)
                        console.Print(x, y, " ");
                    else if (y > nCeiling && y <= nFloor)
                        console.Print(x, y, nShade.ToString());
                    else // Floor
                    {
                        // Shade floor based on distance
                        var b = 1.0f - (y - console.Height / 2.0f) / (console.Height / 2.0f);
                        if (b < 0.25) nShade = '#';
                        else if (b < 0.5) nShade = 'x';
                        else if (b < 0.75) nShade = '.';
                        else if (b < 0.9) nShade = '-';
                        else nShade = ' ';
                        console.Print(x, y, nShade.ToString());
                    }
                }
            }
        }
    }
}