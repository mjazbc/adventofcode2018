using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Util;

namespace Day16
{
    class Day16 : AdventPuzzle
    {
        protected override string SolveFirstPuzzle()
        {
            string[] input = ReadInputArray<string>();
            string[] samples = ParseSamples(input);

            List<Action<int[], int[]>> actions = new List<Action<int[], int[]>>{
                Addr, Addi, Mulr, Muli, Banr, Bani, Borr, Bori, Setr, Seti, Gtir, Gtri, Gtrr, Eqir, Eqri, Eqrr
            };

            int total = 0;

            for (int i = 0; i < samples.Length; i += 3)
            {
                var before = ParseRegister(samples[i]);
                var after = ParseRegister(samples[i + 2]);
                var instruction = ParseInstruction(samples[i + 1]);

                int[] beforeCopy = new int[before.Length];
                before.CopyTo(beforeCopy, 0);

                int c = 0;
                foreach (var Action in actions)
                {
                    Action(before, instruction);
                    if (before.SequenceEqual(after))
                        c++;

                    beforeCopy.CopyTo(before, 0);
                }

                if (c >= 3)
                    total++;
            }

            return total.ToString();
        }

        protected override string SolveSecondPuzzle()
        {
            string[] input = ReadInputArray<string>();
            string[] samples = ParseSamples(input);

            List<Action<int[], int[]>> actions = new List<Action<int[], int[]>>{
                Addr, Addi, Mulr, Muli, Banr, Bani, Borr, Bori, Setr, Seti, Gtir, Gtri, Gtrr, Eqir, Eqri, Eqrr
            };

            Dictionary<int, HashSet<Action<int[], int[]>>> possibleActions = new Dictionary<int, HashSet<Action<int[], int[]>>>();

            //Get Actions matching sample inputs and outputs

            for (int i = 0; i < samples.Length; i += 3)
            {
                var before = ParseRegister(samples[i]);
                var after = ParseRegister(samples[i + 2]);
                var instruction = ParseInstruction(samples[i + 1]);

                int[] beforeCopy = new int[before.Length];
                before.CopyTo(beforeCopy, 0);

                int c = 0;
                foreach (var Action in actions)
                {
                    Action(before, instruction);
                    if (before.SequenceEqual(after)) {

                        if (!possibleActions.ContainsKey(instruction[0]))
                            possibleActions.Add(instruction[0], new HashSet<Action<int[], int[]>>());

                        possibleActions[instruction[0]].Add(Action);       
                    }   

                    beforeCopy.CopyTo(before, 0);
                }
            }

            //Reduce to single action per opcode
            while(possibleActions.Any(a=>a.Value.Count != 1))
            {
                foreach(var action in possibleActions.Where(a => a.Value.Count == 1))
                {
                    foreach(var ac in possibleActions.Where(a => a.Value.Count > 1))
                    {
                        ac.Value.Remove(action.Value.Single());
                    }
                }
            }

            var program = ParseProgram(input);

            int[] registers = new int[] { 0, 0, 0, 0 };

            //Execute program
            foreach(var line in program)
            {
                int opcode = line[0];
                possibleActions[opcode].Single()(registers, line);
            }

            return registers[0].ToString();

        }

        private int[][] ParseProgram(string[] input)
        {
            var lastIdx = Array.LastIndexOf(input, input.LastOrDefault(y => y.Contains("After")));
            var program = input.Skip(lastIdx + 3).Where(l => !string.IsNullOrWhiteSpace(l)).Select(ParseInstruction).ToArray();
            return program;
        }

        private string[] ParseSamples(string[] input)
        {
            var lastIdx = Array.LastIndexOf(input, input.LastOrDefault(y => y.Contains("After")));
            var samples = input.Take(lastIdx + 1).Where(l => !string.IsNullOrWhiteSpace(l)).ToArray();
            return samples;
        }

        private int[] ParseRegister(string sample)
        {
            sample = sample.Replace("Before: ", "").Replace("After:  ", "");
            return sample.TrimStart('[').TrimEnd(']').Split(',').Select(int.Parse).ToArray();
        }

        private int[] ParseInstruction(string sample) => sample.Split().Select(int.Parse).ToArray();

        private void Addr(int[] reg, int[] instr) => reg[instr[3]] = reg[instr[1]] + reg[instr[2]];
        private void Addi(int[] reg, int[] instr) => reg[instr[3]] = reg[instr[1]] + instr[2];
        private void Mulr(int[] reg, int[] instr) => reg[instr[3]] = reg[instr[1]] * reg[instr[2]];
        private void Muli(int[] reg, int[] instr) => reg[instr[3]] = reg[instr[1]] * instr[2];
        private void Banr(int[] reg, int[] instr) => reg[instr[3]] = reg[instr[1]] & reg[instr[2]];
        private void Bani(int[] reg, int[] instr) => reg[instr[3]] = reg[instr[1]] & instr[2];
        private void Borr(int[] reg, int[] instr) => reg[instr[3]] = reg[instr[1]] | reg[instr[2]];
        private void Bori(int[] reg, int[] instr) => reg[instr[3]] = reg[instr[1]] | instr[2];
        private void Setr(int[] reg, int[] instr) => reg[instr[3]] = reg[instr[1]];
        private void Seti(int[] reg, int[] instr) => reg[instr[3]] = instr[1];
        private void Gtir(int[] reg, int[] instr) => reg[instr[3]] = instr[1] > reg[instr[2]] ? 1 : 0;
        private void Gtri(int[] reg, int[] instr) => reg[instr[3]] = reg[instr[1]] > instr[2] ? 1 : 0;
        private void Gtrr(int[] reg, int[] instr) => reg[instr[3]] = reg[instr[1]] > reg[instr[2]] ? 1 : 0;
        private void Eqir(int[] reg, int[] instr) => reg[instr[3]] = instr[1] == reg[instr[2]] ? 1 : 0;
        private void Eqri(int[] reg, int[] instr) => reg[instr[3]] = reg[instr[1]] == instr[2] ? 1 : 0;
        private void Eqrr(int[] reg, int[] instr) => reg[instr[3]] = reg[instr[1]] == reg[instr[2]] ? 1 : 0;

    }
}
