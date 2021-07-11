using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.Threading;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace _04_ConsoleGame_SimpleMap
{
    //剩余问题：
    //1.棋子在移动到边界时会遇到index out of range异常
    //2.调整地图边界
    //3.棋子颜色
    //4.敌人AI制作
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
        //static int offset_x = 2;
        //static int offset_y = 2;
        static List<Point> points = new List<Point>();//用于存豆子位置的
        static List<Point> barrier = new List<Point>();//用于存障碍位置的
        static Player p = new Player();
        static Snake snake;
        static char[,] MapArray = new char[10, 43];
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
                    else if(MapArray[i,j] == '#')
                    {
                        barrier.Add(new Point(j, i));
                    }
                }
            }
        }

        static Direction Input()
        {
            var key = Console.ReadKey();
            switch (key.Key)
            {
                case ConsoleKey.UpArrow:
                    return Direction.Up;
                case ConsoleKey.DownArrow:
                    return Direction.Down;
                case ConsoleKey.LeftArrow:
                    return Direction.Left;
                case ConsoleKey.RightArrow:
                    return Direction.Right;
            }
            return Direction.None;
        }
        
        static void Main(string[] args)
        {
            //缺一个开场文字 -- 按下任意键开始
            Console.ReadKey(true);
            
            SaveObjects();//初始化豆子和障碍物
            snake = new Snake(7, 5, 1);
            Action a = () => snake.ThreadMethod();
            

            Task t = new Task(a); //多线程开启：敌人1移动
            //t.Start();

            while (true)
            {
                
                //Direction dir = Direction.Up;
                //// 用while循环，不断吃掉用户输入，防止一帧内频繁按键
                ////while (Console.KeyAvailable)
                ////{
                ////    dir = Input();
                ////}

                //snake.SetDirection(dir);
                //snake.Move();



                ConsoleKeyInfo key = new ConsoleKeyInfo();
                key = Console.ReadKey(true);
                Point p_pos = new Point(p.x, p.y);
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
                //Thread.Sleep(250);

                ReadBuffer();
                DrawPlayerNew();
                DrawBuffer();
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("score:{0}\nx:{1},y:{2}", p.score, p_pos.x, p_pos.y);
                //Refresh();
            }
        }
    }
}