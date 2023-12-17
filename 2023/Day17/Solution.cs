using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace AdventOfCode.Y2023.Day17;
using Map = Dictionary<Complex, int>;
using State1 = (Complex pos, Complex dir, int straight);
using State2 = (Complex pos, Complex dir, int straight);

[ProblemName("Clumsy Crucible")]
class Solution : Solver {

    static readonly Complex Up = -Complex.ImaginaryOne;
    static readonly Complex Down = Complex.ImaginaryOne;
    static readonly Complex Left = -Complex.One;
    static readonly Complex Right = Complex.One;

    public object PartOne(string input) =>
        Heatloss1(ParseMap(input), (Complex.Zero, Right, 0));

    public object PartTwo(string input) => 
        Heatloss2(ParseMap(input), (Complex.Zero, Right, 0));

    int Heatloss1(Map map, State1 state) {
        var heatloss = 0;

        var br = map.Keys.MaxBy(pos => pos.Imaginary + pos.Real);
        var q = new PriorityQueue<State1, int>();

        q.Enqueue(state, heatloss);
        var seen = new HashSet<State1>();
        while (q.TryDequeue(out state, out heatloss)) {
            if (state.pos == br) {
                break;
            }
            foreach (var stateT in Exits(state)) {
                if (map.ContainsKey(stateT.pos) && !seen.Contains(stateT)) {
                    seen.Add(stateT);
                    var h = heatloss + map[stateT.pos];
                    q.Enqueue(stateT, h);
                }
            }
        }

        return heatloss;
    }

    IEnumerable<State1> Exits(State1 state) {
        if (state.straight < 3) {
            yield return state with { pos = state.pos + state.dir, straight = state.straight + 1 };
        }

        var dir = state.dir * Complex.ImaginaryOne;
        yield return state with { pos = state.pos + dir, dir = dir, straight = 1 };
        dir = -dir;
        yield return state with { pos = state.pos + dir, dir = dir, straight = 1 };
    }

    int Heatloss2(Map map, State2 state) {
        var heatloss = 0;

        var br = map.Keys.MaxBy(pos => pos.Imaginary + pos.Real);
        var q = new PriorityQueue<State1, int>();

        q.Enqueue(state, heatloss);
        var seen = new HashSet<State1>();
        while (q.TryDequeue(out state, out heatloss)) {
            if (state.pos == br && state.straight % 4 == 0) {
                break;
            }
            foreach (var stateT in Exits2(state)) {
                if (map.ContainsKey(stateT.pos) && !seen.Contains(stateT)) {
                    seen.Add(stateT);
                    var h = heatloss + map[stateT.pos];
                    // Console.WriteLine(state.pos + " -> " + stateT.pos + " " + stateT.straight + "    " + h);
                    q.Enqueue(stateT, h);
                }
            }
        }

        return heatloss;
    }

    IEnumerable<State1> Exits2(State1 state) {
        if (state.straight > 0 && state.straight < 4) {
            yield return state with { pos = state.pos + state.dir, straight = state.straight + 1 };
            yield break;
        } 
        
        if (state.straight < 10) {
            yield return state with { pos = state.pos + state.dir, straight = state.straight + 1 };
        }

        var dir = state.dir * Complex.ImaginaryOne;
        yield return state with { pos = state.pos + dir, dir = dir, straight = 1 };
        dir = -dir;
        yield return state with { pos = state.pos + dir, dir = dir, straight = 1 };
    }


    // using a dictionary helps with bounds check (simply containskey):
    Map ParseMap(string input) {
        var lines = input.Split('\n');
        return (
            from irow in Enumerable.Range(0, lines.Length)
            from icol in Enumerable.Range(0, lines[0].Length)
            let cell = int.Parse(lines[irow].Substring(icol, 1))
            let pos = new Complex(icol, irow)
            select new KeyValuePair<Complex, int>(pos, cell)
        ).ToDictionary();
    }


}