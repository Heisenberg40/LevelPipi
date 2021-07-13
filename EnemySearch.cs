using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace _04_ConsoleGame_SimpleMap
{
    class LinkedNode
    {
        public Point pos;
        public LinkedNode previousNode;

        public LinkedNode(Point pos, LinkedNode previousNode)
        {
            this.pos = pos;
            this.previousNode = previousNode;
        }

        public LinkedNode(Point pos)
        {
            this.pos = pos;
        }

        public LinkedNode()
        {

        }
    }
    class EnemySearch
    {
        static Point GetDirectionPos(Point p, Direction dir)
        {
            switch (dir)
            {
                case Direction.Up:
                    return new Point(p.x-1, p.y);
                case Direction.Down:
                    return new Point(p.x+1, p.y);
                case Direction.Left:
                    return new Point(p.x, p.y-=2);
                case Direction.Right:
                    return new Point(p.x, p.y+=2);
                default:
                    return new Point(0,0);
            }
        }
        static bool CheckPos(char[,] map, Point p)
        {
            if (p.x == 1 || p.x == map.GetLength(0) - 1) { return false; }
            if (p.y == 1 || p.y == map.GetLength(1) - 1) { return false; }
            if (map[p.x, p.y] == '#') { return false; }
            return true;
        }
        public bool BreadthFirstSearch(Point start, Point end, char[,] map, LinkedNode endNode)
        {
            Queue<LinkedNode> pendingSearch = new Queue<LinkedNode>();

            LinkedNode startNode = new LinkedNode(start);
            pendingSearch.Enqueue(startNode);

            Dictionary<Point, bool> searchedPos = new Dictionary<Point, bool>();
            searchedPos.Add(start, true);

            while (pendingSearch.Count > 0)
            {
                LinkedNode currentNode = pendingSearch.Dequeue();

                
                if (currentNode.pos.ValueEqules(end))
                {
                    endNode.previousNode = currentNode.previousNode;
                    return true;
                }

                for(int i = 0; i < 4; i++)
                {
                    Point NextPos = GetDirectionPos(currentNode.pos, (Direction)i);

                    if(!searchedPos.ContainsKey(NextPos) && CheckPos(map, NextPos))
                    {
                        LinkedNode nextNode = new LinkedNode(NextPos);
                        //Program.DrawDebugProcess(NextPos);
                        nextNode.previousNode = currentNode;
                        pendingSearch.Enqueue(nextNode);
                        searchedPos.Add(NextPos, true);
                    }
                }
            }
            return false;
        }

       
    
        public void FindPath(LinkedNode endNode)
        {
            LinkedNode curNode = endNode;
            while(curNode.previousNode!=null&&!curNode.previousNode.pos.ValueEqules(new Point(1, 1)))
            {
                curNode = curNode.previousNode;
            }
        }
    }
}
