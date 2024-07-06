using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGame
{
    internal class Node
    {
        public int x { get; set; }
        public int y { get; set; }
        public int g { get; set; }
        public int h { get; set; }
        public int f { get { return g + h; } }
        public Node parent { get; set; }

        public Node(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
    }
}
