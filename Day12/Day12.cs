using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Util;

namespace Day12
{
    class Day12 : AdventPuzzle
    {
        protected override string SolveFirstPuzzle()
        {
            string[] input = ReadInputArray<string>();

            string state = input[0].Substring(15);

            Dictionary<string, char> rules = new Dictionary<string, char>();

            foreach(var line in input.Skip(2))
            {
                var split = line.Split(new string[] { " => " } , 2, StringSplitOptions.RemoveEmptyEntries);
                rules.Add(split[0], split[1][0]);
            }

            int potIdx = 0;
            for(int gen = 0; gen < 20; gen++)
            {
                state = "..." + state + "...";
                potIdx -= 3;

                string newState = state;

                for (int i = 2; i < state.Length - 2; i++)
                {
                    string curr = state.Substring(i - 2, 5);
                    var tmpArray = newState.ToCharArray();

                    tmpArray[i] = rules[curr];

                    newState = string.Join("", tmpArray);
                    
                }
                state = newState;
                var tmp = state.ToCharArray();

            }

            var stateArray = state.ToCharArray();
            int sum = PotSum(stateArray, potIdx);

            return sum.ToString();
        }

        private int PotSum(char[] state, int stardIdx)
        {
            int sum = 0;
            for (int i = 0; i < state.Length; i++)
            {
                if (state[i] == '#')
                    sum += stardIdx;

                stardIdx++;
            }

            return sum;
        }

        protected override string SolveSecondPuzzle()
        {
            string[] input = ReadInputArray<string>();

            string state = input[0].Substring(15);

            Dictionary<string, char> rules = new Dictionary<string, char>();

            foreach (var line in input.Skip(2))
            {
                var split = line.Split(new string[] { " => " }, 2, StringSplitOptions.RemoveEmptyEntries);
                rules.Add(split[0], split[1][0]);
            }

            int count = 0;
            int prevcount = count;

            for (int gen = 0; gen < 1000; gen++)
            {
                state = "..." + state + "...";

                string newState = state;

                for (int i = 2; i < state.Length - 2; i++)
                {
                    string curr = state.Substring(i - 2, 5);
                    var tmpArray = newState.ToCharArray();

                    tmpArray[i] = rules[curr];

                    newState = string.Join("", tmpArray);
                    
                }

                state = newState;
                count = state.Count(x => x == '#');

                if (count == prevcount)
                    break;

                prevcount = count;

            }

            return (prevcount * 50000000000).ToString();
        }
    }
}
