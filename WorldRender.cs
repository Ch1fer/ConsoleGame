using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Formats.Asn1.AsnWriter;


namespace MyGame
{
    internal class WorldRender
    {
        private World world;
        private Point startPoint;

        public WorldRender(World word, int startPointX = 0, int startPointY = 0) 
        {
            this.world = word;
            this.startPoint = new Point(startPointX, startPointY);
            Console.CursorVisible = false;
        }
        public void DrawMap()
        {
            char[,] map = this.world.GetMap();

            ConsoleColor consoleColor = Console.BackgroundColor;
            ConsoleColor foregroundColor = Console.ForegroundColor;
            Console.SetCursorPosition(this.startPoint.x, this.startPoint.y);
            

            for (int y = 0; y < map.GetLength(1); y++)
            {

                for (int x = 0; x < map.GetLength(0); x++)
                {
                    Console.BackgroundColor = consoleColor;
                    Console.ForegroundColor = foregroundColor;
                    

                    if (map[x, y] == '#')
                    {
                        Console.BackgroundColor = ConsoleColor.Blue;
                        Console.Write(' ');
                    }
                    else if (map[x, y] == '.')
                    {
                        Console.BackgroundColor = consoleColor;
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.Write('.');
                    }
                    else
                    {
                        Console.Write(' ');
                    }

                }

                Console.BackgroundColor = consoleColor;
                Console.WriteLine();
                for (int i = 0; i < this.startPoint.x; i++)
                {
                    Console.Write(' ');
                }
            }
        }
        public void DrawScreen()
        {
            ConsoleColor originalColor = Console.ForegroundColor;
            ConsoleColor originalBackground = Console.BackgroundColor;


            Player player = this.world.GetPlayer();
            
            this.DrawActor(player);
            
            List<Enemy> enemys = world.GetEnemys();
            foreach (Enemy enemy in enemys)
            {   
                this.DrawActor(enemy);
            }

            Console.SetCursorPosition(55, 28);
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write($"score: {player.GetScore()}");

            Console.SetCursorPosition(startPoint.x, startPoint.y);
            Console.ForegroundColor = originalColor;
            Console.BackgroundColor = originalBackground;
        }
        public void DrawActor(Actor actor)
        {   
            Point lastPos = actor.GetLastPosition();
            Point newPos = actor.GetPosition();

            Console.SetCursorPosition(this.startPoint.x + lastPos.x, this.startPoint.y + lastPos.y);
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write(this.world.GetCell(lastPos));

            Console.SetCursorPosition(this.startPoint.x + newPos.x, this.startPoint.y + newPos.y);
            Console.ForegroundColor = actor.GetColor();
            Console.Write($"{actor.GetSymbol()}");
        }

    }   
}
