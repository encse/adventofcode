using System;
using System.Linq;

namespace AdventOfCode.Y2021.Day07;

[ProblemName("The Treachery of Whales")]
class Solution : Solver {

    public object PartOne(string input) => 
        MinimalTotalFuel(input, fuelFromDistance: distance => distance);

    public object PartTwo(string input) => 
        MinimalTotalFuel(input, fuelFromDistance: distance => (1 + distance) * distance / 2);
        
    int MinimalTotalFuel(string input, Func<int, int> fuelFromDistance) {
        var positions = input.Split(",").Select(int.Parse).ToArray();

        // Compute the total fuel consumption for each possible target position.
        // We have just about 1000 of these, so an O(n^2) algorithm will suffice.
        var minPosition = positions.Min();
        var maxPosition = positions.Max();

        var totalFuelByTarget =
            from target in Enumerable.Range(minPosition, maxPosition - minPosition + 1)
            let fuelByPosition = 
                from position in positions select fuelFromDistance(Math.Abs(target - position))
            select 
                fuelByPosition.Sum();

        // Get minimum:
        return totalFuelByTarget.Min();
    }
}
