using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.Threading;
using System.IO;
using System.Threading.Tasks;

namespace _04_ConsoleGame_SimpleMap
{
    //剩余问题：
    
    //4.敌人AI制作
    //关卡2制作
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
        public int x;
        public int y;
        public int score = 0;
        public Player(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
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

        public bool ValueEqules(Point v)
        {
            return this.x == v.x && this.y == v.y;
        }

        public override string ToString()
        {
            return  x+"|"+y;
        }
    }


    class Program
    {
        static int width = 40;
        static int height = 10;
        static List<Point> points = new List<Point>();
        static List<Point> powers = new List<Point>();

        static Player player = new Player(2,4);
        static Direction direction;
        static char[,] buffer = new char[10, 43];
        static char[,] fileMap = new char[10, 43];
        static char[,] MapArray = new char[10, 43];
        static char[,] MapArray2 = new char[23, 36];
        static Snake snake = new Snake(5, 32);
        static int life = 3;
        static bool isDead = false;
        static bool isHit = false;
        static bool isRevert = false;
        static bool isFinish = false;//是否通关
        static bool isSleep = false;//敌人是否处于沉睡
        static int RevertTime = 10;
        static void RevertTick()
        {
            isRevert = true;
            for (int i = RevertTime; i >=0; i--)
            {
                //Console.Write("{0}",i);
                //Thread.Sleep(200);
                //Console.Write('\u0008');
                Thread.Sleep(1000);
            }
            isRevert = false;
            //Console.BackgroundColor = ConsoleColor.Black;
        }
        static void EnemySleep()
        {
            isSleep = true;
            for (int i = 5; i >= 0; i--)
            {
                Thread.Sleep(1000);
            }
            isSleep = false;
        }

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

        static void ReadBuffer2()
        {
            StreamReader reader = new StreamReader("..\\..\\..\\Level2.txt");
            int count = 0;
            while (count < 23)//count代表行数，最多10行
            {
                string s = reader.ReadLine();

                char[] line1 = s.ToCharArray();

                for (int i = 0; i < MapArray2.GetLength(1); i++)
                {
                    MapArray2[count, i] = line1[i];
                    Debug.WriteLine(i);
                }
                count++;
            }
            reader.Close();
        }
        static void DrawBuffer(char[,]MapArray)
        {
            Console.Clear();

            for (int i = 0; i < MapArray.GetLength(0); i++)
            {
                for (int j = 0; j < MapArray.GetLength(1); j+=2)
                {
                    if (MapArray[i, j] == '#')
                    {
                        Console.ForegroundColor = ConsoleColor.DarkBlue;
                        Console.Write('■');
                    }
                    else if (MapArray[i, j] == 'o')
                    {
                        if (points.Contains(new Point(i, j)))
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
                        if (!isRevert)
                        {
                            Console.ForegroundColor = ConsoleColor.Yellow;
                        }
                        else
                        {
                            Console.ForegroundColor = ConsoleColor.DarkMagenta;
                        }
                        Console.Write("★");
                    }
                    else if (MapArray[i, j] == 'E')
                    {
                        if (!isRevert)
                        {
                            Console.ForegroundColor = ConsoleColor.DarkMagenta;
                        }
                        else
                        {
                            Console.ForegroundColor = ConsoleColor.Yellow;
                        }
                        Console.Write('▲');
                    }
                    else if (MapArray[i, j] == 'P')
                    {
                        if (powers.Contains(new Point(i, j)))
                        {
                            Console.ForegroundColor = ConsoleColor.DarkGreen;
                            Console.Write('○');
                        }
                        else//如果豆子已经被吃了，显示为空
                        {
                            Console.Write("  ");
                        }
                    }
                    else
                    {
                        Console.Write("  ");
                    }
                }
                Console.WriteLine();
            }
        }

        static void ClearBuffer()
        {
            for (int i = 0; i < buffer.GetLength(0); i++)
            {
                for (int j = 0; j < buffer.GetLength(1); j++)
                {
                    buffer[i, j] = ' ';
                }
            }
        }


