using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Util;

namespace Day01
{
    class Day01 : AdventPuzzle
    {

        protected override string SolveFirstPuzzle()
        {
            int[] input = ReadInputArray<int>();
            return input.Sum().ToString();
        }

        protected override string SolveSecondPuzzle()
        {
            int[] input = ReadInputArray<int>();
            HashSet<int> frequencies = new HashSet<int>();

            int i = 0;
            int sum = 0;

            while (!frequencies.Contains(sum))
            {
                frequencies.Add(sum);
                sum += input[i++ % input.Length];

            }

            return sum.ToString();
        }
    }
}
