using System.Collections.Generic;
using System.Linq;
using System;

namespace AdventOfCode.Y2019.Day14 {


    class Reactions : Dictionary<string, ((long amount, string name) output, (long amount, string name)[] input)> { }
    class DistanceFromOre : Dictionary<string, long> { }

    class Solution : Solver {

        public string GetName() => "Space Stoichiometry";

        public IEnumerable<object> Solve(string input) {
            yield return PartOne(input);
            yield return PartTwo(input);
        }

        long PartOne(string input) => OreForFuel(input)(1);
        long PartTwo(string input) {
            var oreForFuel = OreForFuel(input);
            var hi = 1;
            var amount = 1000000000000;
            while(oreForFuel(hi) < amount){
                hi *=2;
            }
            var lo = hi /2;
            while(hi -lo > 1){
                var m = (hi + lo) /2;
                if(oreForFuel(m) > amount){
                    hi = m;
                } else {
                    lo = m;
                }
            }

            return lo;
        }

        Func<long, long> OreForFuel(string input) {
            var reactions = new Reactions();
            foreach (var line in input.Split("\n")) {
                var inout = line.Split(" => ");
                var inParts = inout[0].Split(", ").Select(ParsePart).ToArray();
                var outPart = ParsePart(inout[1]);
                reactions[outPart.name] = (outPart, inParts);
            }

            var dist = ComputeDistanceFromOre(reactions);
            var q = new SortedDictionary<long, Dictionary<string, long>>();
            void enqueue(string name, long amount){
                var d = dist[name];
                if(!q.ContainsKey(d)){
                    q[d] = new Dictionary<string, long>();
                }
                if(!q[d].ContainsKey(name)){
                    q[d][name] = 0;
                }
                q[d][name] += amount;
            }

            (string name, long amount) dequeue(){
               var d = q.Keys.Last();
               var name = q[d].Keys.First();
               var amount = q[d][name];
               q[d].Remove(name);
               if(q[d].Count == 0){
                   q.Remove(d);
               }
               return (name, amount);
            }

            return (long fuel) => {
                enqueue("FUEL", fuel);
                var res = 0L;
                while (q.Any()) {
                    var part = dequeue();
                    if (part.name == "ORE") {
                        res += part.amount;
                    } else {
                        var rule = reactions[part.name];
                        var mul = (long)Math.Ceiling((decimal)part.amount / rule.output.amount);
                        foreach (var inPart in rule.input) {
                            enqueue(inPart.name, inPart.amount * mul);
                        }
                    }
                }
                return res;
            };
        }

        DistanceFromOre ComputeDistanceFromOre(Reactions reactions) {
            var dist = new DistanceFromOre();

            long ComputeRec(string name) {
                if (!dist.ContainsKey(name)) {
                    dist[name] = reactions[name].input.Select(i => ComputeRec(i.name)).Max() + 1;
                }
                return dist[name];
            }

            dist["ORE"] = 0;

            foreach (var name in reactions.Keys) {
                ComputeRec(name);
            }
            return dist;
        }

        (long amount, string name) ParsePart(string st) {
            var parts = st.Split(" ");
            return (long.Parse(parts[0]), parts[1]);
        }

       
    }

}