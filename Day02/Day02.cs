using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Util;

namespace Day02
{
    class Day02 : AdventPuzzle
    {
        protected override string SolveFirstPuzzle()
        {
            string[] input = ReadInputArray<string>();

            Dictionary<int, int> detected = new Dictionary<int, int>
            {
                {2, 0},
                {3, 0}
            };

            foreach(string line in input)
            {
                var counts = line.GroupBy(c => c).ToDictionary(g => g.Key, g => g.Count());

                if (counts.Where(x => x.Value == 2).Any())
                    detected[2]++;

                if (counts.Where(x => x.Value == 3).Any())
                    detected[3]++;

            }

            return (detected[2] * detected[3]).ToString();
        }

        protected override string SolveSecondPuzzle()
        {
            string[] input = ReadInputArray<string>();

            foreach(string line in input)
            {
                foreach(string compareLine in input.Where(l => l != line))
                {
                    var diffs = line.Zip(compareLine, (first, second) => first == second).ToArray();

                    if(diffs.Count(d => d == false) == 1)
                    {
                        return line.Remove(Array.IndexOf(diffs, false), 1);
                    }
                }
            }

            return null;
        }
    }
}
