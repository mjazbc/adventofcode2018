using System;
using System.Collections.Generic;
using System.Linq;
using Util;

namespace Day07
{
    class Day07 : AdventPuzzle
    {
        protected override string SolveFirstPuzzle()
        {
            string[] lines = ReadInputArray<string>();
            Dictionary<char, HashSet<char>> conditions = CreateConditionsDict(lines);
  
            string steps = "";
            while (conditions.Any())
            {

                var current = conditions.Where(x => !x.Value.Any()).OrderBy(x => x.Key).First().Key;
                conditions.Remove(current);

                foreach (var cond in conditions.Where(x=>x.Value.Contains(current)))
                {
                    cond.Value.Remove(current);        
                }

                steps += current;
            }

            return steps;
        }

        protected override string SolveSecondPuzzle()
        {
            string[] lines = ReadInputArray<string>();
            Dictionary<char, HashSet<char>> conditions = CreateConditionsDict(lines);

            int count = 0;
            Dictionary<char, int> currentWorkers = new Dictionary<char, int>();
            while (conditions.Any())
            {
                //Take first 5 available tasks
                var currentTasks = conditions.Where(x => !x.Value.Any()).OrderBy(x => x.Key).Take(5);

                foreach (var task in currentTasks)
                {
                    if (!currentWorkers.ContainsKey(task.Key))
                    {
                        currentWorkers[task.Key] = task.Key - 5; // Set duration - alphabet starts with ascii 65
                    }
                    else
                    {
                        currentWorkers[task.Key]--; //Decrease number of seconds until completion

                        if (currentWorkers[task.Key] != 0) continue;

                        //If completed, remove task from current tasks and from conditions of other tasks
                        currentWorkers.Remove(task.Key);
                        conditions.Remove(task.Key);

                        foreach (var cond in conditions.Where(x => x.Value.Contains(task.Key)))
                        {
                            cond.Value.Remove(task.Key);
                        }
                    }
                }

                count++;
            }

            return count.ToString();
        }

        private Dictionary<char, HashSet<char>> CreateConditionsDict(string[] lines)
        {
            Dictionary<char, HashSet<char>> conditions = new Dictionary<char, HashSet<char>>();
            foreach (var line in lines)
            {
                (char condition, char key) = ParseConditions(line);
                if (!conditions.ContainsKey(key))
                    conditions.Add(key, new HashSet<char>());

                conditions[key].Add(condition);

                if (!conditions.ContainsKey(condition))
                    conditions.Add(condition, new HashSet<char>());
            }

            return conditions;
        }

        private (char, char) ParseConditions(string line)
        {
            //Step X must be finished before step Y can begin.
            var splitLine = line.Split();
            return (splitLine[1][0], splitLine[7][0]);
        }
    }
}
