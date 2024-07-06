using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace MyGame
{
    internal class GhostChanger: Enemy
    {
        int counterForChange;
        public GhostChanger(string name, char symbol, ConsoleColor color, int posX, int posY) : base(name, symbol, color, new Point(posX, posY))
        {
            this.counterForChange = 20;
        }

        public override void Move(World world)
        {
            counterForChange--;
            Point nextPos = new Point(this.position.x + this.direction.x, this.position.y + this.direction.y);
            char nextCell = world.GetCell(nextPos);

            if (nextCell == ' ' || nextCell == '.')
            {
                this.lastPosition = this.position;
                this.position = nextPos;
            }
            else
            {
                this.direction = this.GetRandomDirection();
            }

            if (this.counterForChange <= 0)
            {
                this.direction = this.GetRandomDirection();
                this.counterForChange = 20;
            }
        }
    }
}
