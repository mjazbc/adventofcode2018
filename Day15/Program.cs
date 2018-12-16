using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day15
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            Day15 d = new Day15();
            d.Solve(Util.Puzzle.Second);
        }
    }
}
