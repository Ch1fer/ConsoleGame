using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGame
{
    internal class Player : Actor
    {
        private int score;
        private bool isAlive = true;

        public Player(string name, ConsoleColor color, char symbol) 
            : base(name, symbol, color, new Point(1, 1))
        {
            this.score = 0;
        }

        public int GetScore() 
        {
            return this.score;
        }
        public void AddScore(int value)
        {
            this.score += value;
        }
        public void KillPLayer()
        {
            isAlive = false;
        }
        public void RevivePlayer()
        {
            this.isAlive = true;
        }
        public bool IsAlive()
        {
            return isAlive;
        }
        public void ResetScore()
        {
            this.score = 0;
        }
    }
}
