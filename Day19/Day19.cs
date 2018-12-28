using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Util;

namespace Day19
{
    public class Day19 : AdventPuzzle
    {
        protected override string SolveFirstPuzzle()
        {
            string[] input = ReadInputArray<string>();
            var program = input.Skip(1).Select(line => new ProgramLine(line)).ToArray();

            int ipReg = int.Parse(input[0].Replace("#ip ", ""));
            int[] registers = new int[6];
            int ip = 0;

            while (ip < program.Length)
            {
                program[ip].Cmnd(registers);
                ip = ++registers[ipReg];
            }

            return registers[0].ToString();
        }

        protected override string SolveSecondPuzzle()
        {
            //string[] input = ReadInputArray<string>();
            //var program = input.Skip(1).Select(line => new ProgramLine(line)).ToArray();
            ////3 10551341 10551340 12 4 1
            //int ipReg = int.Parse(input[0].Replace("#ip ", ""));
            //int[] registers = new int[] { 3, 10551341, 10551340, 12, 10551342, 1 };
            ////registers[0] = 1;
            //int ip = 13;

            //while (ip < program.Length)
            //{
            //    Console.WriteLine(program[ip]);
                

            //    program[ip].Cmnd(registers);
            //    Console.WriteLine(string.Join(" ", registers));
            //    ip = ++registers[ipReg];

               
            //}

            return "Nope.";
        }

    }

    public class ProgramLine
    {
        public Action<int[]> Cmnd;
        private readonly int[] instr;

        private readonly string description;

        public ProgramLine(string line)
        {
            description = line;

            var splitLine = line.Split();
            instr = splitLine.Skip(1).Select(int.Parse).ToArray();
            
            MethodInfo method = this.GetType().GetMethod(splitLine[0], 
                BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.IgnoreCase);

            Cmnd = (Action<int[]>) Delegate.CreateDelegate(typeof(Action<int[]>), this, method);
        }

        public override string ToString() => description;

        private void Addr(int[] reg) => reg[instr[2]] = reg[instr[0]] + reg[instr[1]];
        private void Addi(int[] reg) => reg[instr[2]] = reg[instr[0]] + instr[1];
        private void Mulr(int[] reg) => reg[instr[2]] = reg[instr[0]] * reg[instr[1]];
        private void Muli(int[] reg) => reg[instr[2]] = reg[instr[0]] * instr[1];
        private void Banr(int[] reg) => reg[instr[2]] = reg[instr[0]] & reg[instr[1]];
        private void Bani(int[] reg) => reg[instr[2]] = reg[instr[0]] & instr[1];
        private void Borr(int[] reg) => reg[instr[2]] = reg[instr[0]] | reg[instr[1]];
        private void Bori(int[] reg) => reg[instr[2]] = reg[instr[0]] | instr[1];
        private void Setr(int[] reg) => reg[instr[2]] = reg[instr[0]];
        private void Seti(int[] reg) => reg[instr[2]] = instr[0];
        private void Gtir(int[] reg) => reg[instr[2]] = instr[0] > reg[instr[1]] ? 1 : 0;
        private void Gtri(int[] reg) => reg[instr[2]] = reg[instr[0]] > instr[1] ? 1 : 0;
        private void Gtrr(int[] reg) => reg[instr[2]] = reg[instr[0]] > reg[instr[1]] ? 1 : 0;
        private void Eqir(int[] reg) => reg[instr[2]] = instr[0] == reg[instr[1]] ? 1 : 0;
        private void Eqri(int[] reg) => reg[instr[2]] = reg[instr[0]] == instr[1] ? 1 : 0;
        private void Eqrr(int[] reg) => reg[instr[2]] = reg[instr[0]] == reg[instr[1]] ? 1 : 0;
    }
}
