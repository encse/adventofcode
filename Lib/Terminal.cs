using System;

namespace AdventOfCode {
    class Terminal {
        public const int Black = 000000;
        public const int DarkBlue = 0x00008B;
        public const int DarkGreen = 0x006400;
        public const int DarkCyan = 0x008B8B;
        public const int DarkRed = 0x8B0000;
        public const int DarkMagenta = 0x8B008B;
        public const int DarkYellow = 0x000000;
        public const int Gray = 0x808080;
        public const int DarkGray = 0xA9A9A9;
        public const int Blue = 0x0000FF;
        public const int Green = 0x008000;
        public const int Cyan = 0x00FFFF;
        public const int Red = 0xFF0000;
        public const int Magenta = 0xFF00FF;
        public const int Yellow = 0xFFFF00;
        public const int White = 0xFFFFFF;

        public const int Default = Gray;


        public static void ResetFont() {
            SetFont(Terminal.Default, false);
        }

        public static void SetFont(int rgb, bool bold = false) {
            Console.Write($"\u001b[38;2;{(rgb >> 16) & 255};{(rgb >> 8) & 255};{rgb & 255}{(bold ? ";1" : "")}m");
        }

    }
}