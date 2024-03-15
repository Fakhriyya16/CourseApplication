using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Helpers.Extensions
{
    public static class ConsoleExtension
    {
        public static void ConsoleMessage(this ConsoleColor color, string msj)
        {
            Console.ForegroundColor = color;
            Console.WriteLine(msj);
            Console.ResetColor();
        }
    }
}
