using System.Collections.Generic;
using System;
using System.Text.RegularExpressions;
using System.Linq;

namespace AdventOfCode.Y2019.Day25 {

    class Solution : Solver {

        public string GetName() => "Cryostasis";

        public IEnumerable<object> Solve(string input) {
            yield return PartOne(input);
        }

        int PartOne(string input) {
            var icm = new IntCodeMachine(input);
            var description = icm.RunAscii();

            VisitRooms(icm, description, (_, items) => {
                foreach (var item in items) {
                    if (item != "infinite loop") {
                        var takeCmd = "take " + item + "\n";
                        var clone = icm.Clone();
                        clone.RunAscii(takeCmd);
                        if (!clone.Halted() && Inventory(clone).Contains(item)) {
                            icm.RunAscii(takeCmd);
                        }
                    }
                }
                return false;
            });

            VisitRooms(icm, description, (room, _) => {
                return room == "== Security Checkpoint ==";
            });

            Random r = new Random();
            void TakeOrDrop(string cmd, List<string> from, List<string> to) {
                var i = r.Next(from.Count);
                var item = from[i];
                from.RemoveAt(i);
                to.Add(item);
                icm.RunAscii(cmd + " " + item + "\n");
            }

            var inventory = Inventory(icm).ToList();
            var floor = new List<string>();
            while (true) {
                var output = icm.RunAscii("east\n");
                if (output.Contains("heavier")) {
                    TakeOrDrop("take", floor, inventory);
                } else if (output.Contains("lighter")) {
                    TakeOrDrop("drop", inventory, floor);
                } else {
                    return int.Parse(Regex.Match(output, @"\d+").Value);
                }
            }
        }

        void VisitRooms(IntCodeMachine icm, string description, Func<string, IEnumerable<string>, bool> callback) {
            var directions = new List<string>() { "south", "east", "west", "north" };
            var roomsSeen = new HashSet<string>();
            bool DFSDoors(string description) {
                var room = description.Split("\n").Last(x => x.Contains("=="));
                var listing = ParseLists(description).ToHashSet();
                var doors = listing.Intersect(directions);
                var items = listing.Except(doors);

                if (!roomsSeen.Contains(room)) {
                    roomsSeen.Add(room);

                    if (callback(room, items)) {
                        return true;
                    }
                    foreach (var door in doors) {
                        if (DFSDoors(icm.RunAscii(door + "\n"))) {
                            return true;
                        }
                        icm.RunAscii(directions[3 - directions.IndexOf(door)] + "\n");
                    }
                }
                return false;
            }

            DFSDoors(description);
        }

        IEnumerable<string> Inventory(IntCodeMachine icm) => ParseLists(icm.RunAscii("inv\n"));

        IEnumerable<string> ParseLists(string description) =>
            from line in description.Split("\n") 
            where line.StartsWith("- ") 
            select line.Substring(2);
    }
}