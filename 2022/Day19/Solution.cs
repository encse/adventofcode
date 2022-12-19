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

    int MaxGeodes(
        Blueprint bluePrint,
        State state,
        Dictionary<State, int> cache
    ) {
        if (state.remainingTime == 0) {
            return state.available.geode;
        }

        if (!cache.ContainsKey(state)) {
            cache[state] = (
                from afterBuilding in BuildAll(bluePrint, state)
                let nextState = afterBuilding with {
                    remainingTime = state.remainingTime - 1,
                    available = afterBuilding.available + state.produced
                }
                select MaxGeodes(bluePrint, nextState, cache)
            ).Max();
        }

        return cache[state];
    }

    IEnumerable<State> BuildAll(Blueprint bluePrint, State state) {
        var current = state.available;
        var prev = current - state.produced;

        if (!CanBuild(bluePrint.geode, prev) && CanBuild(bluePrint.geode, current)) {
            yield return Build(bluePrint.geode, state);
            yield break;
        }

        if (!CanBuild(bluePrint.obsidian, prev) && CanBuild(bluePrint.obsidian, current)) {
            yield return Build(bluePrint.obsidian, state);
        }
        if (!CanBuild(bluePrint.clay, prev) && CanBuild(bluePrint.clay, current)) {
            yield return Build(bluePrint.clay, state);
        }
        if (!CanBuild(bluePrint.ore, prev) && CanBuild(bluePrint.ore, current)) {
            yield return Build(bluePrint.ore, state);
        }

        yield return state;
    }

    bool CanBuild(Robot robot, Material material) => material >= robot.cost;

    State Build(Robot robot, State resources) =>
        resources with {
            available = resources.available - robot.cost,
            produced = resources.produced + robot.produces
        };

    State Mine(State resources, Material miners) => 
        resources with {
            available = resources.available + miners
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
