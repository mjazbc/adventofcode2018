using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Util;

namespace Day18
{
    public class Day18 : AdventPuzzle
    {
        protected override string SolveFirstPuzzle()
        {
            string[] input = ReadInputArray<string>();
            char[][] map = input.Select(c => c.ToCharArray()).ToArray();

            for(int minute = 1; minute <= 10; minute++) 
                map = ChangeLandscape(map);

            int mul = GetResourceValue(map);

            return mul.ToString();
        }

        protected override string SolveSecondPuzzle()
        {
            int minutes = 1000000000;

            string[] input = ReadInputArray<string>();
            char[][] map = input.Select(c => c.ToCharArray()).ToArray();

            bool cycleFound = false;
            int cycleStart = 0;
            int cycleLen = 0;

            Dictionary<int,int> results = new Dictionary<int, int>();
            for (int minute = 1; ; minute++)
            {
                map = ChangeLandscape(map);
                int mul = GetResourceValue(map);

                if (mul == cycleStart)
                    break;

                if (!cycleFound && results.Values.Count(v => v == mul) > 2)
                {
                    cycleFound = true;
                    cycleStart = mul;
                }

                if (cycleFound)
                    cycleLen++;

                results.Add(minute, mul);
            }

            var cycle = results.Values.Skip(results.Count() - cycleLen).ToArray();
            return cycle[((minutes - results.Count()) % cycleLen) - 1].ToString();

        }

        private char[][] ChangeLandscape(char[][] map)
        {
            var newMap = map.Select(c => c.ToArray()).ToArray();

            for (int y = 0; y < map[0].Length; y++)
                for (int x = 0; x < map.Length; x++)
                    CheckAdjecant(x, y, map, newMap);

            return newMap.Select(c => c.ToArray()).ToArray();
        }

        private int GetResourceValue(char[][] map)
        {
            var total = string.Join("", map.SelectMany(c => c));
            var mul = total.Count(c => c == '#') * total.Count(c => c == '|');
            return mul;
        }

        private void CheckAdjecant(int x, int y, char[][] map, char[][] newMap)
        {
            int trees = 0;
            int lumberyards = 0;

            char current = map[y][x];

            for (int i = -1; i <= 1; i++)
            {
                for (int j = -1; j <= 1; j++)
                {
                    //Continue if on current point or out of bounds
                    if ((i == 0 && j == 0) || x + j < 0 || x + j >= map.Length || y + i < 0 || y + i >= map[0].Length)
                        continue;

                    if (map[y + i][x + j] == '#')
                        lumberyards++;
                    else if (map[y + i][x + j] == '|')
                        trees++;
                }
            }

            if (current == '.' && trees >= 3)
                newMap[y][x] = '|';
            else if (current == '|' && lumberyards >= 3)
                newMap[y][x] = '#';
            else if (current == '#' && (lumberyards < 1 || trees < 1))
                newMap[y][x] = '.';
        }

        private void Print(char[][] map)
        {
            for (int y = 0; y < map[0].Length; y++)
            {
                for (int x = 0; x < map.Length; x++)
                    Console.Write(map[y][x]);

                Console.WriteLine();
            }
        }

    }
}
