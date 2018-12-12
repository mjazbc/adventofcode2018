using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Util;

namespace Day11
{
    class Day11 : AdventPuzzle
    {
        protected override string SolveFirstPuzzle()
        {
            int serialNum = ReadInputText<int>();
            int[,] grid = FillGrid(serialNum);
            (int x, int y, int n) = GetMaxSumPosition(grid, Enumerable.Range(3, 1).ToArray());
            return $"{x},{y}";
        }

        protected override string SolveSecondPuzzle()
        {
            int serialNum = ReadInputText<int>();
            int[,] grid = FillGrid(serialNum);
            (int x, int y, int n) = GetMaxSumPosition(grid, Enumerable.Range(1, 300).ToArray());
            return $"{x},{y},{n}";
        }

        private int CalcPowerLevel(int serialNum, int x, int y)
        {
            int rackId = x + 10;
            int powerLevel = rackId * y;
            powerLevel += serialNum;
            powerLevel = powerLevel * rackId;

            return int.Parse((powerLevel / 100).ToString().Last().ToString()) -5;
        }

        private int CalcSquareSum(int[,] grid, int x, int y, int windowSize)
        {
            int sum = 0;

            for (int i = 0; i < windowSize; i++)
                for (int j = 0; j < windowSize; j++)
                    sum += grid[x + i, y + j];

            return sum;
        }

        private (int,int,int) GetMaxSumPosition(int[,] grid, int[] squareSizes)
        {
            int maxSumX = 0;
            int maxSumY = 0;
            int maxSize = 0;
            int maxSum = 0;

            foreach(int n in squareSizes)
            {
                int prevSum = maxSum;

                for (int x = 0; x < 300 - n; x++)
                {
                    for (int y = 0; y < 300 - n; y++)
                    {
                        int sum = CalcSquareSum(grid, x, y, n);

                        if (sum > maxSum)
                        {
                            maxSumX = x;
                            maxSumY = y;
                            maxSize = n;
                            maxSum = sum;
                        }  
                    }         
                }

                if (maxSum <= prevSum)
                    break;
            }

            return (maxSumX + 1, maxSumY + 1, maxSize);
        }

        private int[,] FillGrid(int serialNum)
        {
            int[,] grid = new int[300, 300];

            for (int x = 0; x < 300; x++)
                for (int y = 0; y < 300; y++)
                    grid[x, y] = CalcPowerLevel(serialNum, x + 1, y + 1);
            return grid;
        }
    }
}
