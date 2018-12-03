using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day03
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            Day03 d3 = new Day03();
            d3.Solve(Util.Puzzle.Both);
        }
    }
}
