using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGame
{
    internal class AStar
    {
        private static readonly int[,] Directions = new int[,]
        {
            {0, 1}, {1, 0}, {0, -1}, {-1, 0}
        };

        private char[,] map;
        private int width;
        private int height;

        public AStar(char[,] map)
        {
            this.map = map;
            this.width = map.GetLength(0);
            this.height = map.GetLength(1);
        }

        public List<Node> FindPath(Node start, Node goal)
        {
            var openList = new List<Node> { start };
            var closedList = new HashSet<Node>();

            start.g = 0;
            start.h = Math.Abs(start.x - goal.x) + Math.Abs(start.y - goal.y);

            while (openList.Count > 0)
            {
                openList.Sort((node1, node2) => node1.f.CompareTo(node2.f));
                var currentNode = openList[0];

                if (currentNode.x == goal.x && currentNode.y == goal.y)
                {
                    return ReconstructPath(currentNode);
                }

                openList.Remove(currentNode);
                closedList.Add(currentNode);

                for (int i = 0; i < Directions.GetLength(0); i++)
                {
                    var neighborX = currentNode.x + Directions[i, 0];
                    var neighborY = currentNode.y + Directions[i, 1];

                    if (neighborX < 0 || neighborX >= width || neighborY < 0 || neighborY >= height) // перевірка на границі карти
                        continue;

                    if (map[neighborX, neighborY] == '#') //перевірка на стіну
                        continue;

                    var neighbor = new Node(neighborX, neighborY)
                    {
                        parent = currentNode,
                        g = currentNode.g + 1,
                        h = Math.Abs(neighborX - goal.x) + Math.Abs(neighborY - goal.y)
                    };

                    if (closedList.Contains(neighbor))
                        continue;

                    var existingNode = openList.Find(node => node.x == neighborX && node.y == neighborY);
                    if (existingNode == null)
                    {
                        openList.Add(neighbor);
                    }
                    else if (neighbor.g < existingNode.g)
                    {
                        existingNode.parent = currentNode;
                        existingNode.g = neighbor.g;
                    }
                }
            }


            return null;
        }

        private List<Node> ReconstructPath(Node currentNode)
        {
            var path = new List<Node>();
            while (currentNode != null)
            {
                path.Add(currentNode);
                currentNode = currentNode.parent;
            }
            path.Reverse();
            return path;
        }
    }
}
