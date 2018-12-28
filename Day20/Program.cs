using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day20
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            Day20 d = new Day20();
            d.Solve(Util.Puzzle.Second);
        }
    }
}
