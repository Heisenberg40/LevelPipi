using System;
using System.Collections.Generic;
using System.Text;

namespace _04_ConsoleGame_SimpleMap
{
    class Enemy
    {
        public float speed;
        public int x = 7;
        public int y = 5;
        public bool isWeak = false;//默认玩家无法攻击敌人
        public Enemy(int x, int y)
        {
            this.x = x;
            this.y = y;
            //speed = v;
        }

        public bool Hitable()
        {
            return isWeak = true;
        }
        
        public void AIMove(Enemy enemy, Player player)//获取到玩家位置，向其移动
        {
            if (enemy.x <= 2 || enemy.x >= 8 || enemy.y <= 2 || enemy.y >= 38)
            {
                return;
            }
            if(player.x > enemy.x)
            {
                enemy.x--;
            } 
            else if (player.x < enemy.x)
            {
                enemy.x++;
            }
        }
    }
}
