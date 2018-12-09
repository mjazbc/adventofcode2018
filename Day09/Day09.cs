using System;
using System.Collections.Generic;
using System.Linq;
using Util;

namespace Day09
{
    class Day09 : AdventPuzzle
    {
        protected override string SolveFirstPuzzle()
        {
            string[] input = ReadInputText<string>().Split();
            int players = int.Parse(input[0]);
            int lastMarble = int.Parse(input[6]);

            return GetMaxScore(players, lastMarble).ToString();
        }


        protected override string SolveSecondPuzzle()
        {
            string[] input = ReadInputText<string>().Split();
            int players = int.Parse(input[0]);
            int lastMarble = int.Parse(input[6]) * 100;

            return GetMaxScore(players, lastMarble).ToString();
        }

        private long GetMaxScore(int players, int lastMarble)
        {
            LinkedList<int> circle = new LinkedList<int>();
            var current = circle.AddFirst(0);

            long[] playerScores = new long[players];

            for (int marble = 1; marble < lastMarble; marble++)
            {
                if (marble % 23 == 0)
                {
                    for (int i = 0; i < 6; i++)
                        current = current.Previous ?? circle.Last;
                    
                    playerScores[marble % players] += current.Previous.Value + marble;
                    circle.Remove(current.Previous);
                }
                else
                {
                    current = current.Next ?? circle.First;
                    current = circle.AddAfter(current, marble);
                }
            }

            return playerScores.Max();
        }
    }
}
