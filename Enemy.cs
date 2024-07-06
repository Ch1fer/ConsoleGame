using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGame
{
    abstract class Enemy : Actor
    {
        protected Point direction;
        protected Point spawnPosition;
        public Enemy(string name, char symbol, ConsoleColor color, Point position) : base(name, symbol, color, position)
        {   
            this.direction = GetRandomDirection();
            this.spawnPosition = position;
        }
        
        public Point GetRandomDirection() {
            Random random = new Random();
            int number = random.Next(0, 4);
            
            switch (number)
            {
                case 0:
                    return new Point(-1, 0);
                case 1:
                    return new Point(1, 0);
                case 2:
                    return new Point(0, -1);
                case 3:
                    return new Point(0, 1);
                default:
                    return new Point(-1, 0);
            }
        }
        public abstract void Move(World world);
        public void Reload()
        {
            this.position = this.spawnPosition;
            this.lastPosition = this.spawnPosition;
        }

    }
}
