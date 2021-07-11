using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace _04_ConsoleGame_SimpleMap
{
    class Snake
    {
        public LinkedList<Point> link;
        public int x =5;
        public int y =32;
        
        //头 linke.First 尾 link.last
        //public Direction direction;
        public int Len { get; private set; }

        public Snake(int x, int y, int len)
        {

            link = new LinkedList<Point>();
            Len = len;
            for(int i = 0; i < len; i++)
            {
                link.AddFirst(new Point(x, y));
            }

            //direction = Direction.Down;
        }

        public void ThreadMove()
        {
            while (true)
            {
                x--;
                Thread.Sleep(500);
            }
           
        }
        public void SetDirection(Direction dir, Player player)
        {
            
            //p_pos.x >= 3 && p_pos.x < width && p_pos.y >= 2 && p_pos.y < height &&
            //if (barrier.Contains(p_pos))
            //{
            //    return;
            //}

            //else
            //{
                switch (dir)
                {
                    case Direction.Up:
                    if (player.x > x)
                    {
                        x++;
                    }
                    else if (player.x < x)
                    {
                        x--;
                    }
                    else if (player.x == x)
                    {
                        if (player.y > y)
                        {
                            y++;
                        }
                        else
                        {
                            y--;
                        }
                    }
                        break;
                    case Direction.Down:
                        //p_pos.
                        x--;
                        break;
                    case Direction.Left:
                        //p_pos.
                        y += 2;
                        break;
                    case Direction.Right:
                        //p_pos.
                        y -= 2;
                        break;
                }
                // }
                //x = p_pos.x;
                //y = p_pos.y;
            //}
        }

        public void ThreadMethod ()
        {
            //while (true)
            //{
                Direction direction = new Direction();
                ConsoleKeyInfo key = new ConsoleKeyInfo();
                key = Console.ReadKey(true);

                switch (key.KeyChar)
                {
                    case 'w':
                        //Tick();
                        direction = Direction.Up;
                        break;
                    case 's':
                        direction = Direction.Down;
                        break;
                    case 'a':
                        direction = Direction.Left;
                        break;
                    case 'd':
                        direction = Direction.Right;
                        break;
                    default:
                        direction = Direction.None;
                        break;
                }
                //while (Console.KeyAvailable)
                //{
                    if (direction == Direction.Up && x > 2)
                    {
                        x--;
                        Thread.Sleep(1000);
                    }
                    else if (direction == Direction.Down && x < 8)
                    {
                        x++;
                        Thread.Sleep(1000);
                    }
                    else if (direction == Direction.Left && y > 2)
                    {
                        y -= 2;
                        Thread.Sleep(1000);
                    }
                    else if (direction == Direction.Right && y < 38)
                    {
                        y += 2;
                        Thread.Sleep(1000);
                    }
                    else
                    {

                    }
               // }
            //}
            
                
            
            //AIMove();
            //LinkedListNode<Point> last = link.Last;
            //Point pos = link.First.Value;//头位置

            //switch (direction)
            //{
            //    case Direction.Up:
            //        last.Value = new Point(pos.x, pos.y - 2);
            //        break;
            //    case Direction.Down:
            //        last.Value = new Point(pos.x, pos.y + 2);
            //        break;
            //    case Direction.Left:
            //        last.Value = new Point(pos.x - 1, pos.y);
            //        break;
            //    case Direction.Right:
            //        last.Value = new Point(pos.x + 1, pos.y);
            //        break;
            //}

            //link.RemoveLast();
            //link.AddFirst(last);
        }
    
        public void AIMove(Snake enemy)
        {
            enemy.x--;
        }
    }
}
