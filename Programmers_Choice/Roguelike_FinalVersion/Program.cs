using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Roguelike_FinalVersion
{
    class Program
    {
        static void Main(string[] args)
        {
            Game g = new Game();

            g.Menu();
            g.Init();
            g.Run();
            g.End();

            Console.SetCursorPosition(0, Console.WindowHeight - 1);
            Console.Write("Press ENTER to continue...");
            Console.ReadLine();
        }
    }
}
