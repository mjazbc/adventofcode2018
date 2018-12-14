using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Util;

namespace Day13
{
    class Day13 : AdventPuzzle
    {
        protected override string SolveFirstPuzzle()
        {
            string[] input = ReadInputArray<string>();
            char[][] rails = rails = input.Select(c => c.ToCharArray()).ToArray();
            HashSet<Cart> carts = ParseRailsAndCarts(input, ref rails);


            while (true)
            {
                foreach (var cart in carts.OrderBy(c => c.y).ThenBy(c => c.x))
                {
                    cart.Move(rails);
                    if (carts.Count(c => c.Position == cart.Position) > 1)
                        return cart.x + "," + cart.y;
                }

            }
        }

        protected override string SolveSecondPuzzle()
        {
            string[] input = ReadInputArray<string>();
            char[][] rails = rails = input.Select(c => c.ToCharArray()).ToArray();
            HashSet<Cart> carts = ParseRailsAndCarts(input, ref rails);

            while (carts.Count > 1)
            {
                foreach (var cart in carts.OrderBy(c => c.y).ThenBy(c => c.x))
                {
                    cart.Move(rails);

                    if(carts.Count(c => c.Position == cart.Position) > 1)
                        carts.RemoveWhere(x => x.Position == cart.Position);
                     
                }
            }

            return carts.Single().Position;
        }

        private HashSet<Cart> ParseRailsAndCarts(string[] input, ref char[][] rails)
        {
            rails = input.Select(c => c.ToCharArray()).ToArray();
            var carts = new HashSet<Cart>();

            char[] directions = new char[]{ 'v', '<', '>', '^' };

            for (int y = 0; y < rails.Length; y++)
            {
                for (int x = 0; x < rails[0].Length; x++)
                {
                    if (directions.Contains(rails[y][x]))
                    {
                        carts.Add(new Cart
                        {
                            x = x,
                            y = y,
                            direction = rails[y][x]
                        });

                        if (rails[y][x] == '<' || rails[y][x] == '>')
                            rails[y][x] = '-';
                        else
                            rails[y][x] = '|';
                    }
                }
            }

            return carts;
        }
    }

    class Cart
    {
        public int x;
        public int y;
        public char direction;
        int turn = 0;

        public string Position { get { return $"{x},{y}"; } }


        public void Move(char[][] map)
        {
            if (direction == '>')
            {
                char rail = map[ y][x + 1];
                x++;

                if (rail == '\\')
                    direction = 'v';
                else if (rail == '/')
                    direction = '^';
                else if (rail == '+')
                {
                    switch (turn)
                    {
                        case 0: direction = '^'; break;
                        case 2: direction = 'v'; break;
                        default: break;
                    }

                    turn = (turn+1) % 3;
                }
            }
            else if (direction == '<')
            {
                char rail = map[ y][x + -1];
                x--;
                
                if (rail == '\\')
                    direction = '^';
                else if (rail == '/')
                    direction = 'v';
                else if (rail == '+')
                {
                    switch (turn)
                    {
                        case 0: direction = 'v'; break;
                        case 2: direction = '^'; break;
                        default: break;
                    }

                    turn = (turn+1) % 3;
                }
            }
            else if (direction == '^')
            {
                char rail = map[y -1 ][x];
                y--;

                if (rail == '\\')
                    direction = '<';
                else if (rail == '/')
                    direction = '>';
                else if (rail == '+')
                {
                    switch (turn)
                    {
                        case 0: direction = '<'; break;
                        case 2: direction = '>'; break;
                        default: break;
                    }

                    turn = (turn+1) % 3;
                }
            }
            else if (direction == 'v')
            {
                char rail = map[ y +1][x];
                y++;

                if (rail == '\\')
                    direction = '>';
                else if (rail == '/')
                    direction = '<';
                else if (rail == '+')
                {
                    switch (turn)
                    {
                        case 0: direction = '>'; break;
                        case 2: direction = '<'; break;
                        default: break;
                    }

                    turn = (turn+1) % 3;
                }
            }
            else
                throw new Exception("wtf "+ direction);

        }

        public override string ToString()
        {
            return $"{direction} [{x},{y}]";
        }
    }
}
