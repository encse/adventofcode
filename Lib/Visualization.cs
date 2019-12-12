using System;
using System.Linq;
using System.Collections.Generic;

class Visualization {
    static List<string> frames = new List<string>();
    public static void Play() {
        if (frames.Any()) {
            var frame = frames.Last();
            Console.WriteLine(frame);
        }
        frames.Clear();
    }
    public static void Frame(Action<Terminal> action) {
        var terminal = new Terminal();
        action(terminal);
        frames.Add(terminal.Output);
    }
}
