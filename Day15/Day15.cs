using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Util;

namespace Day15
{
    class Day15 : AdventPuzzle
    {
        protected override string SolveFirstPuzzle()
        {
            string[] input = ReadInputArray<string>();
            char[][] map = input.Select(c => c.ToCharArray()).ToArray();

            var units = FindUnits(ref map);

            int rounds =  SimulateBattle(map, units, breakOnElfDeath: false);
            Print(map, units);

            return (rounds*units.Sum(x => x.Hp)).ToString();

        }

        protected override string SolveSecondPuzzle()
        {
            string[] input = ReadInputArray<string>();

            for(int elfAp = 4; ; elfAp++)
            {
                char[][] map = input.Select(c => c.ToCharArray()).ToArray();

                var units = FindUnits(ref map, elfAp);
                int elves = units.Count(x => x.TypeChar == 'E');

                int rounds = SimulateBattle(map, units, breakOnElfDeath: true);

                if (units.Count(x => x.TypeChar == 'E') == elves)
                {
                    Print(map, units);
                    return (rounds * units.Sum(x => x.Hp)).ToString();
                }
            }   
        }

        private HashSet<Unit> FindUnits(ref char[][] map, int elfPower = 3)
        {
            HashSet<Unit> units = new HashSet<Unit>();
            for (int y = 0; y < map[0].Length; y++) 
                for (int x = 0; x < map.Length; x++)
                    if (map[y][x] == 'G' || map[y][x] == 'E')
                    {
                        units.Add(new Unit
                        {
                            X = x,
                            Y = y,
                            TypeChar = map[y][x],
                            Ap = map[y][x] == 'E' ? elfPower : 3
                            
                        });

                        map[y][x] = '.';
                    }

            return units;
        }

