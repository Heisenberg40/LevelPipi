using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.Threading;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Cryptography.X509Certificates;

namespace _04_ConsoleGame_SimpleMap
{
    //剩余问题：
    //1.棋子在移动到边界时会遇到index out of range异常，敌人寻路法则优化
    //2.添加道具，吃了后可以反杀敌人，获得100分。吃了后，敌我棋子颜色都要变化。同时敌人的寻路规则也要修改，改为躲避玩家
    //3.添加关卡2新地图，会有2个敌人……依次类推
    //4.剩余命数和计分板的界面优化（低优先级）
    enum Direction
    {
        None,
        Up,
        Down,
        Left,
        Right,
    };

    public class Player
    {
        public int x = 4;
        public int y = 3;
        public int score = 0;
    }

    struct Point
    {
        public int x;
        public int y;
        public Point(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
    }


    class Program
    {
        static Direction direction;
        static int width = 40;
        static int height = 8;
        static List<Point> points = new List<Point>();//用于存豆子位置的
        static List<Point> barrier = new List<Point>();//用于存障碍位置的
        static Player p = new Player();
        static Snake snake;
        static char[,] MapArray = new char[10, 43];
        static int life = 3;
        //static Enemy e1 = new Enemy(3,36);



        static void ReadBuffer()
        {
            StreamReader reader = new StreamReader("..\\..\\..\\Level1.txt");
            int count = 0;
            while (count < 10)//count代表行数，最多10行
            {
                string s = reader.ReadLine();

                char[] line1 = s.ToCharArray();

                for (int i = 0; i < MapArray.GetLength(1); i++)
                {
                    MapArray[count, i] = line1[i];
                }
                count++;
            }
            reader.Close();
        }
        static void DrawBuffer()
        {
            Console.Clear();
            //存角色
            LinkedListNode<Point> node = snake.link.First;

            while (node != null && !barrier.Contains(node.Value) && node.Next != null)
            {
                Point v = node.Value;
                MapArray[v.y + 1, v.x + 1] = '@';

                node = node.Next;
            }
            //画地图
            for (int i = 0; i < MapArray.GetLength(0); i++)
            {
                for (int j = 0; j < MapArray.GetLength(1); j++)
                {
                    if (MapArray[i, j] == '#')
                    {
                        Console.ForegroundColor = ConsoleColor.DarkBlue;
                        Console.Write('■');
                    }
                    else if (MapArray[i, j] == 'o')
                    {
                        Point bean = new Point(i, j);
                        if (points.Contains(bean))
                        {
                            Console.ForegroundColor = ConsoleColor.DarkYellow;
                            Console.Write('●');
                        }
                        else//如果豆子已经被吃了，显示为空
                        {
                            Console.Write("  ");
                        }
                    }
                    else if (MapArray[i, j] == '@')
                    {
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.Write('★');
                    }
                    else if (MapArray[i, j] == 'E')
                    {
                        Console.ForegroundColor = ConsoleColor.DarkMagenta;
                        Console.Write('▲');
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Gray;
                        Console.Write(MapArray[i, j]);
                    }
                }
                Console.WriteLine();
            }
        }
        static bool HitBarrier()
        {
            return true;
        }

        static void DrawPlayerNew()
        {
            MapArray[p.y, p.x] = '@';
            MapArray[snake.x, snake.y] = 'E';
        }

        static void SaveObjects()//把txt文件中的信息读取到MapArray中，并且取出豆子和障碍物的位置信息存入相关List中
        {
            ReadBuffer();

            for (int i = 0; i < MapArray.GetLength(0); i++)
            {
                for (int j = 0; j < MapArray.GetLength(1); j++)
                {
                    if (MapArray[i, j] == 'o')
                    {
                        points.Add(new Point(i, j));
                    }
                    else if (MapArray[i, j] == '#')
                    {
                        barrier.Add(new Point(j, i));
                    }
                }
            }
        }

        static Direction Input(char keyChar)
        {
            switch (keyChar)
            {
                case 'w':
                    return Direction.Up;
                case 's':
                    return Direction.Down;
                case 'a':
                    return Direction.Left;
                case 'd':
                    return Direction.Right;
            }
            return Direction.None;
        }
        static bool isHit = false;
        static bool isDead = false;
        static int HitCount = 0;
        static Random random;
        //private void Dead();
        
