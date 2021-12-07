using System;
using System.Linq;

namespace AdventOfCode.Y2021.Day07;

[ProblemName("The Treachery of Whales")]
class Solution : Solver {

    public object PartOne(string input) => 
        FuelMin(input, fuelConsumption: distance => distance);

    public object PartTwo(string input) => 
        FuelMin(input, fuelConsumption: distance => (1 + distance) * distance / 2);
        
    int FuelMin(string input, Func<int, int> fuelConsumption) {
        var positions = input.Split(",").Select(int.Parse).ToArray();

        var totalFuelToReachTarget = (int target) => 
            positions.Select(position => fuelConsumption(Math.Abs(target - position))).Sum();

        // Minimize the total fuel consumption checking each possible target position.
        // We have just about 1000 of these, so an O(n^2) algorithm will suffice.
        var minPosition = positions.Min();
        var maxPosition = positions.Max();
        return Enumerable.Range(minPosition, maxPosition - minPosition + 1).Select(totalFuelToReachTarget).Min();
    }
}
