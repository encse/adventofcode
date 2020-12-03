using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Y2018.Day08 {

    [ProblemName("Memory Maneuver")]
    class Solution : Solver {

        public IEnumerable<object> Solve(string input) {
            yield return PartOne(input);
            yield return PartTwo(input);
        }

        int PartOne(string input) =>
            Parse(input).fold(0, (cur, node) => cur + node.metadata.Sum());
        

        int PartTwo(string input) {
            return Parse(input).value();
        }

        Node Parse(string input) {
            var nums = input.Split(" ").Select(int.Parse).GetEnumerator();
            Func<int> next = () => {
                nums.MoveNext();
                return nums.Current;
            };

            Func<Node> read = null;
            read = () => {
                var node = new Node() {
                    children = new Node[next()],
                    metadata = new int[next()]
                };
                for (var i = 0; i < node.children.Length; i++) {
                    node.children[i] = read();
                }
                for (var i = 0; i < node.metadata.Length; i++) {
                    node.metadata[i] = next();
                }
                return node;
            };
            return read();
        }


    }

    class Node {
        public Node[] children;
        public int[] metadata;
        public T fold<T>(T seed, Func<T, Node, T> aggregate) {
            return children.Aggregate(aggregate(seed, this), (cur, child) => child.fold(cur, aggregate));
        }

        public int value() {
            if(children.Length == 0){
                return metadata.Sum();
            }

            var res = 0;
            foreach(var i in metadata){
                if(i >= 1 && i <= children.Length){
                    res += children[i-1].value();
                }
            }
            return res;
        }
    }
}