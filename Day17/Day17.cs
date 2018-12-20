using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Util;

namespace Day17
{
    public class Day17 : AdventPuzzle
    {
        private Dictionary<(int x, int y), char> _water = new Dictionary<(int x, int y), char>();
        private int _miny;
        private int _maxy;

        protected override string SolveFirstPuzzle()
        {
            string[] input = ReadInputArray<string>();

            Queue<(int x, int y)> springs = new Queue<(int x, int y)>();
            HashSet<(int x, int y)> clayPoints = ParseClay(input);
            var map = CreateMap(clayPoints);

            RunFlow(springs, map);

            _miny = clayPoints.Min(c => c.y);
            _maxy = clayPoints.Max(c => c.y);
            return _water.Count(w => w.Key.y >= _miny && w.Key.y <= _maxy).ToString();
        }

        protected override string SolveSecondPuzzle()
        {
            return _water.Count(w => w.Key.y >= _miny && w.Key.y <= _maxy && w.Value == '~').ToString();
        }

        private void RunFlow(Queue<(int x, int y)> springs, char[,] map)
        {
            springs.Enqueue((500, 0));

            while (springs.Any())
            {
                var p = springs.Dequeue();

                while (p.y < map.GetLength(0) - 1 && map[p.y + 1, p.x] == '.')
                {
                    map[p.y, p.x] = '|';
                    _water[p] = '|';
                    p.y++;

                }

                if (p.y == map.GetLength(0) - 1 || map[p.y + 1, p.x] == '|')
                {
                    map[p.y, p.x] = '|';
                    _water[p] = '|';
                    continue;
                }

                Fill(map, springs, p);
            }
        }

        private void Fill(char[,] map, Queue<(int,int)> springs, (int x, int y) pos)
        {
            List<(int x, int y)> points = new List<(int x, int y)>();

            bool overflow = false;
            while (!overflow)
            {
                var overflowRight = CheckDirection(map, springs, pos, points, (x) => x+1);
                var overflowLeft = CheckDirection(map, springs, pos, points, (x) => x-1);

                overflow = overflowLeft || overflowRight;
                pos.y--;
            }

            var minY = points.Min(p => p.y);
            foreach(var point in points.Where(p =>p.y == minY)) { 
                map[point.y, point.x] = '|';
                _water[point] = '|';
            }


        }

        private bool CheckDirection(char[,] map, Queue<(int, int)> springs, (int x, int y) pos, List<(int x, int y)> points, Func<int,int> move)
        {
            while (map[pos.y, pos.x] != '#')
            {

                points.Add(pos);
                map[pos.y, pos.x] = '~';

                if (_water.ContainsKey(pos))
                    _water[pos] = '~';
                else
                    _water.Add(pos, '~');

                if (map[pos.y + 1, pos.x] == '.')
                {
                    springs.Enqueue(pos);
                    return true;
                }

                if (map[pos.y + 1, pos.x] == '|')
                    return true;

                pos.x = move(pos.x);
            }
            return false;
        }

        private char[,] CreateMap(HashSet<(int x, int y)> clayPoints)
        {
            char[,] map = new char[clayPoints.Max(c => c.y) +1, clayPoints.Max(c => c.x) +2];
            for (int y = 0 ; y < map.GetLength(0) ; y++)
            {
                for (int x = 0; x < map.GetLength(1) ; x++)
                {
                    if (clayPoints.Contains((x, y)))
                        map[y, x] = '#';
                    else
                        map[y, x] = '.';
                }
            }

            return map;
        }

        private HashSet<(int, int)> ParseClay(string[] input)
        {
            HashSet<(int x, int y)> clayPoints = new HashSet<(int x, int y)>();

            foreach (string line in input)
            {
                var split = line.Split(',');
                var var1 = split[0].Split('=');
                var var2 = split[1].Split('=');

                int tmpVar1 = int.Parse(var1[1]);
                var range = var2[1].Split(new string[] { ".." }, StringSplitOptions.None).Select(int.Parse).ToArray();

                for (int i = range[0]; i <= range[1]; i++)
                {
                    if (var1[0] == "x")
                        clayPoints.Add((tmpVar1, i));
                    else
                        clayPoints.Add((i, tmpVar1));
                }
            }

            return clayPoints;
        }

        private bool IsValid((int x, int y) position, char[,] map)
        {
            return !(position.x < 0 || position.y < 0 || position.x >= map.GetLength(1) || position.y >= map.GetLength(0));
        }

        #region Print

        private void Print(char[,] map, (int x, int y) current, HashSet<(int x, int y)> clayPoints)
        {
            for (int y = clayPoints.Min(c => c.y); y < clayPoints.Max(c=>c.y) +1 ; y++)
            {
                for (int x = clayPoints.Min(c => c.x) - 1; x < clayPoints.Max(c => c.x) + 1; x++)
                {
                    char c = map[y,x];

                    if (c== '~') { 

                        Console.ForegroundColor = ConsoleColor.Blue;
                        c = '▓';
                    }
                    else if (c == '|')
                    { 

                        Console.ForegroundColor = ConsoleColor.Cyan;
                        c = '░';
                    }

                    if (current.x == x && current.y == y)
                        Console.ForegroundColor = ConsoleColor.Yellow;

                    Console.Write(c);
                    Console.ResetColor();
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }

        private void Print(char[,] map, string filePath, HashSet<(int x, int y)> clayPoints)
        {
            using (var writer = new StreamWriter(filePath))
            { 
                for (int y = clayPoints.Min(c => c.y); y < clayPoints.Max(c => c.y) + 1; y++)
                {
                    for (int x = clayPoints.Min(c => c.x) - 1; x < clayPoints.Max(c => c.x) + 2; x++)
                    {
                        writer.Write(map[y, x]);
                    }
                    writer.WriteLine();
                }
            }
        }

        #endregion
    }
}