        private void Print(char[][] map, HashSet<Unit> units)
        {
            for (int y = 0; y < map[0].Length; y++) 
            {
                for (int x = 0; x < map.Length; x++)
                {
                    var tmp = units.FirstOrDefault(u => u.Position == (x, y));
                    char c = map[y][x];

                    if (tmp != null)
                    {
                        if(tmp.TypeChar == 'G')
                            Console.ForegroundColor = ConsoleColor.Red;
                        else
                            Console.ForegroundColor = ConsoleColor.Green;

                        c = tmp.TypeChar;
                    }
                     
                    Console.Write(c);
                    Console.ResetColor();
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }


        private int SimulateBattle(char[][] map, HashSet<Unit> units, bool breakOnElfDeath)
        {
            for(int round = 1; ;round++)
            {
                //Order units in reading order
                foreach (Unit u in units.OrderBy(u => u.Y).ThenBy(u => u.X))
                {
                    //Skip unit if its dead
                    if (u.Hp <= 0)
                        continue;

                    //Check if there are still both types of creatures alive
                    if (units.Select(un => un.TypeChar).Distinct().Count() < 2)
                        return round - 1; 

                    //Find ajecant enemy
                    var enemy = u.FindTarget(units);

                    //If enemy is not found, move and try again
                    if (enemy == null)
                    {
                        u.MoveToClosest(map, units);
                        enemy = u.FindTarget(units);
                    }

                    //If enemy found, attack and remove if enemy is killed
                    if (enemy != null)
                    {
                        enemy.Hp -= u.Ap;

                        if (enemy.Hp <= 0) { 
                            units.Remove(enemy);

                            //To speed up part 2
                            if (enemy.TypeChar == 'E' && breakOnElfDeath)
                                return round - 1;
                        }
                    }
                }
            }
        }
    }

    class Unit
    {
        public int X;
        public int Y;
        public char TypeChar;

        public int Ap = 3;
        public int Hp = 200;

        public (int x,int y) Position { get { return (X, Y); } }

        public (int x, int y) Top   { get { return (X, Y - 1); } }
        public (int x, int y) Left  { get { return (X - 1, Y); } }
        public (int x, int y) Right { get { return (X + 1, Y ); } }
        public (int x, int y) Bottom { get { return (X, Y + 1); } }

        /// <summary>
        /// Finds adjecant enemy with lowest hp.
        /// </summary>
        public Unit FindTarget(HashSet<Unit> units)
        {
            return units.Where(u => u.TypeChar != TypeChar && ( u.Position == Top || u.Position == Left || u.Position == Right || u.Position == Bottom))
                .OrderBy(u=>u.Hp).ThenBy(u => u.Y).ThenBy(u => u.X).FirstOrDefault();
        }

        /// <summary>
        /// Implements breadth-first search to find nearest enemies. If multiple enemies are found at same distance, unit moves to first in reading order.
        /// </summary>
        public void MoveToClosest(char[][] map, HashSet<Unit> units)
        {
            Queue<Node> q = new Queue<Node>();
            HashSet<(int x, int y)> visited = new HashSet<(int x, int y)>();
            HashSet<Node> enemiesAtDistance = new HashSet<Node>();

            q.Enqueue(new Node
            {
                Pos = Position
            });

            visited.Add(Position);

            while (true)
            {
                if (q.Count == 0) 
                    break;

                var current = q.Dequeue();

                visited.Add(current.Pos);

                if (EnemyFound(units, current.Pos))
                {
                    //Break when all enemies in closest range are found
                    if (enemiesAtDistance.Any() && current.Distance > enemiesAtDistance.Max(e => e.Distance)) 
                        break;

                    enemiesAtDistance.Add(current);
                }

                if (current.Pos != Position && !IsValid(current.Pos, map, units))
                    continue;

                EnqueueAllDirections(q, current, visited);

            }

            var enemy = enemiesAtDistance.OrderBy(e => e.Pos.y).ThenBy(e => e.Pos.x).FirstOrDefault();

            if (enemy == null)
                return;

            //Backtrack to find first step and move unit
            while (enemy.PreviousNode.PreviousNode != null)
                enemy = enemy.PreviousNode;

            X = enemy.Pos.x;
            Y = enemy.Pos.y;
        }

        /// <summary>
        /// Enqueue all directions from current position.
        /// </summary>
        private  void EnqueueAllDirections(Queue<Node> q, Node current, HashSet<(int x, int y)> visited)
        {
            AddToQueue(q, current, (current.Pos.x, current.Pos.y - 1), visited);
            AddToQueue(q, current, (current.Pos.x - 1, current.Pos.y ), visited);
            AddToQueue(q, current, (current.Pos.x +1, current.Pos.y), visited);
            AddToQueue(q, current, (current.Pos.x, current.Pos.y + 1), visited);
        }

        /// <summary>
        /// Adds position to search queue if it has not been visited already and if it is not already in the queue.
        /// </summary>
        private void AddToQueue(Queue<Node> q, Node current, (int x, int y) newPos, HashSet<(int x, int y)> visited)
        {
            if (visited.Contains(newPos) || q.Any(x=>x.Pos == newPos))
                return;

            q.Enqueue(new Node
            {
                Pos = newPos,
                PreviousNode = current,
                Distance = current.Distance + 1
            });

        }

        /// <summary>
        /// Checks if move is in range of map and if there are no obstacles.
        /// </summary>
        private bool IsValid((int x,int y) position, char[][] map, HashSet<Unit> units)
        {
            //Check edges
            if (position.x < 0 || position.y < 0 || position.x >= map[0].Length || position.y >= map.Length)
                return false;

            //Check that no other unit occupies this space and there is no wall
            return !units.Any(x => x.Position == position) && map[position.y][position.x] == '.';
        }

        /// <summary>
        /// Checks if unit in position is of opposite type.
        /// </summary>
        private bool EnemyFound(HashSet<Unit> units, (int x, int y) pos) {
            char? currentChar = units.FirstOrDefault(u => u.Position == pos)?.TypeChar;
            return currentChar.HasValue && currentChar != this.TypeChar;
        }

        public override string ToString()
        {
            return TypeChar +" "+ Position + " HP: " + Hp;
        }
    }

    public class Node
    {
        public Node PreviousNode;
        public (int x, int y) Pos;
        public int Distance = 0;
    }
}
