using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Y2021.Day01;

[ProblemName("Sonar Sweep")]
class Solution : Solver {

    public object PartOne(string input) => DepthIncrease(Numbers(input));

    public object PartTwo(string input) => DepthIncrease(ThreeMeasurements(Numbers(input)));

    int DepthIncrease(IEnumerable<int> ns) => (
        from p in Enumerable.Zip(ns, ns.Skip(1)) 
        where p.First < p.Second 
        select 1
    ).Count();

    // the sum of elements in a sliding window of 3
    IEnumerable<int> ThreeMeasurements(IEnumerable<int> ns) => 
        from t in Enumerable.Zip(ns, ns.Skip(1), ns.Skip(2)) // .Net 6 comes with three way zip
        select t.First + t.Second + t.Third;

    // parse input to array of numbers
    IEnumerable<int> Numbers(string input) => 
        from n in input.Split('\n') 
        select int.Parse(n);
}
