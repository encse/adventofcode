using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Y2020.Day11 {

    [ProblemName("Seating System")]
    class Solution : Solver {

        public IEnumerable<object> Solve(string input) {
            yield return PartOne(input);
            yield return PartTwo(input);
        }

        int PartOne(string input) => Solve(input, 4, _ => true);
        int PartTwo(string input) => Solve(input, 5, place => place != '.');

        int Solve(string input, int occupiedLimit, Func<char, bool> placeToCheck) {
            var (crow, ccol) = (input.Split("\n").Length, input.IndexOf('\n'));

            char PlaceInDirection(char[] st, int idx, int drow, int dcol) {
                var (irow, icol) = (idx / ccol, idx % ccol);
                while (true) {
                    (irow, icol) = (irow + drow, icol + dcol);
                    var place =
                        irow < 0 || irow >= crow ? 'L' :
                        icol < 0 || icol >= ccol ? 'L' :
                        st[irow * ccol + icol];
                    if (placeToCheck(place)) {
                        return place;
                    }
                }
            }

            int OccupiedPlacesAround(char[] st, int idx) {
                var directions = new[] { (0, -1), (0, 1), (-1, 0), (1, 0), (-1, -1), (-1, 1), (1, -1), (1, 1) };
                var occupied = 0;
                foreach (var (drow, dcol) in directions) {
                    if (PlaceInDirection(st, idx, drow, dcol) == '#') {
                        occupied++;
                    }
                }
                return occupied;
            }

            var prevState = new char[0];
            var state = input.Replace("\n", "").Replace("L", "#").ToArray();
            while (!prevState.SequenceEqual(state)) {
                prevState = state;
                state = state.Select((place, i) =>
                    place == '#' && OccupiedPlacesAround(state, i) >= occupiedLimit ? 'L' :
                    place == 'L' && OccupiedPlacesAround(state, i) == 0             ? '#' :
                    place /*otherwise*/
                ).ToArray();
            }
            return state.Count(place => place == '#');
        }
    }
}