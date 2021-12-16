using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Y2021.Day16;

[ProblemName("Packet Decoder")]
class Solution : Solver {

    public object PartOne(string input) =>
        GetTotalVersion(GetPacket(GetInput(input)));

    public object PartTwo(string input) =>
        Evaluate(GetPacket(GetInput(input)));

    long Evaluate(Packet packet) {
        var parts = packet.packets.Select(Evaluate).ToArray();
        return packet.type switch {
            0 => parts.Sum(),
            1 => parts.Aggregate(1L, (acc, x) => acc * x),
            2 => parts.Min(),
            3 => parts.Max(),
            4 => packet.payload,
            5 => parts[0] > parts[1] ? 1 : 0,
            6 => parts[0] < parts[1] ? 1 : 0,
            7 => parts[0] == parts[1] ? 1 : 0,
            _ => throw new Exception()
        };
    }

    int GetTotalVersion(Packet packet) =>
        packet.version + packet.packets.Select(GetTotalVersion).Sum();

    Input GetInput(string input) => new Input(
        (from ch in input
         let bits = Convert.ToString(Convert.ToInt32(ch.ToString(), 16), 2).PadLeft(4, '0')
         from bit in bits
         select bit - '0').ToList()
    );

    Packet GetPacket(Input input) {
        var version = input.ReadInt(3);
        var type = input.ReadInt(3);
        var packets = new List<Packet>();
        var payload = 0L;

        if (type == 0x4) {
            while (true) {
                var isLast = input.ReadInt(1) == 0;
                payload = payload * 16 + input.ReadInt(4);
                if (isLast) {
                    break;
                }
            }
        } else if (input.ReadInt(1) == 0) {
            var length = input.ReadInt(15);
            var subPackages = input.ReadInput(length);
            while (subPackages.Any()) {
                packets.Add(GetPacket(subPackages));
            }
        } else {
            var packetCount = input.ReadInt(11);
            packets.AddRange(from _ in Enumerable.Range(0, packetCount) select GetPacket(input));
        }

        return new Packet(version, type, payload, packets.ToArray());
    }
}

class Input {
    private List<int> bits;
    private int ptr;

    public Input(List<int> numbers) {
        this.bits = numbers;
    }

    public bool Any() {
        return ptr < bits.Count();
    }

    public Input ReadInput(int n) {
        var sub = bits.GetRange(ptr, n).ToList();
        var res = new Input(sub);
        ptr += n;
        return res;
    }

    public int ReadInt(int n) {
        var res = 0;
        foreach (var bit in bits.GetRange(ptr, n)) {
            res = res * 2 + bit;
        }
        ptr += n;
        return res;
    }
}

record Packet(int version, int type, long payload, Packet[] packets);