        static void Main(string[] args)
        {
            new Scenes().PrintStart();

            Console.ReadKey(true);

            SaveObjects();//初始化豆子和障碍物
            snake = new Snake(7, 5, 1);
            //Action a = () => snake.ThreadMove();


            //Task t = new Task(a); //多线程开启：敌人1移动
            //t.Start();
            Direction dir = Direction.None;
            Point snake_pos = new Point(0, 0);

            while (true)
            {




                //snake.Move();
                snake_pos.y = snake.y;
                snake_pos.x = snake.x;
                ConsoleKeyInfo key = new ConsoleKeyInfo();
                key = Console.ReadKey(true);
                Point p_pos = new Point(p.x, p.y);
                //Point snake_pos = new Point(snake.x, snake.y);
                //e1.AIMove(e1, p);
                switch (key.KeyChar)
                {
                    case 'w':
                        //Tick();
                        //direction = Direction.Up;
                        p_pos.y--;
                        break;
                    case 's':
                        p_pos.y++;
                        break;
                    case 'a':
                        p_pos.x -= 2;
                        break;
                    case 'd':
                        p_pos.x += 2;
                        break;
                    default:
                        break;
                }

                //没撞墙
                if (p_pos.x >= 3 && p_pos.x < width && p_pos.y >= 2 && p_pos.y < height && !barrier.Contains(p_pos))
                {
                    p.x = p_pos.x;
                    p.y = p_pos.y;
                }
                if (snake.x == p.y && snake.y == p.x)
                {
                    isDead = true;
                    --life;
                    if (life == 0)
                    {
                        break;
                    }
                }
                if (snake.y >= 3 && snake.y < width && snake.x >= 2 && snake.x < height && !barrier.Contains(new Point(snake.y, snake.x)))
                {
                    dir = Input(key.KeyChar);
                    snake.SetDirection(dir, p);
                }

                else if (barrier.Contains(new Point(snake.y, snake.x)))
                {
                    isHit = true;
                    HitCount++;
                    if (!barrier.Contains(new Point(snake.y - 2, snake.x)))
                    {
                        if (HitCount < 2)
                        {
                            snake.y -= 2;

                        }
                        else
                        {
                            snake.x++;
                        }
                    }
                    else if (!barrier.Contains(new Point(snake.y + 2, snake.x)))
                    {
                        if (HitCount < 2)
                        {
                            snake.y += 2;
                        }
                        else
                        {
                            snake.x--;
                        }

                    }
                    else if (!barrier.Contains(new Point(snake.y, snake.x - 1)))
                    {
                        if (HitCount < 2)
                        {
                            snake.x++;
                        }
                        else
                        {
                            snake.y += 2;
                        }

                    }
                    else if (!barrier.Contains(new Point(snake.y, snake.x + 1)))
                    {
                        if (HitCount < 2)
                        {
                            snake.x--;
                        }
                        else
                        {
                            snake.y -= 2;
                        }
                    }
                    else
                    {
                        if (random != null)
                        {
                            int res = random.Next(0, 4);

                            if (res == 0)
                            {
                                snake.x--;
                            }
                            else if (res == 1)
                            {
                                snake.x++;
                            }
                            else if (res == 2)
                            {
                                snake.y -= 2;
                            }
                            else if (res == 3)
                            {
                                snake.y += 2;
                            }
                        }
                        else
                        {
                            snake.y -=2;
                        }
                    }
                }

                //else if (barrier.Contains(snake_pos))
                //{
                //    isHit = true;
                //    //continue;
                //}
                //else if(snake.y<3)
                //{
                //    dir = Direction.Down;
                //    snake.SetDirection(dir, p);
                //}
                //else if (snake.x < 2)
                //{
                //    dir = Direction.Right;
                //    snake.SetDirection(dir, p);
                //}
                //else if (snake.x >= height)
                //{
                //    dir = Direction.Up;
                //}
                //else if(snake.y >= width)
                //{
                //    dir = Direction.Left;
                //}
                //吃豆
                for (int i = 0; i < points.Count; i++)
                {
                    if (points[i].y == p.x && points[i].x == p.y)
                    {
                        points.RemoveAt(i);
                        p.score++;
                        break;
                    }
                }
                Thread.Sleep(250);


                ReadBuffer();
                DrawPlayerNew();
                DrawBuffer();
                Console.ForegroundColor = ConsoleColor.White;
                if (life == 3)
                {
                    Console.Write("剩余生命：");
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Console.WriteLine("◇◇◇");
                }
                else if(life == 2)
                {
                    Console.WriteLine("剩余生命：◇◇");
                }
                else 
                {
                    Console.WriteLine("剩余生命：◇");
                }
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("score:{0}\nx:{1},y:{2}\nisHit:{3}\nisDead:{4}\nplayer_x{5},player_y{6}", p.score, snake.x, snake.y, isHit, isDead, p.x, p.y);
                //Refresh();
            }
            new Scenes().PrintDie();
            //Console.ReadKey();
        }
    }
}