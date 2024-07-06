using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Formats.Asn1.AsnWriter;

namespace MyGame
{
    internal class InputHandler
    {
        private ConsoleKeyInfo currentPressedKey;
        private ConsoleKeyInfo lastPressedKey;
        private Player player;
        private World world;

        public InputHandler(Player player, World world)
        {
            this.player = player;
            this.world = world;
            this.currentPressedKey = new ConsoleKeyInfo('o', ConsoleKey.O, false, false, false);
            this.lastPressedKey = new ConsoleKeyInfo('o', ConsoleKey.O, false, false, false);
        }

        private Point GetDirection(ConsoleKeyInfo pressedKey)
        {
            Point direction = new Point(0, 0);

            if (pressedKey.Key == ConsoleKey.A)
            {
                direction.x = -1;
            }
            else if (pressedKey.Key == ConsoleKey.D)
            {
                direction.x = +1;
            }
            else if (pressedKey.Key == ConsoleKey.W)
            {
                direction.y = -1;
            }
            else if (pressedKey.Key == ConsoleKey.S)
            {
                direction.y = +1;
            }

            return direction;
        }

        public void UpdatePlayerPosition()
        {
            Point playerPos = player.GetPosition();
            Point lastPlayerPos = player.GetLastPosition();

            
            Point newDir = this.GetDirection(currentPressedKey);
            Point nextPosition = new(playerPos.x + newDir.x, playerPos.y + newDir.y);
            char nextCell = world.GetCell(nextPosition);

            Point lastDir = this.GetDirection(lastPressedKey);
            Point lastPosition = new(playerPos.x + lastDir.x, playerPos.y + lastDir.y);
            char oldCell = world.GetCell(lastPosition);

            if (nextCell == ' ' || nextCell == '.')
            {
                player.SetPosition(nextPosition);

                this.lastPressedKey = currentPressedKey;

                if (nextCell == '.')
                {
                    this.player.AddScore(1);
                    this.world.SetCell(nextPosition, ' ');
                }
            }
            else if (oldCell == ' ' || oldCell == '.')
            {
                player.SetPosition(lastPosition);

                if (oldCell == '.')
                {
                    this.player.AddScore(1);
                    this.world.SetCell(lastPosition, ' ');
                }
            }

            List<Enemy> enemyList = this.world.GetEnemys();

            foreach (Enemy enemy in enemyList)
            {
                if (enemy.GetPosition().x == this.player.GetPosition().x && enemy.GetPosition().y == this.player.GetPosition().y)
                {
                    this.player.KillPLayer();
                }
            }
        }

        public void UpdateKey(ConsoleKeyInfo pressedKey)
        {
            if (pressedKey != this.currentPressedKey && (pressedKey.Key == ConsoleKey.A || pressedKey.Key == ConsoleKey.D
                || pressedKey.Key == ConsoleKey.W || pressedKey.Key == ConsoleKey.S))
            {
                this.lastPressedKey = this.currentPressedKey;
                this.currentPressedKey = pressedKey;    
            }
        }
    }
}