        static void DrawPlayerNew()
        {
            MapArray[player.x, player.y] = '@';
            MapArray[snake.x, snake.y] = 'E';
        }

        
        static void SaveObjects()//把txt文件中的信息读取到MapArray中，并且取出豆子的位置信息存入points，取出道具位置存入powers
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
                    if (MapArray[i, j] == 'P')
                    {
                        powers.Add(new Point(i, j));
                    }
                }
            }
        }

        static void GameMain()
        {
            SaveObjects();
            Thread t1 = new Thread(RevertTick);
            Thread t2 = new Thread(RevertTick);
            Thread t3 = new Thread(EnemySleep);
            Thread t4 = new Thread(EnemySleep);
            int level = 1; 
            string[] print = new string[2] { "得分：", level.ToString() };
            Point target_pos = new Point(player.x, player.y);
            LinkedNode endNode = new LinkedNode(target_pos);
            LinkedNode curNode = new LinkedNode();
            EnemySearch route = new EnemySearch();
            List<Point> route_found = new List<Point>();
            while (true)
            {
                ConsoleKeyInfo key = new ConsoleKeyInfo();
                Point p_pos = new Point(player.x, player.y);
                Point snake_pos = new Point(snake.x, snake.y);
                while (Console.KeyAvailable)
                {
                    key = Console.ReadKey(true);

                    switch (key.KeyChar)
                    {
                        case 'w':
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
                            break;
                    }
                }
                switch (direction)//玩家的持续移动
                {
                    case Direction.Up:
                        p_pos.x--;
                        Thread.Sleep(400);
                        break;
                    case Direction.Down:
                        p_pos.x++;
                        Thread.Sleep(400);
                        break;
                    case Direction.Left:
                        p_pos.y -= 2;
                        Thread.Sleep(400);
                        break;
                    case Direction.Right:
                        p_pos.y += 2;
                        Thread.Sleep(400);
                        break;
                    default:
                        Thread.Sleep(400);
                        break;
                }
                
                //switch (direction)//敌人的持续移动，缓存在snake_pos中
                //{
                //    case Direction.Up:
                //        snake_pos.x++;
                //        Thread.Sleep(400);
                //        break;
                //    case Direction.Down:
                //        snake_pos.x--;
                //        Thread.Sleep(400);
                //        break;
                //    case Direction.Left:
                //        snake_pos.y += 2;
                //        Thread.Sleep(400);
                //        break;
                //    case Direction.Right:
                //        snake_pos.y -= 2;
                //        Thread.Sleep(400);
                //        break;
                //    default:
                //        Thread.Sleep(400);
                //        break;
                //}
                ///
                //没撞墙
                if (p_pos.x >= 2 && p_pos.x < height && p_pos.y >= 2 && p_pos.y < width && MapArray[p_pos.x, p_pos.y] != '#')
                {
                    player.x = p_pos.x;
                    player.y = p_pos.y;
                }
                //if (snake_pos.x >= 2 && snake_pos.x < height && snake_pos.y >= 2 && snake_pos.y < width && MapArray[snake_pos.x, snake_pos.y] != '#')
                //{
                //    snake.x = snake_pos.x;
                //    snake.y = snake_pos.y;
                //}
                if (player.x == snake.x && player.y == snake.y && !isRevert)
                {
                    life--;
                }
                else if (player.x == snake.x && player.y == snake.y && isRevert)//无敌状态下碰到敌人
                {
                    player.score += 100;
                    snake.x = 5;
                    snake.y = 32;//玩家吃了敌人后，敌人回到出生点
                    t3.Start();
                }

                bool isFind = route.BreadthFirstSearch(new Point(snake.x, snake.y), new Point(player.x,player.y), MapArray, endNode);
                Debug.WriteLine("寻路结果：" + isFind);
                //route.FindPath(endNode);
                curNode = endNode;
                route_found.Clear();
                while (curNode.previousNode != null && !curNode.previousNode.pos.ValueEqules(new Point(1, 1)))
                {
                    Debug.WriteLine("进入循环了,{0},{1}",curNode.pos.x,curNode.pos.y);
                    curNode = curNode.previousNode;
                    
                    route_found.Add(new Point(curNode.pos.x, curNode.pos.y));
                    //snake.x = curNode.pos.x;
                    //snake.y = curNode.pos.y;
                }
                if (!isSleep)
                {
                    for (int i = 0; i < route_found.Count - 1; i++)
                    {
                        snake.x = route_found[i].x;
                        snake.y = route_found[i].y;
                    }
                }
                
                //吃豆
                for (int i = 0; i < points.Count; i++)
                {
                    if (points[i].x == player.x && points[i].y == player.y)
                    {
                        points.RemoveAt(i);
                        isHit = true;
                        player.score+=10;
                        break;
                    }
                }
                if (points.Count == 0)
                {
                    isFinish = true;
                    break;
                }
                //吃道具
                for (int i = 0; i < powers.Count; i++)
                {
                    if (powers[i].x == player.x && powers[i].y == player.y)
                    {
                        powers.RemoveAt(i);

                        switch (t1.ThreadState)
                        {
                            case System.Threading.ThreadState.Unstarted:
                                t1.Start();
                                break;
                            case System.Threading.ThreadState.Stopped:
                                t2.Start();
                                break;
                            case System.Threading.ThreadState.Running:
                                t2.Start();
                                break;
                        }
                    }
                }

                ReadBuffer();
                DrawPlayerNew();
                DrawBuffer(MapArray);
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine();
                
                foreach (string s in print)
                {
                    if (s == "1")
                    {
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.BackgroundColor = ConsoleColor.DarkYellow;
                        Console.WriteLine("当前关卡："+s);
                        Console.BackgroundColor = ConsoleColor.Black;
                        Console.WriteLine();
                    }
                    else if (s == "得分：")
                    {
                        Console.WriteLine();
                        Console.BackgroundColor = ConsoleColor.DarkGray;
                        Console.WriteLine(s + player.score);
                        Console.BackgroundColor = ConsoleColor.Black;
                        Console.WriteLine();
                    }
                }
                Console.BackgroundColor = ConsoleColor.DarkRed;
                Console.Write("剩余生命：");
                if (life == 3)
                {
                    
                    Console.WriteLine("◇◇◇");
                    Console.BackgroundColor = ConsoleColor.Black;
                }
                else if (life == 2)
                {
                    Console.WriteLine("◇◇");
                    Console.BackgroundColor = ConsoleColor.Black;
                }
                else if (life == 1)
                {
                    Console.WriteLine("◇");
                    Console.BackgroundColor = ConsoleColor.Black;
                }
                else
                {
                    isDead = true;
                    break;
                }
                Console.ForegroundColor = ConsoleColor.White;

                
                //Console.WriteLine("score:{0}\nx:{1},y:{2}\nisHit:{3}\nisDead:{4}\nplayer_x{5},player_y{6}", player.score, snake.x, snake.y, isHit, isDead, player.x, player.y);
                //Console.WriteLine("score:{0}", player.score);
                if (isRevert)
                {
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.BackgroundColor = ConsoleColor.DarkMagenta;
                    
                    Console.WriteLine("无敌ING");
                    //Console.Write('\u0008');
                }
                Console.BackgroundColor = ConsoleColor.Black;
            }
        }
        static void GameMain2()
        {
            ReadBuffer2();
            DrawPlayerNew();
            DrawBuffer(MapArray2);
        }
        
        static void Main(string[] args)
        {
            Console.SetWindowSize((width + 4) * 2, (height + 2) * 2);
            Console.CursorVisible = false;
            //GameMain2();

            //Console.ReadKey();

            Scenes scene = new Scenes();
            scene.PrintStart();
            while (true)
            {
                GameMain();
                if (!isFinish)//未通关情况下，将所有数据置回初始状态
                {
                    scene.PrintDie();
                    life = 3;
                    snake.x = 5;
                    snake.y = 32;
                    player.x = 2;
                    player.y = 4;
                }
                else
                {
                    GameMain2();
                    break;
                }
            }
            scene.PrintFinish();
        }
    }
}