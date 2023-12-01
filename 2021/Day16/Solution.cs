using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;

namespace AdventOfCode.Y2021.Day16;

[ProblemName("Packet Decoder")]
class Solution : Solver {

    public object PartOne(string input) =>
        GetTotalVersion(GetPacket(GetReader(input)));

    public object PartTwo(string input) =>
        Evaluate(GetPacket(GetReader(input)));

    // recursively sum the versions of a packet and its content for part 1:
    int GetTotalVersion(Packet packet) =>
        packet.version + packet.packets.Select(GetTotalVersion).Sum();

    // recursively evaluate the packet and its contents based on the type tag for part 2:
    long Evaluate(Packet packet) {

        var parts = packet.packets.Select(Evaluate).ToArray();
        return packet.type switch {
            0 => parts.Sum(),
            1 => parts.Aggregate(1L, (acc, x) => acc * x),
            2 => parts.Min(),
            3 => parts.Max(),
            4 => packet.payload, // <--- literal packet is handled uniformly
            5 => parts[0] > parts[1] ? 1 : 0,
            6 => parts[0] < parts[1] ? 1 : 0,
            7 => parts[0] == parts[1] ? 1 : 0,
            _ => throw new Exception()
        };
    }

    // convert hex string to bit sequence reader
    BitSequenceReader GetReader(string input) => new BitSequenceReader(
        new BitArray((
            from hexChar in input
            // get the 4 bits out of a hex char:
            let value = Convert.ToInt32(hexChar.ToString(), 16)
            // convert to bitmask
            from mask in new []{8,4,2,1}
            select (mask & value) != 0
        ).ToArray()
    ));

    // make sense of the bit sequence:
    Packet GetPacket(BitSequenceReader reader) {
        var version = reader.ReadInt(3);
        var type = reader.ReadInt(3);
        var packets = new List<Packet>();
        var payload = 0L;

        if (type == 0x4) {
            // literal, payload is encoded in the following bits in 5 bit long chunks:
            while (true) {
                var isLast = reader.ReadInt(1) == 0;
                payload = payload * 16 + reader.ReadInt(4);
                if (isLast) {
                    break;
                }
            }
        } else if (reader.ReadInt(1) == 0) {
            // operator, the next 'length' long bit sequence encodes the sub packages:
            var length = reader.ReadInt(15);
            var subPackages = reader.GetBitSequenceReader(length);
            while (subPackages.Any()) {
                packets.Add(GetPacket(subPackages));
            }
        } else {
            // operator with 'packetCount' sub packages:
            var packetCount = reader.ReadInt(11);
            packets.AddRange(from _ in Enumerable.Range(0, packetCount) select GetPacket(reader));
        }

        return new Packet(version, type, payload, packets.ToArray());
    }
}

// Reader class with convenience methods to retrieve n-bit integers and subreaders as needed
class BitSequenceReader {
    private BitArray bits;
    private int ptr;

    public BitSequenceReader(BitArray bits) {
        this.bits = bits;
    }

    public bool Any() {
        return ptr < bits.Length;
    }

    public BitSequenceReader GetBitSequenceReader(int bitCount) {
        var bitArray = new BitArray(bitCount);
        for (var i = 0; i < bitCount; i++) {
            bitArray.Set(i, bits[ptr++]);
        }
        return new BitSequenceReader(bitArray);
    }

    public int ReadInt(int bitCount) {
        var res = 0;
        for (var i = 0; i < bitCount; i++) {
            res = res * 2 + (bits[ptr++] ? 1 : 0);
        }
        return res;
    }
}

// Each packet has all fields, type tag tells how to interpret the contents
record Packet(int version, int type, long payload, Packet[] packets);
