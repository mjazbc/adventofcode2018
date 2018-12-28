using System;

using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Util;

namespace Day20
{
    public class Day20 : AdventPuzzle
    {
        private Dictionary<(int x, int y), bool> map = new Dictionary<(int, int), bool>();
        
        protected override string SolveFirstPuzzle()
        {
            string input = ReadInputText<string>();
            input = input.TrimStart('^').TrimEnd('$');

            map.Add((0, 0), false);
            GenerateMap( input, new List<(int x, int y)> { (0, 0) });

        
            var paths = FindPaths();

            return (paths.Max(x => x.Distance) / 2).ToString();            

        }

        protected override string SolveSecondPuzzle()
        {
            map = new Dictionary<(int x, int y), bool>();
            string input = ReadInputText<string>();
            input = input.TrimStart('^').TrimEnd('$');

            map.Add((0, 0), false);
            GenerateMap(input, new List<(int x, int y)> { (0, 0) });

            var paths = FindPaths();

            return (paths.Count(p=>p.Distance >= 2000)).ToString();
        }

        private List<(int x, int y)> GenerateMap(string regexString, List<(int x, int y)> current)
        {  
            for(int i = 0; ; i++)
            {
                if(i >= regexString.Length)
                    return current;

                char c = regexString[i];
                
                if (c == '(')
                {
                    var branches = SplitRegex(regexString.Substring(i));

                    current = branches.SelectMany(b => GenerateMap(b, current)).Distinct().ToList();

                    string branchesString = $"({string.Join("|", branches)})";

                    regexString = regexString.Substring(0, i) + regexString.Substring(i + branchesString.Length);
                    i--;
                }
                else
                {
                    current = current.Select(curr => Move(c, curr)).ToList();
                }
            }
        }

        private (int, int, int, int) GetEdges()
        {
            int minX = map.Keys.Min(c => c.x);
            int maxX = map.Keys.Max(c => c.x);
            int minY = map.Keys.Min(c => c.y);
            int maxY = map.Keys.Max(c => c.y);

            return (minX, maxX, minY, maxY);
        }

        private (int x, int y) Move(char direction, (int x, int y) pos)
        {
            (int x, int y) door = pos;
            (int x, int y) move = pos;

            switch (direction)
            {
                case 'N':
                    door = (pos.x, pos.y - 1);
                    move = (pos.x, pos.y - 2);
                    break;
                case 'W':
                    door = (pos.x - 1, pos.y);
                    move = (pos.x - 2, pos.y);
                    break;
                case 'E':
                    door = (pos.x + 1, pos.y);
                    move = (pos.x + 2, pos.y);
                    break;
                case 'S':
                    door = (pos.x, pos.y + 1);
                    move = (pos.x, pos.y + 2);
                    break;
            }

            map[door] = true;
            map[move] = false;

            return move;

        }

        private List<string> SplitRegex(string regexString)
        {
            int depth = 0;

            string tmpStr = "";
            List<string> tmp = new List<string>();

            bool first = true;
            foreach (char c in regexString)
            {
                if (c == '(') { 
                    depth++;
                }
                else if (c == ')')
                {
                    depth--;
                }

                if (c == '|' && depth == 1)
                {
                    if (first)
                        tmp.Add(tmpStr.Substring(1));
                    else
                        tmp.Add(tmpStr);

                    first = false;
                    tmpStr = "";
                    continue;
                }

                tmpStr += c;

                if (depth == 0)
                {
                    tmp.Add(tmpStr.Substring(0, tmpStr.Length - 1));
                    break;
                }
            }

            return tmp;
        }

        public HashSet<Node> FindPaths()
        {
            HashSet<(int x, int y)> visited = new HashSet<(int x, int y)>();
            HashSet<Node> distances = new HashSet<Node>();
            Queue<Node> q = new Queue<Node>();

            q.Enqueue(new Node() { Pos = (0, 0), Distance = 0}); 

            while (true)
            {
                if (q.Count == 0)
                    break;

                var current = q.Dequeue();
                visited.Add(current.Pos);

                if (!map.ContainsKey(current.Pos)) //stop searching if on wall
                    continue;

                if (!map[current.Pos])
                    distances.Add(current);


                EnqueueAllDirections(q, current, visited);

            }

            var d = distances.OrderByDescending(x => x.Distance).ToList();
            

            return distances;
        }

        private void EnqueueAllDirections(Queue<Node> q, Node current, HashSet<(int x, int y)> visited)
        {
            AddToQueue(q, current, (current.Pos.x, current.Pos.y - 1), visited);
            AddToQueue(q, current, (current.Pos.x - 1, current.Pos.y), visited);
            AddToQueue(q, current, (current.Pos.x + 1, current.Pos.y), visited);
            AddToQueue(q, current, (current.Pos.x, current.Pos.y + 1), visited);
        }

        private void AddToQueue(Queue<Node> q, Node current, (int x, int y) newPos, HashSet<(int x, int y)> visited)
        {
            if (visited.Contains(newPos) || q.Any(x => x.Pos == newPos))
                return;

            q.Enqueue(new Node
            {
                Pos = newPos,
                Distance = current.Distance + 1
            });
        }

        private void DrawMap()
        {
            (int minX, int maxX, int minY, int maxY) = GetEdges();

            int xdiff = maxX - minX + 1;
            int ydiff = maxY - minY + 1;

            StringBuilder sb = new StringBuilder();
            sb.AppendLine();
            for (int x = 0; x < xdiff + 2; x++)
                sb.Append("#");
            sb.AppendLine();

            for (int y = 0; y < ydiff; y++)
            {
                bool vertical = false;
                sb.Append('#');
                for (int x = 0; x < xdiff; x++)
                {
                    if (map.TryGetValue((x + minX, y + minY), out bool door))
                    {
                        if (door)
                        {
                            if (vertical)
                                sb.Append("|");
                            else
                                sb.Append("-");

                            vertical = false;
                        }
                        else
                        {
                            sb.Append(".");
                            vertical = true;
                        }
                    }
                    else
                        sb.Append("#");
                }

                sb.Append('#');
                sb.AppendLine();
            }

            for (int x = 0; x < xdiff + 2; x++)
                sb.Append("#");

            Console.WriteLine(sb.ToString());
        }

    }

    public class Node
    {
        public (int x, int y) Pos;
        public int Distance = 0;
    }
}
