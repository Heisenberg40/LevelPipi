using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.Threading;
using System.IO;
using System.Linq;

namespace _04_ConsoleGame_SimpleMap
{
    //剩余问题：
    //1.棋子在移动到边界时会遇到index out of range异常
    //2.调整地图边界
    //3.棋子颜色
    //4.敌人AI制作
    enum Direction
    {
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
        static int width = 40;
        static int height = 8;
        static int offset_x = 2;
        static int offset_y = 2;
        static List<Point> points = new List<Point>();//用于存豆子位置的
        static List<Point> barrier = new List<Point>();//用于存障碍位置的
        static Player p = new Player();

        static char[,] MapArray = new char[10, 43];


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
        }
        
        static int[] Direction = new int[] { 0, 1, 2, 3 };//0 < 1 > 2 V 3 A
       
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

        

        static void Main(string[] args)
        {
            //缺一个开场文字 -- 按下任意键开始
            Console.ReadKey(true);
            SaveObjects();//初始化豆子和障碍物
            while (true)
            {
                ConsoleKeyInfo key = new ConsoleKeyInfo();
                key = Console.ReadKey();
                Point p_pos = new Point(p.x, p.y);
                
                switch (key.KeyChar)
                {
                    case 'w':
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


                ReadBuffer();
                DrawPlayerNew();
                DrawBuffer();
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("score:{0}\nx:{1},y:{2}", p.score,p_pos.x,p_pos.y);
                //Refresh();
            }
        }
    }
}