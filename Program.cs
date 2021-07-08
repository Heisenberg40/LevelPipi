﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.Threading;
using System.IO;

namespace _04_ConsoleGame_SimpleMap
{
    public class Player
    {
        public int x;
        public int y;
        public int score=0;
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

    class Map
    {
        
    }

    class Program
    {
        static int width = 30;
        static int height = 20;
        static int offset_x = 2;
        static int offset_y = 2;
        static List<Point> points = new List<Point>();
        static Player p = new Player();

        static char[,] buffer = new char[height+offset_y+5, width+offset_x+5];
        
        static void ClearBuffer()
        {
            for(int i = 0; i < buffer.GetLength(0); i++)
            {
                for(int j = 0; j < buffer.GetLength(1); j++)
                {
                    buffer[i, j] = ' ';
                }
            }
            //Thread.Sleep(100);
        }
        
        static void DrawBorder()
        {
            //up
            for(int i = 0; i < width; i++)
            {
                buffer[offset_y-1, i + offset_x] = '#';
            }
            //down
            for(int i = 0; i < width; i++)
            {
                buffer[offset_y + height, i + offset_x] = '#';
            }
            //left
            for(int i = 0; i < height; i++)
            {
                buffer[i + offset_y, offset_x-1] = '#';
            }
            //right
            for(int i = 0; i < height; i++)
            {
                buffer[i + offset_y, offset_x + width] = '#';
            }
        }

        static void DrawPlayer()
        {
            buffer[p.y + offset_y, p.x + offset_x] = '@';
        }

        static void DrawPoints()
        {
            for(int i = 0; i < points.Count; i++)
            {
                buffer[points[i].y + offset_y, points[i].x + offset_x] = 'o';
            }
        }
        //static ConsoleColor color = Console.ForegroundColor;
        static void Refresh()
        {
            ClearBuffer();
            DrawBorder();
            DrawPlayer();
            DrawPoints();

            Console.Clear();

            //ConsoleColor color = Console.ForegroundColor;
            StringBuilder sb = new StringBuilder();

            for(int i = 0; i < buffer.GetLength(0); i++)
            {
                for(int j = 0; j < buffer.GetLength(1); j++)
                {
                    //if(buffer[i,j] == '@')
                   // {
                    //    Console.ForegroundColor = ConsoleColor.Yellow;
                  //  }
                   sb.Append(buffer[i, j] + " ");
                  
                }
                sb.Append("\n");
            }
            
            Console.WriteLine(sb.ToString());
            //Console.ForegroundColor = color;
            Console.WriteLine("score:{0}", p.score);
        }
        static void Map_BinaryRead()
        {
            StreamReader reader = new StreamReader("..\\..\\..\\Level1.txt");
            //Console.WriteLine(reader.ReadToEnd());
            while (true)
            {
                int res = reader.Read();
                if (res == -1) break;
                else if((char)res =='#')
                {
                    Console.ForegroundColor = ConsoleColor.DarkBlue;
                    Console.Write((char)res);
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.Write((char)res);
                }
            }

            reader.Close();
        }
        static void Main(string[] args)
        {
            

            //Map_BinaryRead();

            //Console.ReadKey();
            points = new List<Point>();
            points.Add(new Point(4, 5));
            points.Add(new Point(4, 7));
            points.Add(new Point(1, 6));
            points.Add(new Point(12, 7));

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
                        p_pos.x--;
                        break;
                    case 'd':
                        p_pos.x++;
                        break;
                    default:
                        break;
                }
                //没撞墙
                if (p_pos.x >= 0 && p_pos.x < width && p_pos.y >= 0 && p_pos.y < height)
                {
                    p.x = p_pos.x;
                    p.y = p_pos.y;
                }
                //吃豆
                for(int i = 0; i < points.Count; i++)
                {
                    if (points[i].x == p.x && points[i].y == p.y)
                    {
                        points.RemoveAt(i);
                        p.score++;
                        break;
                    }
                }

                Refresh();
            }
        }
    }
}
