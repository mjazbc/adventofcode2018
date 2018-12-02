using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day02
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            Day02 d2 = new Day02();

            d2.Solve(Util.Puzzle.Both);
        }
    }
}
