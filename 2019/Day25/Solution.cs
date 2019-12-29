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

        List<string> directions = new List<string>() { "south", "east", "west", "north" };
        string ReverseDir(string direction) => directions[3 - directions.IndexOf(direction)];

        long PartOne(string input) {
            var securityRoom = "== Security Checkpoint ==";
            var icm = new IntCodeMachine(input);
            var description = icm.RunAscii();

            VisitRooms(securityRoom, icm, description, args => {
                foreach (var item in args.items) {
                    if (item != "infinite loop") {
                        var takeCmd = "take " + item;
                        var clone = icm.Clone();
                        clone.RunAscii(takeCmd);
                        if (!clone.Halted() && Inventory(clone).Contains(item)) {
                            icm.RunAscii(takeCmd);
                        }
                    }
                }
                return null as string;
            });

            var door = VisitRooms(securityRoom, icm, description, args =>
               args.room == securityRoom ? args.doors.Single(door => door != ReverseDir(args.doorTaken)) : null);
            
            Random r = new Random();
            void TakeOrDrop(string cmd, List<string> from, List<string> to) {
                var i = r.Next(from.Count);
                var item = from[i];
                from.RemoveAt(i);
                to.Add(item);
                icm.RunAscii(cmd + " " + item);
            }

            var inventory = Inventory(icm).ToList();
            var floor = new List<string>();
            while (true) {
                var output = icm.RunAscii(door);
                if (output.Contains("heavier")) {
                    TakeOrDrop("take", floor, inventory);
                } else if (output.Contains("lighter")) {
                    TakeOrDrop("drop", inventory, floor);
                } else {
                    return long.Parse(Regex.Match(output, @"\d+").Value);
                }
            }
        }

        T VisitRooms<T>(
            string securityRoom,
            IntCodeMachine icm, 
            string description, 
            Func<(IEnumerable<string> items, string room, string doorTaken, IEnumerable<string> doors), T> callback
        ) {

            var roomsSeen = new HashSet<string>();
            T DFS(string description, string doorTaken) {
                var room = description.Split("\n").Single(x => x.Contains("=="));
                var listing = TakeListItems(description).ToHashSet();
                var doors = listing.Intersect(directions);
                var items = listing.Except(doors);

                if (!roomsSeen.Contains(room)) {
                    roomsSeen.Add(room);

                    var res = callback((items, room, doorTaken, doors));
                    if (res != null) {
                        return res;
                    }
                    if (room != securityRoom) {
                        foreach (var door in doors) {
                            res = DFS(icm.RunAscii(door), door);
                            if (res != null) {
                                return res;
                            }
                            icm.RunAscii(directions[3 - directions.IndexOf(door)]);
                        }
                    }
                }
                return default(T);
            }

            return DFS(description, null);
        }

        IEnumerable<string> Inventory(IntCodeMachine icm) => TakeListItems(icm.RunAscii("inv"));

        IEnumerable<string> TakeListItems(string description) =>
            from line in description.Split("\n")
            where line.StartsWith("- ")
            select line.Substring(2);
    }
}