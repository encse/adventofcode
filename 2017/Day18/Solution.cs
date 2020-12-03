using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Y2017.Day18 {

    [ProblemName("Duet")]
    class Solution : Solver {

        public IEnumerable<object> Solve(string input) {
            yield return PartOne(input);
            yield return PartTwo(input);
        }

        long PartOne(string input) =>
            new Machine1()
                .Execute(input)
                .First(received => received != null).Value;

        int PartTwo(string input) {
            var p0Input = new Queue<long>();
            var p1Input = new Queue<long>();

            return Enumerable
                .Zip(
                    new Machine2(0, p0Input, p1Input).Execute(input), 
                    new Machine2(1, p1Input, p0Input).Execute(input), 
                    (state0, state1) => (state0: state0, state1: state1))
                .First(x => !x.state0.running && !x.state1.running)
                .state1.valueSent;
        }
    }

    abstract class Machine<TState> {
        private Dictionary<string, long> regs = new Dictionary<string, long>();

        protected bool running;
        protected int ip = 0;
        protected long this[string reg] {
            get {
                return long.TryParse(reg, out var n) ? n
                    : regs.ContainsKey(reg) ? regs[reg]
                    : 0;
            }
            set {
                regs[reg] = value;
            }
        }

        public IEnumerable<TState> Execute(string input) {
            var prog = input.Split('\n').ToArray();

            while (ip >= 0 && ip < prog.Length) {
                running = true;
                var line = prog[ip];
                var parts = line.Split(' ');
                switch (parts[0]) {
                    case "snd": snd(parts[1]); break;
                    case "rcv": rcv(parts[1]); break;
                    case "set": set(parts[1], parts[2]); break;
                    case "add": add(parts[1], parts[2]); break;
                    case "mul": mul(parts[1], parts[2]); break;
                    case "mod": mod(parts[1], parts[2]); break;
                    case "jgz": jgz(parts[1], parts[2]); break;
                    default: throw new Exception("Cannot parse " + line);
                }
                yield return State();
            }

            running = false;
            yield return State();
        }

        protected abstract TState State();

        protected abstract void snd(string reg);
        
        protected abstract void rcv(string reg);

        protected void set(string reg0, string reg1) {
            this[reg0] = this[reg1];
            ip++;
        }

        protected void add(string reg0, string reg1) {
            this[reg0] += this[reg1];
            ip++;
        }

        protected void mul(string reg0, string reg1) {
            this[reg0] *= this[reg1];
            ip++;
        }

        protected void mod(string reg0, string reg1) {
            this[reg0] %= this[reg1];
            ip++;
        }

        protected void jgz(string reg0, string reg1) {
            ip += this[reg0] > 0 ? (int)this[reg1] : 1;
        }
    }

    class Machine1 : Machine<long?> {
        private long? sent = null;
        private long? received = null;

        protected override long? State() { 
            return received; 
        }

        protected override void snd(string reg) {
            sent = this[reg];
            ip++;
        }

        protected override void rcv(string reg) {
            if (this[reg] != 0) {
                received = sent;
            }
            ip++;
        }

    }

    class Machine2 : Machine<(bool running, int valueSent)> {
        private int valueSent = 0;
        private Queue<long> qIn;
        private Queue<long> qOut;

        public Machine2(long p, Queue<long> qIn, Queue<long> qOut) {
            this["p"] = p;
            this.qIn = qIn;
            this.qOut = qOut;
        }

        protected override (bool running, int valueSent) State() { 
            return (running: running, valueSent: valueSent); 
        }  

        protected override void snd(string reg) {
            qOut.Enqueue(this[reg]);
            valueSent++;
            ip++;
        }

        protected override void rcv(string reg) {
            if (qIn.Any()) {
                this[reg] = qIn.Dequeue();
                ip++;
            } else {
                running = false;
            }
        }
    }
}
