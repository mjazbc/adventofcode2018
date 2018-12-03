using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Util;

namespace Day03
{
    class Day03 : AdventPuzzle
    {
        protected override string SolveFirstPuzzle()
        {
            string[] input = ReadInputArray<string>();
            Claim[] claims = input.Select(x => new Claim(x)).ToArray();

            HashSet<string> overlaps = GetOverlaps(claims);

            return overlaps.Count.ToString();

        }

        protected override string SolveSecondPuzzle()
        {
            //TODO: Move this to abstract
            string[] input = ReadInputArray<string>();
            Claim[] claims = input.Select(x => new Claim(x)).ToArray();

            HashSet<string> overlaps = GetOverlaps(claims);

            //Gets id of a single claim that has none of its coordinates in overlaps.
            var notOverlapping = claims.Single(c => !c.GetCoordinates().Any(coord => overlaps.Contains(coord))).Id;
            return notOverlapping;
        }

        private HashSet<string> GetOverlaps(Claim[] claims)
        {
            HashSet<string> overlaps = new HashSet<string>();
            HashSet<string> allCoordinates = new HashSet<string>();

            foreach (var coordinates in claims.SelectMany(x => x.GetCoordinates()))
            {
                if (allCoordinates.Contains(coordinates))
                    overlaps.Add(coordinates);

                allCoordinates.Add(coordinates);
            }

            return overlaps;
        }

        class Claim
        {
            public string Id;
            public int FromLeft;
            public int FromTop;

            public int Width;
            public int Height;

            public Claim(string line)
            {
                var splitLine = line.Split();
                Id = splitLine[0].TrimStart('#');

                var coordinates = splitLine[2].TrimEnd(':').Split(',');
                FromLeft = int.Parse(coordinates[0]);
                FromTop = int.Parse(coordinates[1]);

                var size = splitLine[3].Split('x');
                Width = int.Parse(size[0]);
                Height = int.Parse(size[1]);
            }

            public List<string> GetCoordinates()
            {
                List<string> coordinates = new List<string>();

                for(int x = 0; x < Width; x++)
                    for(int y = 0; y < Height; y++)
                        coordinates.Add($"[{FromLeft + x}, {FromTop + y}]");

                return coordinates;
                
            }


        }
    }
}
