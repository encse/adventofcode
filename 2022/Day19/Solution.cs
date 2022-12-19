using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode.Y2022.Day19;

[ProblemName("Not Enough Minerals")]
class Solution : Solver {

    public object PartOne(string input) {
        var res = 0;
        foreach(var blueprint in Parse(input)) {
            res += blueprint.id *  MaxGeodes(blueprint, 24);
        }
        return res;
    }

    public object PartTwo(string input) {
        var res = 1;
        foreach(var blueprint in Parse(input).Where(bp => bp.id <= 3)) {
            res *= MaxGeodes(blueprint, 32);
        }
        return res;
    }

    private int MaxGeodes(Blueprint blueprint, int timeLimit) {
        return MaxGeodes(
            blueprint, 
            new State(
                remainingTime: timeLimit,
                available: new Material(ore: 0, 0, 0, 0),
                produced: new Material(ore: 1, 0, 0, 0)
            ),
            new Dictionary<State, int>()
        );
    }

    // Returns the maximum mineable geodes under the given state constraints,
    // Recursion with a cache.
    int MaxGeodes(Blueprint bluePrint, State state, Dictionary<State, int> cache) {
        if (state.remainingTime == 0) {
            return state.available.geode;
        }

        if (!cache.ContainsKey(state)) {
            cache[state] = (
                from afterFactory in NextSteps(bluePrint, state)
                let afterMining = afterFactory with {
                    remainingTime = state.remainingTime - 1,
                    available = afterFactory.available + state.produced
                }
                select MaxGeodes(bluePrint, afterMining, cache)
            ).Max();
        }

        return cache[state];
    }

    // Returns all possible factory steps
    IEnumerable<State> NextSteps(Blueprint bluePrint, State state) {
        var now = state.available;
        var prev = now - state.produced;

        // The !canBuild(X, prev) && canBuild(X, now) conditions are tricky.
        // We consider building a miner only if we couldn't build it in the previous step
        // otherwise we would introduce combinatorical explosion with a factor of 2 with
        // doing nothing in this phase (yield return state) and reconsidering the building
        // of the same miner in the next recursion step.

        if (!CanBuild(bluePrint.geode, prev) && CanBuild(bluePrint.geode, now)) {
            yield return Build(state, bluePrint.geode);
            // Building a geode miner asap seems to be an optimal choice, no 
            // need to try anything else.
            yield break;
        }

        if (!CanBuild(bluePrint.obsidian, prev) && CanBuild(bluePrint.obsidian, now)) {
            yield return Build(state, bluePrint.obsidian);
        }
        if (!CanBuild(bluePrint.clay, prev) && CanBuild(bluePrint.clay, now)) {
            yield return Build(state, bluePrint.clay);
        }
        if (!CanBuild(bluePrint.ore, prev) && CanBuild(bluePrint.ore, now)) {
            yield return Build(state, bluePrint.ore);
        }

        yield return state;
    }

    bool CanBuild(Robot robot, Material availableMaterial) => availableMaterial >= robot.cost;

    State Build(State state, Robot robot) =>
        state with {
            available = state.available - robot.cost,
            produced = state.produced + robot.produces
        };

    State Mine(State state, Material miners) => 
        state with {
            available = state.available + miners
        };

    IEnumerable<Blueprint> Parse(string input) {
        foreach (var line in input.Split("\n")) {
            var numbers = Regex.Matches(line, @"(\d+)").Select(x => int.Parse(x.Value)).ToArray();
            yield return new Blueprint(
                id: numbers[0],
                ore: new Robot(
                    cost: new Material(ore: numbers[1], clay: 0, obsidian: 0, geode: 0),
                    produces: new Material(ore: 1, clay: 0, obsidian: 0, geode: 0)
                ),
                clay: new Robot(
                    cost: new Material(ore: numbers[2], clay: 0, obsidian: 0, geode: 0),
                    produces: new Material(ore: 0, clay: 1, obsidian: 0, geode: 0)
                ),
                obsidian: new Robot(
                    cost: new Material(ore: numbers[3], clay: numbers[4], obsidian: 0, geode: 0),
                    produces: new Material(ore: 0, clay: 0, obsidian: 1, geode: 0)
                ),
                geode: new Robot(
                    cost: new Material(ore: numbers[5], clay: 0, obsidian: numbers[6], geode: 0),
                    produces: new Material(ore: 0, clay: 0, obsidian: 0, geode: 1)
                )
            );
        }
    }
    record Material(int ore, int clay, int obsidian, int geode) {
        public static Material operator +(Material a, Material b) {
            return new Material(
                a.ore + b.ore,
                a.clay + b.clay,
                a.obsidian + b.obsidian,
                a.geode + b.geode
            );
        }

        public static Material operator -(Material a, Material b) {
            return new Material(
                a.ore - b.ore,
                a.clay - b.clay,
                a.obsidian - b.obsidian,
                a.geode - b.geode
            );
        }

        public static bool operator <=(Material a, Material b) {
            return
                a.ore <= b.ore &&
                a.clay <= b.clay &&
                a.obsidian <= b.obsidian &&
                a.geode <= b.geode;
        }

        public static bool operator >=(Material a, Material b) {
            return
                a.ore >= b.ore &&
                a.clay >= b.clay &&
                a.obsidian >= b.obsidian &&
                a.geode >= b.geode;
        }
    }

    record Robot(Material cost, Material produces);
    record State(int remainingTime, Material available, Material produced);
    record Blueprint(int id, Robot ore, Robot clay, Robot obsidian, Robot geode);
}
