using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Util;

namespace Day10
{
    class Day10 : AdventPuzzle
    {
        protected override string SolveFirstPuzzle()
        {
            string[] input = ReadInputArray<string>();

            (HashSet<Coordinate> coords, int _) = GetMessage(input);

            string result = Draw(coords);
            return result;
        }

        private static (HashSet<Coordinate>, int) GetMessage(string[] input)
        {
            HashSet<Coordinate> coords = new HashSet<Coordinate>(input.Select(line => new Coordinate(line)));

            int dist = int.MaxValue;
            int newDist = dist;

            int i= -1;
            while (newDist <= dist)
            {
                foreach (var item in coords)
                    item.Move();

                dist = newDist;
                newDist = coords.Max(x => x.X) - coords.Min(x => x.X);
                i++;
            }

            foreach (var item in coords)
                item.MoveBack();

            return (coords, i);
        }

        private string Draw(HashSet<Coordinate> coordinates)
        {
            (int minX, int maxX, int minY, int maxY) = GetEdges(coordinates);

            int xdiff = maxX - minX + 1;
            int ydiff = maxY - minY + 1;

            StringBuilder sb = new StringBuilder();
            sb.AppendLine();

            for (int y = 0; y < ydiff; y++)   
            {
                for (int x = 0; x < xdiff; x++)
                {
                    if (coordinates.Any(c => c.X - minX == x && c.Y - minY == y))
                        sb.Append("# ");
                    else
                        sb.Append("  ");
                }

                sb.AppendLine();
            }

            return sb.ToString();
        }


        protected override string SolveSecondPuzzle()
        {
            string[] input = ReadInputArray<string>();

            (HashSet<Coordinate> _, int secs) = GetMessage(input);

            return secs.ToString();
        }

        private (int, int, int, int) GetEdges(HashSet<Coordinate> parsedInput)
        {
            int minX = parsedInput.Min(c => c.X);
            int maxX = parsedInput.Max(c => c.X);
            int minY = parsedInput.Min(c => c.Y);
            int maxY = parsedInput.Max(c => c.Y);

            return (minX, maxX, minY, maxY);
        }

    }

    class Coordinate
    {
        public int X;
        public int Y;

        public int MoveX;
        public int MoveY;

        public Coordinate(string line)
        {
            line = line.Replace("> velocity=<", ",").Replace("position=<", "").Replace(">", "");
            int[] coords = line.Split(',').Select(int.Parse).ToArray();

            X = coords[0];
            Y = coords[1];
            MoveX = coords[2];
            MoveY = coords[3];
        }

        public void Move()
        {
            X += MoveX;
            Y += MoveY;
        }

        public void MoveBack()
        {
            X -= MoveX;
            Y -= MoveY;
        }

    }
}
