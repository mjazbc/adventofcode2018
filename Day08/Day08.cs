using System;
using System.Collections.Generic;
using System.Linq;
using Util;

namespace Day08
{
    class Day08 : AdventPuzzle
    {
        protected override string SolveFirstPuzzle()
        {
            string text = ReadInputText<string>();
            int[] input = text.Split().Select(n => int.Parse(n)).ToArray();

            (int sum, int _) = SumMetaData(input);

            return sum.ToString();
           
        }

        private (int, int) SumMetaData(int[] input)
        {
            int childNodes = input[0];
            int metaEntries = input[1];

            int idx = 2;
            int sum = 0;

            for(int i = 0; i < childNodes; i++)
            {
                (int tmpSum, int tmpIdx) = SumMetaData(input.Skip(idx).ToArray());

                idx += tmpIdx;
                sum += tmpSum;
            }

            return (sum + input.Skip(idx).Take(metaEntries).Sum(), idx + metaEntries);          
        }


        private (int, int) SumChildValues(int[] input)
        {
            int childNodes = input[0];
            int metaEntries = input[1];

            int idx = 2;
            int sum = 0;

            int[] childNodeValues = new int[childNodes];

            for (int i = 0; i < childNodes; i++)
            {
                (int tmpSum, int tmpIdx) = SumChildValues(input.Skip(idx).ToArray());
                childNodeValues[i] = tmpSum;
                idx += tmpIdx;
            }

            if(childNodes == 0)
                sum += input.Skip(idx)
                    .Take(metaEntries)
                    .Sum();
            else
                sum += input.Skip(idx)
                    .Take(metaEntries)
                    .Where(i => i <= childNodeValues.Length)
                    .Sum(x => childNodeValues[x - 1]);

            return (sum, idx + metaEntries);
        }

        protected override string SolveSecondPuzzle()
        {
            string text = ReadInputText<string>();
            int[] input = text.Split().Select(n => int.Parse(n)).ToArray();

            (int sum, int _) = SumChildValues(input);

            return sum.ToString();
        }
    }
}
