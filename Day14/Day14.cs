using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Util;

namespace Day14
{
    class Day14 : AdventPuzzle
    {
        protected override string SolveFirstPuzzle()
        {
            int input = ReadInputText<int>();
            LinkedList<int> scoreBoard = new LinkedList<int>();
            scoreBoard.AddFirst(3);
            scoreBoard.AddLast(7);

            var firstElf = scoreBoard.First;
            var secondElf = scoreBoard.Last;

            for(int i = 0; i < input + 10; i++)
            {
                int sum = firstElf.Value + secondElf.Value;

                if (sum >= 10)
                    scoreBoard.AddLast(sum / 10);

                scoreBoard.AddLast(sum % 10);

                MoveForward(ref firstElf);
                MoveForward(ref secondElf);
            }

            return string.Join("", scoreBoard.Skip(input).Take(10));

        }

        protected override string SolveSecondPuzzle()
        {
            int input = ReadInputText<int>();
            LinkedList<int> scoreBoard = new LinkedList<int>();
            scoreBoard.AddFirst(3);
            scoreBoard.AddLast(7);

            int inputLen = input.ToString().Length;
            string inputString = input.ToString();

            var firstElf = scoreBoard.First;
            var secondElf = scoreBoard.Last;

            string lastsequence = "37";

            while(true)
            {
                int sum = firstElf.Value + secondElf.Value;

                if (sum >= 10) { 
                    scoreBoard.AddLast(sum / 10);
                    lastsequence += sum / 10;

                    if (lastsequence.Length > inputLen)
                        lastsequence = lastsequence.Substring(1);
                    
                    if (lastsequence == inputString)
                        return (scoreBoard.Count - inputLen).ToString();

                }

                scoreBoard.AddLast(sum % 10);
                lastsequence += sum % 10;

                if (lastsequence.Length > inputLen)
                    lastsequence = lastsequence.Substring(1);

                if (lastsequence == inputString)
                    return (scoreBoard.Count - inputLen).ToString();

                MoveForward(ref firstElf);
                MoveForward(ref secondElf);
            }
        }

        private void MoveForward(ref LinkedListNode<int> elf)
        {
            int steps = elf.Value + 1;
            for (int i = 0; i < steps; i++)
                elf = elf.Next ?? elf.List.First;
        }

    }
}
