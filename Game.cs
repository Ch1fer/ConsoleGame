using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace MyGame
{
    internal class Game
    {
        private Player player;
        private List<(World, bool)> worlds;
        private WorldRender render;
        private InputHandler handler;
        private bool isRun = true;
        private AStar aStar;
        Point startRender;

        public Game(Player player,  int startRenderX, int startRenderY)
        {
            this.player = player;
            this.startRender = new Point(startRenderX, startRenderY);
            this.worlds = new List<(World, bool)>();
        }

        public void Run()
        {
            int menuOption = 0;
            int optionCoutn = 3;

            DrawMainMenu();
            DrawCursor(menuOption);

            ConsoleKeyInfo pressedKey = new ConsoleKeyInfo('\0', ConsoleKey.NoName, false, false, false);

            while (true)
            {   
                
                if (pressedKey.Key == ConsoleKey.W)
                {
                    if (menuOption <= 0)
                    {
                        menuOption = optionCoutn - 1;
                    }
                    else
                    {
                        menuOption--;
                    }
                }
                else if(pressedKey.Key == ConsoleKey.S)
                {
                    menuOption = (menuOption + 1) % optionCoutn;
                }

                if (pressedKey.Key == ConsoleKey.W || pressedKey.Key == ConsoleKey.S)
                {
                    DrawMainMenu();
                    DrawCursor(menuOption);
                }

                if (pressedKey.Key == ConsoleKey.Enter && menuOption == 0)
                {
                                      

                    pressedKey = new ConsoleKeyInfo('\0', ConsoleKey.NoName, false, false, false);
                    int levelOption = 0;

                    DrawLevelMenu();
                    DrawLevelCursor(levelOption);

                    while (pressedKey.Key != ConsoleKey.Escape)
                    {
                        pressedKey = Console.ReadKey(true);
                        

                        if (pressedKey.Key == ConsoleKey.W)
                        {
                            levelOption = (levelOption + 4) % 8;
                        }
                        else if(pressedKey.Key == ConsoleKey.S) 
                        {
                            levelOption = (levelOption + 4) % 8;
                        }
                        else if(pressedKey.Key == ConsoleKey.D)
                        {
                            levelOption = (levelOption + 1) % 8;
                        }
                        else if(pressedKey.Key == ConsoleKey.A)
                        {
                            levelOption = (levelOption - 1);
                            if (levelOption < 0)
                                levelOption = 7;
                        }
                        else if(pressedKey.Key == ConsoleKey.Enter){
                            worlds[levelOption].Item1.reloadWorld();
                            StartWorld(worlds[levelOption].Item1);  
                        }

                        DrawLevelMenu();
                        DrawLevelCursor(levelOption);
                    }
                }
                else if (pressedKey.Key == ConsoleKey.Enter && menuOption == 2)
                {
                    Console.Clear();
                    break;
                }

                DrawMainMenu();
                DrawCursor(menuOption);
                pressedKey = Console.ReadKey(true);

            }
        }
        public void StartWorld(World world)
        {   

            isRun = true;
            this.player.RevivePlayer();
            this.player.ResetScore();
            this.player.SetPosition(world.GetSpawnPosition());

            aStar = new AStar(world.GetMap());
            this.render = new WorldRender(world, startRender.x, startRender.y);
            this.handler = new InputHandler(this.player, world);

            world.GetEnemys().Where(o => o is GhostSmart)
                .Cast<GhostSmart>().ToList()
                .ForEach(o => o.UpdatePath(world, aStar));

            StartInputHandler();
            StartUpdateEnemyPath(world);
            StartGameLoop(world);
        }
        public void StartInputHandler()
        {
            Task.Run(() =>
            {
                while (isRun)
                {
                    handler.UpdateKey(Console.ReadKey(true));
                }

            });
        }
        public void StartGameLoop(World world)
        {
            render.DrawMap();
            while (isRun)
            {
                
                handler.UpdatePlayerPosition();
                world.MoveAllEnemy();
                render.DrawScreen();

                if (this.player.GetScore() >= world.GetCountOfBeepers())
                {
                    isRun = false;
                    markAsCompleted(world);
                    ShowVictoryScreen();
                    Thread.Sleep(500);
                    Console.SetCursorPosition(50, 17);
                    Console.Write("Press any button...");
                    Console.ReadKey();
                }

                else if(!this.player.IsAlive())
                {
                    isRun = false;
                    ShowGameOverScreen();
                    Thread.Sleep(500);
                    Console.SetCursorPosition(50, 17);
                    Console.Write("Press any button...");
                    Console.ReadKey();
                }
               
                Thread.Sleep(30);
            }

        }
        public void StartUpdateEnemyPath(World world)
        {
            new Thread(() =>
            {
                while (isRun)
                { 
                    List<Enemy> enemys = world.GetEnemys();
                    foreach (Enemy enemy in enemys)
                    {
                        if (enemy is GhostSmart ghost)
                        {
                            ghost.UpdatePath(world, aStar);

                        }
                    }
                }
            })
            { Priority = ThreadPriority.Highest }.Start();
        }
        public void AddWorld(World world)
        {
            this.worlds.Add((world, false));
        }
        public void DrawMainMenu()
        {
            Console.CursorVisible = false;
            Console.ForegroundColor = ConsoleColor.White;

            string padding = "                                   ";
            Console.Clear();
            Console.SetCursorPosition(0, 7);
            Console.WriteLine(padding + "        ______   __         ______     __  __               ");
            Console.WriteLine(padding + "       /\\  == \\ /\\ \\       /\\  __ \\   /\\ \\_\\ \\              ");
            Console.WriteLine(padding + "       \\ \\  _-/ \\ \\ \\____  \\ \\  __ \\  \\ \\____ \\             ");
            Console.WriteLine(padding + "        \\ \\_\\    \\ \\_____\\  \\ \\_\\ \\_\\  \\/\\_____\\            ");
            Console.WriteLine(padding + "         \\/_/     \\/_____/   \\/_/\\/_/   \\/_____/            ");
            Console.WriteLine();


            Console.WriteLine(padding + " ______     ______     ______     __  __     ______  ");
            Console.WriteLine(padding + "/\\  __ \\   /\\  == \\   /\\  __ \\   /\\ \\/\\ \\   /\\__  _\\ ");
            Console.WriteLine(padding + "\\ \\  __ \\  \\ \\  __<   \\ \\ \\/\\ \\  \\ \\ \\_\\ \\  \\/_/\\ \\/ ");
            Console.WriteLine(padding + " \\ \\_\\ \\_\\  \\ \\_____\\  \\ \\_____\\  \\ \\_____\\    \\ \\_\\ ");
            Console.WriteLine(padding + "  \\/_/\\/_/   \\/_____/   \\/_____/   \\/_____/     \\/_/ ");
            Console.WriteLine();

            Console.WriteLine(padding + "        ______     __  __     __     ______                 ");
            Console.WriteLine(padding + "       /\\  ___\\   /\\_\\_\\_\\   /\\ \\   /\\__  _\\                ");
            Console.WriteLine(padding + "       \\ \\  __\\   \\/_/\\_\\/_  \\ \\ \\  \\/_/\\ \\/                ");
            Console.WriteLine(padding + "        \\ \\_____\\   /\\_\\/\\_\\  \\ \\_\\    \\ \\_\\                ");
            Console.WriteLine(padding + "         \\/_____/   \\/_/\\/_/   \\/_/     \\/_/                ");



        }
        public void DrawCursor(int options)
        {
            Console.CursorVisible = false;
            Console.ForegroundColor = ConsoleColor.Red;
            Console.SetCursorPosition(30, 6 + options * 6);
            Console.Write("==========================================================");
            Console.SetCursorPosition(30, 7 + options * 6);
            Console.Write("\\\\");
            Console.SetCursorPosition(31, 8 + options * 6);
            Console.Write("\\\\");
            Console.SetCursorPosition(32, 9 + options * 6);
            Console.Write("\\\\");
            Console.SetCursorPosition(33, 10 + options * 6);
            Console.Write("\\\\");
            Console.SetCursorPosition(34, 11 + options * 6);
            Console.Write("\\\\");

            Console.SetCursorPosition(87, 7 + options * 6);
            Console.Write("\\\\");
            Console.SetCursorPosition(88, 8 + options * 6);
            Console.Write("\\\\");
            Console.SetCursorPosition(89, 9 + options * 6);
            Console.Write("\\\\");
            Console.SetCursorPosition(90, 10 + options * 6);
            Console.Write("\\\\");
            Console.SetCursorPosition(91, 11 + options * 6);
            Console.Write("\\\\");

            Console.SetCursorPosition(35, 12 + options * 6);
            Console.Write("==========================================================");

            Console.ForegroundColor = ConsoleColor.White;
        }
        public void DrawLevelMenu()
        {   
            Console.Clear();


            for (int i = 0; i < worlds.Count; i++)
            {
                Console.ForegroundColor = ConsoleColor.White;
                int x = i % 4;
                int y = i / 4;

                if (this.worlds[i].Item2)
                    Console.ForegroundColor = ConsoleColor.Yellow;

                DrawNumber(i, 36 + 13 * x, 8 + 7 * y);
            }
        }
        public void DrawNumber(int number, int x, int y)
        {
            List<string[]> numbers = new List<string[]>
            {
                new string[]
                {
                   " ____     ",
                   "/\\__ \\    ",
                   "\\/_/\\ \\__ ",
                   "  /\\_____\\",
                   "  \\/_____/"
                },
                new string[]
                {
                   " ______   ",
                   "/\\___  \\  ",
                   "\\/\\  ___\\ ",
                   " \\ \\_____\\",
                   "  \\/_____/"
                },
                new string[]
                {
                   " ______   ",
                   "/\\___  \\  ",
                   "\\/\\___  \\ ",
                   " \\/\\_____\\",
                   "  \\/_____/"
                },
                new string[]
                {
                   " __  __   ",
                   "/\\ \\_\\ \\  ",
                   "\\ \\____ \\",
                   " \\/___/\\ \\",
                   "      \\/_/"
                },
                new string[]
                {
                   " ______   ",
                   "/\\  ___\\  ",
                   "\\ \\___  \\ ",
                   " \\/\\_____\\",
                   "  \\/_____/"
                },
                new string[]
                {
                   " ______   ",
                   "/\\  ___\\  ",
                   "\\ \\  __ \\ ",
                   " \\ \\_____\\",
                   "  \\/_____/"
                },
                new string[]
                {
                   " ______   ",
                   "/\\_____\\  ",
                   "\\/___/\\ \\ ",
                   "     \\ \\_\\",
                   "      \\/_/"
                },
                new string[]
                {
                    " ______  ",
                    "/\\  __ \\",
                    "\\ \\  __ \\",
                    " \\ \\_____\\",
                    "  \\/_____/" 
                },
            };

            for (int i = 0; i < 5; i++)
            {
                Console.SetCursorPosition(x, y + i);
                Console.Write(numbers[number][i]);
            }
        }
        public void DrawLevelCursor(int option)
        {
            int x = 30 + 13 * (option % 4);
            int y = 7 + 7 *  (option / 4);

            Console.ForegroundColor = ConsoleColor.Red;
            
            Console.SetCursorPosition(x + 1, y);
            Console.Write("==============");
            Console.SetCursorPosition(x + 1, y + 1);
            Console.Write("\\\\");
            Console.SetCursorPosition(x + 2, y + 2);
            Console.Write("\\\\");
            Console.SetCursorPosition(x + 3, y + 3);
            Console.Write("\\\\");
            Console.SetCursorPosition(x + 4, y + 4);
            Console.Write("\\\\");
            Console.SetCursorPosition(x + 5, y + 5);
            Console.Write("\\\\");
            Console.SetCursorPosition(x + 6, y + 6);
            Console.Write("==============");
            
            Console.SetCursorPosition(14 + x, y + 1);
            Console.Write("\\\\");
            Console.SetCursorPosition(15 + x, y + 2);
            Console.Write("\\\\");
            Console.SetCursorPosition(16 + x, y + 3);
            Console.Write("\\\\");
            Console.SetCursorPosition(17 + x, y + 4);
            Console.Write("\\\\");
            Console.SetCursorPosition(18 + x, y + 5);
            Console.Write("\\\\");
        }
        public void markAsCompleted(World world)
        {   
            int idx = 0;
            
            for (int i = 0; i < worlds.Count; i++)
            {
                if (worlds[i].Item1 == world)
                {
                    idx = i;
                }
            }
            worlds[idx] = (world, true);
        }
        public void ShowVictoryScreen()
        {

            int x = 0;
            int y = 8;

            int xText = 25;
            int yText = 11;

            for (int i = 0; i < 12; i++)
            {
                if (i == 0 || i == 11)
                {
                    Console.BackgroundColor = ConsoleColor.DarkYellow;
                }
                else
                {
                    Console.BackgroundColor = ConsoleColor.Black;
                }

                Console.SetCursorPosition(x, y + i);
                Console.Write("                                                                                                                        ");
            }
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.DarkYellow;

            Console.SetCursorPosition(xText, yText);
            Console.Write(" __   __   __     ______     ______   ______     ______     __  __    ");
            Console.SetCursorPosition(xText, yText + 1);
            Console.Write("/\\ \\ / /  /\\ \\   /\\  ___\\   /\\__  _\\ /\\  __ \\   /\\  == \\   /\\ \\_\\ \\   ");
            Console.SetCursorPosition(xText, yText + 2);
            Console.Write("\\ \\ \\'/   \\ \\ \\  \\ \\ \\____  \\/_/\\ \\/ \\ \\ \\/\\ \\  \\ \\  __<   \\ \\____ \\  ");
            Console.SetCursorPosition(xText, yText + 3);
            Console.Write(" \\ \\__|    \\ \\_\\  \\ \\_____\\    \\ \\_\\  \\ \\_____\\  \\ \\_\\ \\_\\  \\/\\_____\\ ");
            Console.SetCursorPosition(xText, yText + 4);
            Console.Write("  \\/_/      \\/_/   \\/_____/     \\/_/   \\/_____/   \\/_/ /_/   \\/_____/ ");

            Console.ForegroundColor = ConsoleColor.Yellow;
        }
        public void ShowGameOverScreen()
        {
            int x = 0;
            int y = 8;

            int xText = 15;
            int yText = 11;

            for (int i = 0; i < 12; i++)
            {
                if (i == 0 || i == 11)
                {
                    Console.BackgroundColor = ConsoleColor.Red;
                }
                else
                {
                    Console.BackgroundColor = ConsoleColor.Black;
                }

                Console.SetCursorPosition(x, y + i);
                Console.Write("                                                                                                                        ");
            }
            Console.BackgroundColor = ConsoleColor.Black;

            Console.SetCursorPosition(xText, yText);
            Console.Write(" ______     ______     __    __     ______        ______     __   __   ______     ______    ");
            Console.SetCursorPosition(xText, yText + 1);
            Console.Write("/\\  ___\\   /\\  __ \\   /\\ \"-./  \\   /\\  ___\\      /\\  __ \\   /\\ \\ / /  /\\  ___\\   /\\  == \\   ");
            Console.SetCursorPosition(xText, yText + 2);
            Console.Write("\\ \\ \\__ \\  \\ \\  __ \\  \\ \\ \\-./\\ \\  \\ \\  __\\      \\ \\ \\/\\ \\  \\ \\ \\'/   \\ \\  __\\   \\ \\  __<   ");
            Console.SetCursorPosition(xText, yText + 3);
            Console.Write(" \\ \\_____\\  \\ \\_\\ \\_\\  \\ \\_\\ \\ \\_\\  \\ \\_____\\     \\ \\_____\\  \\ \\__|    \\ \\_____\\  \\ \\_\\ \\_\\ ");
            Console.SetCursorPosition(xText, yText + 4);
            Console.Write("  \\/_____/   \\/_/\\/_/   \\/_/  \\/_/   \\/_____/      \\/_____/   \\/_/      \\/_____/   \\/_/ /_/ ");

            Console.ForegroundColor = ConsoleColor.Yellow;
        }
    }
}
