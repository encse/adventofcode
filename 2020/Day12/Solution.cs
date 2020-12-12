using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text.RegularExpressions;
using System.Text;

namespace AdventOfCode.Y2020.Day12 {

    [ProblemName("Rain Risk")]
    class Solution : Solver {

        public IEnumerable<object> Solve(string input) {
            yield return PartOne(input);
            yield return PartTwo(input);
        }

        int PartOne(string input) {
            var d = (x: 1, y: 0);
            var p = (x: 0, y: 0);
            foreach (var line in input.Split("\n")) {
                var ch = line[0];
                var arg = int.Parse(line.Substring(1));
                switch (ch) {
                    case 'N':
                        p = (p.x, p.y + arg);
                        break;
                    case 'S':
                        p = (p.x, p.y - arg);
                        break;
                    case 'E':
                        p = (p.x + arg, p.y);
                        break;
                    case 'W':
                        p = (p.x - arg, p.y);
                        break;
                    case 'F':
                        p = (p.x + d.x * arg, p.y + d.y * arg);
                        break;
                    case 'R':
                        switch (arg) {
                            case 90:
                                d = (d.y, -d.x);
                                break;
                            case 180:
                                d = (-d.x, -d.y);
                                break;
                            case 270:
                                d = (-d.y, d.x);
                                break;
                            default:
                                throw new Exception();
                        }
                        break;
                    case 'L':
                        switch (arg) {
                            case 90:
                                d = (-d.y, d.x);
                                break;
                            case 180:
                                d = (-d.x, -d.y);
                                break;
                            case 270:
                                d = (d.y, -d.x);
                                break;
                            default:
                                throw new Exception();
                        }
                        break;
                    default: 
                        throw new Exception();
                }
            }
            return Math.Abs(p.x) + Math.Abs(p.y);
        }

        int PartTwo(string input) {
            var p = (x: 0, y: 0);
            var wp = (x: 10, y: 1);
            foreach (var line in input.Split("\n")) {
                var ch = line[0];
                var arg = int.Parse(line.Substring(1));
                switch (ch) {
                    case 'N':
                        wp = (wp.x, wp.y + arg);
                        break;
                    case 'S':
                        wp = (wp.x, wp.y - arg);
                        break;
                    case 'E':
                        wp = (wp.x + arg, wp.y);
                        break;
                    case 'W':
                        wp = (wp.x - arg, wp.y);
                        break;
                    case 'F': {
                        var d = (wp.x, wp.y);
                        p = (p.x + d.x * arg, p.y + d.y * arg);
                        break;
                    }case 'R':{
                        var d = wp; //(x:wp.x -p.x, y: wp.y-p.y);
                        switch (arg) {
                            case 90:
                                d = (d.y, -d.x);
                                break;
                            case 180:
                                d = (-d.x, -d.y);
                                break;
                            case 270:
                                d = (-d.y, d.x);
                                break;
                            default:
                                throw new Exception();
                        }
                        wp = (d.x, d.y);
                        break;
                    }case 'L':{
                        var d = wp; //(x:wp.x -p.x, y: wp.y-p.y);
                        switch (arg) {
                            case 90:
                                d = (-d.y, d.x);
                                break;
                            case 180:
                                d = (-d.x, -d.y);
                                break;
                            case 270:
                                d = (d.y, -d.x);
                                break;
                            default:
                                throw new Exception();
                        }
                        wp = (d.x, d.y);
                        break;
                     } default: 
                        throw new Exception();
                }
            }
            return Math.Abs(p.x) + Math.Abs(p.y);
        }
    }
}