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

            var rails = input.Select(c => c.ToCharArray()).ToArray();
            HashSet<Cart> carts = new HashSet<Cart>();

            for (int y = 0; y < rails.Length; y++)
            {
                for (int x = 0; x < rails[0].Length; x++)
                {
                    if (rails[y][x] == '<' || rails[y][x] == '>' || rails[y][x] == '^' || rails[y][x] == 'v')
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

            while (true)
            {
                foreach (var cart in carts.OrderBy(c => c.y).ThenBy(c => c.x))
                {
                    cart.Move(rails);
                    Console.WriteLine(cart);
                    if (carts.Select(c => c.Position()).Count(c => c == cart.Position()) > 1)
                        return cart.x + "," + cart.y;
                }
                Console.WriteLine();

            }

                return null;

        }

        protected override string SolveSecondPuzzle()
        {
            throw new NotImplementedException();
        }
    }

    class Cart
    {
        public int x;
        public int y;
        public char direction;
        int turn = 0;

        public (int,int) Position()
        {
            return (x, y);
        }

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
