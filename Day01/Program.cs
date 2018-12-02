using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day01
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            Day01 day01 = new Day01();

            day01.Solve(Util.Puzzle.Both);
        }

    }
}
