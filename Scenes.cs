using System;
using System.Collections.Generic;
using System.Text;

namespace _04_ConsoleGame_SimpleMap
{
    public class Scenes
    {

        public void PrintStart()
        {
            Console.WriteLine(@"




            $$$$$$$\                           $$\      $$\                     
            $$  __$$\                          $$$\    $$$ |                    
            $$ |  $$ |$$$$$$\   $$$$$$$\       $$$$\  $$$$ | $$$$$$\  $$$$$$$\  
            $$$$$$$  |\____$$\ $$  _____|      $$\$$\$$ $$ | \____$$\ $$  __$$\ 
            $$  ____/ $$$$$$$ |$$ /            $$ \$$$  $$ | $$$$$$$ |$$ |  $$ |
            $$ |     $$  __$$ |$$ |            $$ |\$  /$$ |$$  __$$ |$$ |  $$ |
            $$ |     \$$$$$$$ |\$$$$$$$\       $$ | \_/ $$ |\$$$$$$$ |$$ |  $$ |
            \__|      \_______| \_______|      \__|     \__| \_______|\__|  \__|
                                                                     
                                                                   
                                ──────▄████▄─────
                                ─────███▄█ ──────
                                ──── ████──█──█──
                                ──── █████▄ ──────
                                ────  █████ ─────          

                                Press Any Key to Start
");
            Console.ReadKey(true);
        }
        public void PrintDie()
        {
            Console.Clear();
            Console.WriteLine(@"

                ▓██   ██▓  █████   █    ██    ▓████▄      █▓    ██▓███▄ 
                  ██  ██  ██   ██  ██  ▓██     ██       █▌          ▓██▀ 
                   ██ ██  ██   ██ ▓██   ██     ██       █▌ ██   ██████▌
                    ▐██▓  ██   ██ ▓▓█   ██     ▓█      ██  ▓█   ▓█▄  
                    ██▓   ████▓  ▓████▓    ████▓▓     ██   ████████▓ 
                   ██         ▓         ▓      ▓             
                 ▓██        


                               Press Enter to Restart
");
            Console.ReadLine();
        }
    }
}
