using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace AdventOfCode.Y2016.Day11;

enum Element {
    Thulium    = 0b1,
    Plutonium  = 0b10,
    Strontium  = 0b100,
    Promethium = 0b1000,
    Ruthenium  = 0b10000,
    Elerium    = 0b100000,
    Dilithium  = 0b1000000
}

[ProblemName("Radioisotope Thermoelectric Generators")]
class Solution : Solver {

    public object PartOne(string input) => Solve(Parse(input));
    public object PartTwo(string input) => Solve(Parse(input)
        .AddGenerator(0, Element.Elerium).AddChip(0, Element.Elerium)
        .AddGenerator(0, Element.Dilithium).AddChip(0, Element.Dilithium)
        );

    int Solve(ulong state){
        var steps = 0;
        var seen = new HashSet<ulong>();
        var q = new Queue<(int steps, ulong state)>();
        q.Enqueue((0, state));
        while (q.Any()) {
            (steps, state) = q.Dequeue();

            if (state.Final()) {
                return steps;
            }

            foreach(var nextState in state.NextStates()){
                if(!seen.Contains(nextState)){
                    q.Enqueue((steps + 1, nextState));
                    seen.Add(nextState);
                }
            }
        }
        return 0;
    }
    ulong Parse(string input) {

        var nextMask = 1;
        var elementToMask = new Dictionary<string, int>();
        int mask(string element) {
            if (!elementToMask.ContainsKey(element)) {
                if (elementToMask.Count() == 5) {
                    throw new NotImplementedException();
                }
                elementToMask[element] = nextMask;
                nextMask <<= 1;
            }
            return elementToMask[element];
        }

        ulong state = 0;
        var floor = 0;
        foreach(var line in input.Split('\n')){
            var chips = (from m in Regex.Matches(line, @"(\w+)-compatible")
                         let element = m.Groups[1].Value
                         select mask(element)).Sum();

            var generators = (from m in Regex.Matches(line, @"(\w+) generator")
                         let element = m.Groups[1].Value
                         select mask(element)).Sum();
            state = state.SetFloor((ulong)floor, (ulong)chips, (ulong)generators);
            floor++;
        }
        return state;
    }
}

static class StateExtensions {
    const int elementCount = 7;
    const int elevatorShift = 8 * elementCount;
    const int generatorShift = 0;

    static int[] floorShift = new int[] { 0, 2 * elementCount, 4 * elementCount, 6 * elementCount };
   
    const ulong elevatorMask = 0b00111111111111111111111111111111111111111111111111111111;
    const ulong chipMask = 0b00000001111111;
    const ulong generatorMask = 0b11111110000000;

    static ulong[] floorMask = new ulong[]{
        0b1111111111111111111111111111111111111111111100000000000000,
        0b1111111111111111111111111111110000000000000011111111111111,
        0b1111111111111111000000000000001111111111111111111111111111,
        0b1100000000000000111111111111111111111111111111111111111111
    };
    
    public static ulong SetFloor(this ulong state, ulong floor, ulong chips, ulong generators) =>
         (state & floorMask[floor]) | 
         (((chips << elementCount) | (generators << generatorShift)) << floorShift[floor]);

    public static ulong GetElevator(this ulong state) => 
        (ulong)(state >> elevatorShift);

    public static ulong SetElevator(this ulong state, ulong elevator) => 
        (state & elevatorMask) | ((ulong)elevator << elevatorShift);

    public static ulong GetChips(this ulong state, ulong floor) =>
        (ulong)(((state & ~floorMask[floor]) >> floorShift[floor]) & ~chipMask) >> elementCount;
    
    public static ulong GetGenerators(this ulong state, ulong floor) =>
        (ulong)(((state & ~floorMask[floor]) >> floorShift[floor]) & ~generatorMask) >> generatorShift;

    public static ulong AddChip(this ulong state, ulong floor, Element chip) =>
        state | (((ulong)chip << elementCount) << floorShift[floor]);

    public static ulong AddGenerator(this ulong state, ulong floor, Element genetator) =>
        state | (((ulong)genetator << generatorShift) << floorShift[floor]);

    public static bool Valid(this ulong state) {
        for (int floor = 3; floor >= 0; floor--) {
            var chips = state.GetChips((ulong)floor);
            var generators = state.GetGenerators((ulong)floor);
            var pairs = chips & generators;
            var unpairedChips = chips & ~pairs;
            if (unpairedChips != 0 && generators != 0) {
                return false;
            }
        }
        return true;
    }

    public static IEnumerable<ulong> NextStates(this ulong state) {
        var floor = state.GetElevator();
        for (ulong i = 1; i < 0b100000000000000; i <<= 1) {
            for (ulong j = 1; j < 0b100000000000000; j <<= 1) {
                var iOnFloor = i << floorShift[floor];
                var jOnFloor = j << floorShift[floor];
                if ((state & iOnFloor) != 0 && (state & jOnFloor) != 0) {
                    if (floor > 0) {
                        var iOnPrevFloor = i << floorShift[floor - 1];
                        var jOnPrevFloor = j << floorShift[floor - 1];
                        var elevatorOnPrevFloor = (floor - 1) << elevatorShift;
                        var stateNext = (state & ~iOnFloor & ~jOnFloor & elevatorMask) | iOnPrevFloor | jOnPrevFloor | elevatorOnPrevFloor;
                        if (stateNext.Valid())
                            yield return stateNext;
                    }

                    if (floor < 3) {
                        var iOnNextFloor = i << floorShift[floor + 1];
                        var jOnNextFloor = j << floorShift[floor + 1];
                        var elevatorOnNextFloor = (floor + 1) << elevatorShift;
                        var stateNext = (state & ~iOnFloor & ~jOnFloor & elevatorMask) | iOnNextFloor | jOnNextFloor | elevatorOnNextFloor;
                        if (stateNext.Valid())
                            yield return stateNext;
                    }
                }
            }
        }
    }

    public static bool Final(this ulong state) =>
        (state & 0b0000000000000000111111111111111111111111111111111111111111) == 0;

    public static string Tsto(this ulong state){
        var sb = new StringBuilder();
        for (int floor = 3; floor >= 0; floor --){
            var e = state.GetElevator() == (ulong)floor ? "E" : " ";
            var chips = state.GetChips((ulong)floor);
            var generators =state.GetGenerators((ulong)floor);

            sb.Append($"F{(floor + 1)} {e} |");
            for (int i = 0; i < elementCount;i++){
                sb.Append((generators & 1) == 1 ? " #" : " .");
                sb.Append((chips & 1) == 1 ? " #" : " .");
                sb.Append(" |");
                chips >>= 1;
                generators >>= 1;
            }
            sb.AppendLine();
        }
        return sb.ToString();
    }
}
