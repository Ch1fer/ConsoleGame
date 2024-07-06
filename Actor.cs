using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGame
{
    public abstract class Actor
    {
        protected string name;
        protected char symbol;
        protected ConsoleColor color;
        protected Point lastPosition;
        protected Point position;

        public Actor(string name, char symbol, ConsoleColor color, Point position) 
        {
            this.name = name;
            this.symbol = symbol;
            this.color = color;
            this.lastPosition = position;
            this.position = position;
        }

        public Point GetLastPosition()
        {
            return lastPosition;
        }
        public Point GetPosition()
        {
            return position;
        }
        public void SetPosition(Point newPosition)
        {
            if (newPosition.x != this.position.x || newPosition.y != this.position.y)
            {
                this.lastPosition = this.position;
                this.position = newPosition;
            }
        }
        public char GetSymbol()
        {
            return this.symbol;
        }
        public ConsoleColor GetColor()
        {
            return this.color;
        }
    }
}
