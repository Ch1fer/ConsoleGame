using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGame
{
    internal class GhostRandom : Enemy
    {
        public GhostRandom(string name, char symbol, ConsoleColor color, int posX, int posY) : base(name, symbol, color, new Point(posX, posY))
        {

        }

        public override void Move(World world)
        {
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
        }
    }
}
