using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skylines_Problem
{
    public static class SkylineCalculatorUtil
    {
        public static List<Tuple<int, int>> GetSkylines(int low, int high, List<Building> buildings)
        {

            List<Tuple<int, int>> skyLines = new List<Tuple<int, int>>();

            if (low > high)
            {
                return skyLines;
            }
            else if (low == high)
            {
                var building = buildings.ElementAt(low);

                Tuple<int, int> element1 = new Tuple<int, int>(building.Left, building.Height);
                Tuple<int, int> element2 = new Tuple<int, int>(building.Right, 0);
                skyLines.Add(element1);
                skyLines.Add(element2);
                return skyLines;
            }
            else
            {
                int mid = (low + high) / 2;
                List<Tuple<int, int>> skyLinesLeft = GetSkylines(low, mid, buildings);
                List<Tuple<int, int>> skyLinesRight = GetSkylines(mid + 1, high, buildings);

                return mergeSkylines(skyLinesLeft, skyLinesRight);
            }
        }

        private static List<Tuple<int, int>> mergeSkylines(List<Tuple<int, int>> left, List<Tuple<int, int>> right)
        {
            int h1 = 0, h2 = 0;

            List<Tuple<int, int>> mergedSkyLines = new List<Tuple<int, int>>();
            Tuple<int, int> previous = new Tuple<int, int>(int.MaxValue, int.MaxValue);
            Tuple<int, int> mergedStripe;
            while (true)
            {
                if (left.Count == 0 || right.Count == 0) break;

                if (left.First().Item1 < right.First().Item1)
                {
                    h1 = left.First().Item2;

                    if (h1 < h2)
                    {
                        mergedStripe = new Tuple<int, int>(left.First().Item1, h2);
                    }
                    else
                    {
                        mergedStripe = new Tuple<int, int>(left.First().Item1, left.First().Item2);
                    }
                    left.RemoveAt(0);
                }
                else if (left.First().Item1 > right.First().Item1)
                {
                    h2 = right.First().Item2;

                    if (h2 < h1)
                    {
                        mergedStripe = new Tuple<int, int>(right.First().Item1, h1);

                    }
                    else
                    {
                        mergedStripe = new Tuple<int, int>(right.First().Item1, right.First().Item2);
                    }
                    right.RemoveAt(0);
                }
                else
                {
                    mergedStripe = new Tuple<int, int>(left.First().Item1, Math.Max(left.First().Item2, right.First().Item2));
                    h1 = left.First().Item2;
                    h2 = right.First().Item2;
                    left.RemoveAt(0);
                    right.RemoveAt(0);
                }
                if (previous.Item2 != mergedStripe.Item2)
                {
                    mergedSkyLines.Add(mergedStripe);
                    previous = mergedStripe;
                }
            }
            if (left.Count == 0)
            {
                while (right.Count != 0)
                {
                    mergedSkyLines.Add(right.ElementAt(0));
                    right.RemoveAt(0);
                }
            }
            else if (right.Count == 0)
            {
                while (left.Count != 0)
                {
                    mergedSkyLines.Add(left.ElementAt(0));
                    left.RemoveAt(0);
                }
            }
            return mergedSkyLines;
        }
    }
}
