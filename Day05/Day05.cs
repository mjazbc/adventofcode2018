using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Util;

namespace Day05
{
    class Day05 : AdventPuzzle
    {
        protected override string SolveFirstPuzzle()
        {
            string input = ReadInputText<string>();

            return GetReducedLength(input).ToString();
        }

        private int GetReducedLength(string input)
        {
            for (int i = 1; i < input.Length; i++)
            {
                char currChar = input[i];
                char prevChar = input[i - 1];

                if (Math.Abs(prevChar - currChar) == 32)
                {
                    input = input.Remove(i - 1, 2);
                    if (i == 1)
                        i = 0;
                    else
                        i = i - 2;
                }
            }

            return input.Length;
        }

        protected override string SolveSecondPuzzle()
        {
            string alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
          
            int shortest = 99999999;
            
            foreach (string character in alphabet.Select(x => x.ToString()))
            {
                string input = ReadInputText<string>();
                input = input.Replace(character, "").Replace(character.ToLower(), "");

                int len = GetReducedLength(input);

                if (len < shortest)
                    shortest = len;
            }

            return shortest.ToString();

        }
    }
}
