using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text.RegularExpressions;
using System.Text;

namespace AdventOfCode.Day25 {

    class Solution : Solver {
        enum State { A, B, C, D, E, F }

        public string GetName() => "The Halting Problem";

        public IEnumerable<object> Solve(string input) {
            yield return PartOne(input);
        }

        int PartOne(string input) {
            Dictionary<int, int> tape = new Dictionary<int, int>();

            int getV(int p) {
                if (tape.ContainsKey(p)) {
                    return tape[p];
                }
                return 0;
            }
            void setV(int p, int value) {
                tape[p] = value;
            }
            var state = State.A;
            var pos = 0;
            for (int i = 0; i < 12173597; i++) {
                switch (state) {
                    case State.A:
                        if (getV(pos) == 0) {
                            setV(pos, 1);
                            pos++;
                            state = State.B;
                        } else {
                            setV(pos, 0);
                            pos--;
                            state = State.C;
                        }
                        break;
                    case State.B:
                        if (getV(pos) == 0) {
                            setV(pos, 1);
                            pos--;
                            state = State.A;
                        } else {
                            setV(pos, 1);
                            pos++;
                            state = State.D;
                        }
                        break;
                    case State.C:
                        if (getV(pos) == 0) {
                            setV(pos, 1);
                            pos++;
                            state = State.A;
                        } else {
                            setV(pos, 0);
                            pos--;
                            state = State.E;
                        }
                        break;
                    case State.D:
                        if (getV(pos) == 0) {
                            setV(pos, 1);
                            pos++;
                            state = State.A;
                        } else {
                            setV(pos, 0);
                            pos++;
                            state = State.B;
                        }
                        break;
                    case State.E:
                        if (getV(pos) == 0) {
                            setV(pos, 1);
                            pos--;
                            state = State.F;
                        } else {
                            setV(pos, 1);
                            pos--;
                            state = State.C;
                        }
                        break;
                    case State.F:
                        if (getV(pos) == 0) {
                            setV(pos, 1);
                            pos++;
                            state = State.D;
                        } else {
                            setV(pos, 1);
                            pos++;
                            state = State.A;
                        }
                        break;
                }
            }
            return tape.Select(kvp => kvp.Value).Sum();
        }
    }

    // class Parser {
    //     Machine Parse(string input) {
    //          var lines = input.Split('\n').Where(line => !string.IsNullOrEmpty(line));

    //         Machine problem = new Machine();

    //         string state;
    //         string newState;
    //         string dir;
    //         int read;
    //         int write;

    //         Literal(@"Begin in state (\w).", out problem.state).
    //         Literal(@"Perform a diagnostic checksum after (\d+) steps.", out problem.iterations).
    //         OneOrMore(
    //             Literal(@"In state (\w):", out state).
    //             OneOrMore(
    //                 Literal(@"If the current value is (\d):", out read).
    //                 Literal(@"- Write the value (\d).", out write).
    //                 Literal(@"- Move one slot to the (left|right).", out dir).
    //                 Literal(@" - Continue with state (\w).", out newState).
    //                 Then(() => problem.prg[(state, read)] = (write, dir == "left" ? -1 : 1, newState))
    //             ));

    //         return problem;
    //     }

    //     private Parser OneOrMore(Parser p) {
    //         return this;
    //     }
    //     private Parser Literal(string q, out string st) {
    //         st = null;
    //         return this;
    //     }
    //     private Parser Literal(string q, out int st) {
    //         st = 0;
    //         return this;
    //     }
    //     private Parser Then(Action cb) {
    //         cb();
    //         return this;
    //     }
    // }

    // class Machine {
    //     public string state;
    //     public int iterations;
    //     public Dictionary<(string, int), (int, int, string)> prg =
    //         new Dictionary<(string state, int r), (int w, int dir, string state)>();
    // }
}