using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text.RegularExpressions;
using System.Text;

namespace AdventOfCode.Y2018.Day21 {

    class Solution : Solver {

        public string GetName() => "Chronal Conversion";

        public IEnumerable<object> Solve(string input) {
            yield return PartOne(input);
            yield return PartTwo(input);
        }

        int PartOne(string input) {
            //  var max = 0;
            // for(var i=0;i<=108;i++){
            //     var r3=10736359 + i;
            //     r3 = r3&16777215;
            //     r3 = r3*65899;
            //     r3 = r3&16777215;
            //     max = Math.Max(r3, max);
            //     Console.WriteLine(r3);
            // }

            Foo(16311888);
            for (var i = 0; i < int.MaxValue; i++) {
                if (Solver(input, 16311888 + 1)) {
                    return i;
                }
            }
            throw new Exception();
        }

        int PartTwo(string input) {
            return 0;
            // var max = 0;
            // for(var i=0;i<=108;i++){
            //     var r3=10736359 + i;
            //     r3 = r3&16777215;
            //     r3 = r3*65899;
            //     r3 = r3&16777215;
            //     max = Math.Max(r3, max);
            //     Console.WriteLine(r3);
            // }
            // return max;
        }

        void Foo(int r0) {
            var (r1, r2, r3, r4, r5) = (0, 0, 0, 0, 0);
            //00 seti 123 0 3            ;r3=123      
            r3 = 123;
            while (true) {
                //01 bani 3 456 3            ;r3=r3&456      // L0
                r3 = r3 & 456;
                //02 eqri 3 72 3             ;r3=r3==72
                r3 = r3 == 72 ? 1 : 0;
                //03 addr 3 4 4              ;r4=r3+r4       // goto H
                if (r3 == 1) {
                    break;
                }
                //04 seti 0 0 4              ;r4=0           // goto L0
                //r4 = 0;
            }


            //05 seti 0 5 3              ;r3=0           // H
            r3 = 0;
            //06 bori 3 65536 2          ;r2=r3|65536
            while (true) {
                r2 = r3 | 65536;
                //07 seti 10736359 9 3       ;r3=10736359
                r3 = 10736359;
                //08 bani 2 255 1            ;r1=r2&255      // Q   
                while (true) {
                    r1 = r2 & 255;
                    //09 addr 3 1 3              ;r3=r3+r1
                    r3 = r3 + r1;
                    //10 bani 3 16777215 3       ;r3=r3&16777215
                    r3 = r3 & 16777215;
                    //11 muli 3 65899 3          ;r3=r3*65899
                    r3 = r3 * 65899;
                    //12 bani 3 16777215 3       ;r3=r3&16777215
                    r3 = r3 & 16777215;
                    //13 gtir 256 2 1            ;r1=256>r2
                    r1 = 256 > r2 ? 1 : 0;
                    if (r1 == 1) {
                        break;
                    }
                    //14 addr 1 4 4              ;r4=r1+r4       // goto B
                    //r4 = r1 + r4;
                    //15 addi 4 1 4              ;r4=r4+1        // goto C
                    //r4 = r4 + 1;
                    //16 seti 27 2 4             ;r4 = 27        // B: goto D
                    //r4 = 27;
                    //17 seti 0 3 1              ;r1=0           // C:
                    r1 = 0;
                    //18 addi 1 1 5              ;r5=r1+1
                    while (true) {
                        r5 = r1 + 1;
                        //19 muli 5 256 5            ;r5=r5*256
                        r5 = r5 * 256;
                        //20 gtrr 5 2 5              ;r5=r5>r2
                        r5 = r5 > r2 ? 1 : 0;
                        //21 addr 5 4 4              ;r4=r4+r5       // goto E
                        if (r5 == 1) {
                            break;
                        }
                        //r4 = r5 + r4;
                        //22 addi 4 1 4              ; goto G
                        //r4 = r4 + 1;
                        //23 seti 25 8 4             ;E: goto F
                        //r4  = 25;
                        //24 addi 1 1 1              ;G: r1=r1+1
                        r1 = r1 + 1;
                        //25 seti 17 6 4             ;F: goto C
                        //r4 = 17;
                    }
                    //26 setr 1 5 2              ;r2=r1
                    r2 = r1;
                    //27 seti 7 7 4              ;goto Q
                    //r4 = 7;
                }
                //28 eqrr 3 0 1              ;r1=r0==r3      // D:
                r1 = r3 == r0 ? 1 : 0;
                //29 addr 1 4 4              ;r4=r1+r4 //halt
                if (r1 == 1) {
                    break;
                }
                //r4 = r1 + r4;
                //30 seti 5 1 4              ;r4=5 ; goto H
                //r4 = 5;
            }
        }
        bool Solver(string input, int r0) {
            var ip = 0;
            var ipReg = int.Parse(input.Split("\n").First().Substring("#ip ".Length));
            var prg = input.Split("\n").Skip(1).ToArray();
            var regs = new int[6];
            regs[0] = r0;

            var x = new HashSet<int>();
            while (ip >= 0 && ip < prg.Length) {
                var args = prg[ip].Split(";")[0].Trim().Split(" ");
                regs[ipReg] = ip;
                regs = Step(regs, args[0], args.Skip(1).Select(int.Parse).ToArray());
                // if(ip == 7){
                //     Console.WriteLine($"{ip.ToString("00")} {prg[ip]}\t{string.Join(" ", regs)}");
                //     if(x.Contains(regs[2] & 255)){
                //         Console.WriteLine("Xxxx");    
                //     }
                //     x.Add(regs[2] & 255);
                // }

                if (ip == 28) {
                    Console.WriteLine($"{ip.ToString("00")} {prg[ip]}\t{string.Join(" ", regs)}");
                    if (x.Contains(regs[3])) {
                        Console.WriteLine("Xxxx");
                        Console.WriteLine(x.Max());
                        return true;
                    }
                    x.Add(regs[3]);
                }
                ip = regs[ipReg];
                ip++;
            }
            return true;
        }
        int[] Step(int[] regs, string op, int[] stm) {
            regs = regs.ToArray();
            switch (op) {
                case "addr": regs[stm[2]] = regs[stm[0]] + regs[stm[1]]; break;
                case "addi": regs[stm[2]] = regs[stm[0]] + stm[1]; break;
                case "mulr": regs[stm[2]] = regs[stm[0]] * regs[stm[1]]; break;
                case "muli": regs[stm[2]] = regs[stm[0]] * stm[1]; break;
                case "banr": regs[stm[2]] = regs[stm[0]] & regs[stm[1]]; break;
                case "bani": regs[stm[2]] = regs[stm[0]] & stm[1]; break;
                case "borr": regs[stm[2]] = regs[stm[0]] | regs[stm[1]]; break;
                case "bori": regs[stm[2]] = regs[stm[0]] | stm[1]; break;
                case "setr": regs[stm[2]] = regs[stm[0]]; break;
                case "seti": regs[stm[2]] = stm[0]; break;
                case "gtir": regs[stm[2]] = stm[0] > regs[stm[1]] ? 1 : 0; break;
                case "gtri": regs[stm[2]] = regs[stm[0]] > stm[1] ? 1 : 0; break;
                case "gtrr": regs[stm[2]] = regs[stm[0]] > regs[stm[1]] ? 1 : 0; break;
                case "eqir": regs[stm[2]] = stm[0] == regs[stm[1]] ? 1 : 0; break;
                case "eqri": regs[stm[2]] = regs[stm[0]] == stm[1] ? 1 : 0; break;
                case "eqrr": regs[stm[2]] = regs[stm[0]] == regs[stm[1]] ? 1 : 0; break;
            }
            return regs;
        }
    }
}