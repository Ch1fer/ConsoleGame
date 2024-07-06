using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Collections;

namespace MyGame
{
    internal class GhostSmart : Enemy
    {
        private int stamina;
        private int maxStamina;
        List<Node> path;
        int pathIdx;


        public GhostSmart(string name, char symbol, ConsoleColor color, int posX, int posY, int stamina) : base(name, symbol, color, new Point(posX, posY))
        {   
            this.stamina = stamina;
            this.maxStamina = stamina;
        }

        public void UpdatePath(World world, AStar aStar)
        {
            Point playerPosition = world.GetPlayer().GetPosition();

            Node currentPos = new Node(this.position.x, this.position.y);
            Node playerPos = new Node(playerPosition.x, playerPosition.y);

            this.path = aStar.FindPath(currentPos, playerPos);
            this.pathIdx = 1;
        }

        public override void Move(World world)
        {
            if (pathIdx <= this.path.Count - 1)
            {
                if (this.stamina < 1)
                {
                    this.stamina = maxStamina;
                    return;
                }

                Point direction = Normalize(position, new Point(this.path[pathIdx].x, this.path[pathIdx].y));
                Point nextPos = new Point(this.position.x + direction.x, this.position.y + direction.y);

                if (world.GetCell(nextPos) != '#')
                {
                    this.lastPosition = this.position;
                    this.position = nextPos;
                    this.pathIdx++;
                    this.stamina--;
                }
            }
        }

        public Point Normalize(Point start, Point target)
        {
            Point vector = new Point(target.x - start.x , target.y - start.y);
            Point normalizedVector = new Point(0, 0);

            if (Math.Abs(vector.x) > Math.Abs(vector.y))
            {
                if (vector.x > 0)
                {
                    normalizedVector.x = 1;
                }
                else if (vector.x < 0)
                {
                    normalizedVector.x = -1;
                }
            }
            else
            {
                if (vector.y > 0)
                {
                    normalizedVector.y = 1;
                }
                else if (vector.y < 0)
                {
                    normalizedVector.y = -1;
                }
            }
            
            return normalizedVector;
        }
    }
    
}
