using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace _04_ConsoleGame_SimpleMap
{
    class Snake
    {
        public LinkedList<Point> link;
        public int x =7 ;
        public int y =16 ;
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
        //public void SetDirection(Direction dir)
        //{
        //    if(dir == Direction.None)
        //    {
        //        return;
        //    }
        //    switch (direction)
        //    {
        //        case Direction.Up:
        //            if(dir == Direction.Down)
        //            {
        //                return;
        //            }
        //            break;
        //        case Direction.Down:
        //            if(dir == Direction.Up)
        //            {
        //                return;
        //            }
        //            break;
        //        case Direction.Left:
        //            if(dir == Direction.Right)
        //            {
        //                return;
        //            }
        //            break;
        //        case Direction.Right:
        //            if(dir == Direction.Left)
        //            {
        //                return;
        //            }
        //            break;
        //    }
        //    direction = dir;
        //}

        public void ThreadMethod ()
        {
            while (true)
            {
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
                while (Console.KeyAvailable)
                {
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
                }
            }
            
                
            
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
