using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Util;

namespace Day04
{
    class Day04 : AdventPuzzle
    {
        protected override string SolveFirstPuzzle()
        {
            var input = ReadInputArray<string>();

            var sleepSchedules = GetSleepSchedules(input);

            int mostAsleepHrs = sleepSchedules.Max(x => x.Value.SelectMany(hrs => hrs).Count());
            var mostAsleepGuard = sleepSchedules.Single(x => x.Value.SelectMany(hrs => hrs).Count() == mostAsleepHrs);

            int guardId = int.Parse(mostAsleepGuard.Key.TrimStart('#'));
            int minute = GetMaxMinute(mostAsleepGuard).Key;

            return (guardId * minute).ToString();
        }

        protected override string SolveSecondPuzzle()
        {
            var input = ReadInputArray<string>();

            var sleepSchedules = GetSleepSchedules(input);

            int maxGuardId = 0;
            int maxMinutesSlept = 0;
            int maxMinute = 0;

            foreach(var guard in sleepSchedules)
            {
                IGrouping<int, int> minute = GetMaxMinute(guard);

                int timeAsleep = minute.Count();

                if (timeAsleep > maxMinutesSlept)
                {
                    maxMinutesSlept = timeAsleep;
                    maxGuardId = int.Parse(guard.Key.TrimStart('#'));
                    maxMinute = minute.Key;
                }
            }

            return (maxGuardId * maxMinute).ToString();
        }

        private static IGrouping<int, int> GetMaxMinute(KeyValuePair<string, List<int[]>> guard)
        {
            return guard.Value
              .SelectMany(h => h)
              .GroupBy(h => h)
              .OrderByDescending(g => g.Count())
              .First();
        }


        private Dictionary<string, List<int[]>> GetSleepSchedules(string[] input)
        {
            input = input.OrderBy(x => x).ToArray();

            var sleepSchedules = new Dictionary<string, List<int[]>>();

            string currentGuardId = null;
            int fallsAsleep = 0;
            int wakesUp = 0;

            foreach (var line in input)
            {
                if (line.Length > 32)
                {
                    currentGuardId = line.Split()[3];
                    continue;
                }

                if (line.Contains("falls"))
                {
                    fallsAsleep = int.Parse(line.Substring(15, 2));
                }
                else
                {
                    wakesUp = int.Parse(line.Substring(15, 2));

                    if (!sleepSchedules.ContainsKey(currentGuardId))
                        sleepSchedules.Add(currentGuardId, new List<int[]>());

                    sleepSchedules[currentGuardId].Add(Enumerable.Range(fallsAsleep, wakesUp - fallsAsleep).ToArray());
                }
            }

            return sleepSchedules;
        }
    }
}
