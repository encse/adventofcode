using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Y2021.Day16;

[ProblemName("Packet Decoder")]
class Solution : Solver {

    public object PartOne(string input) {

        var packet = Parse(GetInput(input));
        int rec(Packet p)  {
            if (p is Literal) {
                return p.version;
            } else if (p is Operator) {
                return p.version + (p as Operator).packets.Select(rec).Sum();
            } else {
                throw new Exception();
            }
        }
        return rec(packet);
    }


    public object PartTwo(string input) {
        var packet = Parse(GetInput(input));
        long rec(Packet p) {
            if (p is Literal) {
                return (p as Literal).value;
            } else if (p is Operator) {
                var op = p as Operator;
                var parts = op.packets.Select(rec).ToArray();
                switch (op.type) {
                    case 0:
                        return parts.Sum();
                    case 1:
                        return parts.Aggregate(1L, (acc, x) => acc * x);
                    case 2: return parts.Min();
                    case 3: return parts.Max();
                    case 5: return parts[0] > parts[1] ? 1 : 0;
                    case 6: return parts[0] < parts[1] ? 1 : 0;
                    case 7: return parts[0] == parts[1] ? 1 : 0;
                    default: throw new Exception();
                }
            } else {
                throw new Exception();
            }
        }
        return rec(packet);

    }

    Input GetInput(string input) => new Input(
        (from ch in input
         let bits = Convert.ToString(Convert.ToInt32(ch.ToString(), 16), 2).PadLeft(4, '0')
         from bit in bits
         select bit - '0').ToList()
    );

    Packet Parse(Input input) {
        var version = input.ReadInt(3);
        var type = input.ReadInt(3);
        return
            type == 0x4 ?
                ParseLiteral(version, type, input) :
                ParseOperator(version, type, input);
    }

    Packet ParseLiteral(int version, int type, Input input) {
        var value = 0L;
        while (true) {
            var last = input.ReadInt(1) == 0;
            value <<= 4;
            value += input.ReadInt(4);
            if (last) {
                break;
            }
        }
        return new Literal(version, type, value);
    }

    Packet ParseOperator(int version, int type, Input input) {
        var packets = new List<Packet>();
        if (input.ReadInt(1) == 0) {
            var length = input.ReadInt(15);
            var subInput = input.ReadInput(length);
            while (subInput.Any()) {
                packets.Add(Parse(subInput));
            }

        } else {
            var length = input.ReadInt(11);
            packets = Enumerable.Range(0, length).Select(x => Parse(input)).ToList();
        }

        return new Operator(version, type, packets.ToArray());
    }

}

class Input {
    private List<int> numbers;
    private int ptr;
    public Input(List<int> numbers) {
        this.numbers = numbers;
    }

    public bool Any() {
        return ptr < numbers.Count();
    }
    public Input ReadInput(int n) {
        var sub = numbers.GetRange(ptr, n).ToList();
        var res = new Input(sub);
        ptr += n;
        return res;
    }

    public int ReadInt(int n) {
        var res = 0;
        foreach (var bit in numbers.GetRange(ptr, n)) {
            res = res * 2 + bit;
        }
        ptr += n;
        return res;
    }
}

record Packet(int version, int type);
record Literal(int version, int type, long value) : Packet(version, type);
record Operator(int version, int type, Packet[] packets) : Packet(version, type);