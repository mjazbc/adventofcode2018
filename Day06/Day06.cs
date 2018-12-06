using System;
using System.Collections.Generic;
using System.Linq;
using Util;

namespace Day06
{
    class Day06 : AdventPuzzle
    {
        protected override string SolveFirstPuzzle()
        {
            string[] input = ReadInputArray<string>();
            var parsedInput = ParseCoordinates(input);

            (int minX, int maxX, int minY, int maxY) = GetEdges(parsedInput);

            var edgeCoordinates = parsedInput.Where(c => c.X == minX || c.X == maxX || c.Y == maxY || c.Y == minY);

            Dictionary<int, HashSet<Coordinate>> assignedCoordinates = new Dictionary<int, HashSet<Coordinate>>();

            for (int x = minX; x < maxX; x++)
            {
                for (int y = minY; y < maxY; y++)
                {
                    var distances = parsedInput.ToDictionary(c => c, c => c.Distance(x, y));
                    var minDistance = distances.Min(d => d.Value);
                    var minDistanceCoordinates = distances.Where(d => d.Value == minDistance).ToList();
                    
                    if (minDistanceCoordinates.Count == 1)
                    {
                        var coord = minDistanceCoordinates.First();
                        if (!assignedCoordinates.ContainsKey(coord.Key.Id))
                            assignedCoordinates.Add(coord.Key.Id, new HashSet<Coordinate>());

                        assignedCoordinates[coord.Key.Id].Add(new Coordinate(x, y));
                        
                    }
                }
            }

            return assignedCoordinates.Where(x => !edgeCoordinates.Select(ec => ec.Id).Contains(x.Key))
                .Max(x => x.Value.Count).ToString();

        }

        protected override string SolveSecondPuzzle()
        {
            string[] input = ReadInputArray<string>();
            var parsedInput = ParseCoordinates(input);

            (int minX, int maxX, int minY, int maxY) = GetEdges(parsedInput);

            int count = 0;

            for (int x = minX; x < maxX; x++)
            {
                for (int y = minY; y < maxY; y++)
                {
                    var distancesSum = parsedInput.Sum(c => c.Distance(x, y));

                    if (distancesSum < 10000)
                        count++;
                }
            }

            return count.ToString();
        }

        private (int, int, int, int) GetEdges(Coordinate[] parsedInput)
        {
            int minX = parsedInput.Min(c => c.X);
            int maxX = parsedInput.Max(c => c.X);
            int minY = parsedInput.Min(c => c.Y);
            int maxY = parsedInput.Max(c => c.Y);

            return (minX, maxX, minY, maxY);
        }

        private Coordinate[] ParseCoordinates(string[] input)
        {      
            Coordinate[] coords = new Coordinate[input.Length];
            for (int i = 0; i < coords.Length; i++)
            {
                var split = input[i].Split(',');
                coords[i] = new Coordinate
                {
                    X = int.Parse(split[0]),
                    Y = int.Parse(split[1]),
                    Id = i
                };
            }

            return coords;
        }

        class Coordinate
        {

            public int X;
            public int Y;
            public int Id;

            public Coordinate(int x, int y)
            {
                X = x;
                Y = y;
            }

            public Coordinate()
            {
                
            }
            public int Distance(int x, int y) => Math.Abs(X - x) + Math.Abs(Y - y);

        }
    }
}
