using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGame
{
    internal class World
    {
        private char[,] map;
        private char[,] originalMap;
        private Player player;
        Point playerSpawnPoint;
        private List<Enemy> enemies;
        private readonly int CountOfBeepers;


        public World(string path, Player player, int playerSpawnPointX, int playerSpawnPointY)
        {
            this.map = ReadMap(path); 
            this.originalMap = ReadMap(path);
            this.player = player;
            this.enemies = new List<Enemy>();
            this.CountOfBeepers = CountBeepers();
            this.playerSpawnPoint = new Point(playerSpawnPointX, playerSpawnPointY);
        }
        private char[,] ReadMap(string path)
        {
            string[] file = File.ReadAllLines(path);
            char[,] map = new char[GetMaxLenghtOfLine(file), file.Length];

            for (int x = 0; x < map.GetLength(0); x++)
            {
                for (int y = 0; y < map.GetLength(1); y++)
                {
                    map[x, y] = file[y][x];
                }
            }

            return map;
        }
        private int GetMaxLenghtOfLine(string[] lines)
        {
            int maxLength = lines[0].Length;

            foreach (string line in lines)
                if (line.Length > maxLength)
                    maxLength = line.Length;

            return maxLength;
        }
        public void AddEnemy(Enemy enemy)
        {
            this.enemies.Add(enemy);
        }
        public List<Enemy> GetEnemys()
        {
            return this.enemies;
        }
        public char[,] GetMap()
        {
            return this.map;
        }
        public Player GetPlayer()
        {
            return this.player;
        }
        public void MoveAllEnemy()
        {
            foreach (Enemy enemy in this.enemies)
            {   
                enemy.Move(this);
                if (enemy.GetPosition().x == this.player.GetPosition().x && enemy.GetPosition().y == this.player.GetPosition().y)
                {
                    this.player.KillPLayer();
                }
            }
        }
        public char GetCell(Point position)
        {
            return map[position.x, position.y];
        }
        public void SetCell(Point position, char element)
        {
            map[position.x, position.y] = element;
        }
        public int CountBeepers()
        {   
            int count = 0;

            for (int i = 0; i < map.GetLength(0); i++)
            {
                for (int j = 0; j < map.GetLength(1); j++)
                {
                    if (map[i, j] == '.')
                        count++;    
                }
            }

            return count;
        }
        public int GetCountOfBeepers()
        {
            return CountOfBeepers;
        }
        public Point GetSpawnPosition()
        {
            return this.playerSpawnPoint;
        }
        public void reloadWorld()
        {
            this.player.SetPosition(this.playerSpawnPoint);
            this.map = (char[,])this.originalMap.Clone();
            foreach (Enemy enemy in this.enemies)
            {
                enemy.Reload();
            }
        }
    }
}
