using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Text;
using AngleSharp.Dom;

namespace AdventOfCode.Model;

class CalendarToken {
    public string Text { get; set; }
    public string RgbaColor { get; set; }
    public int ConsoleColor { get; set; }
    public bool Bold { get; set; }
}

class Calendar {
    public int Year;

    public Dictionary<string[], int> Theme = new Dictionary<string[], int>();
    public IReadOnlyList<IReadOnlyList<CalendarToken>> Lines { get; private set; }

    public static Calendar Parse(int year, IDocument document) {

        var theme = new Dictionary<string[], int>();

        // anglesharp bug, it doesn't handle external stylehseets well.
        var q = document.CreateElement("style");
        q.SetInnerText($@"
            .calendar > span {{
                color: #333333;
            }}
            .calendar > a {{
                text-decoration: none;
                color: #666666;
                outline: none;
                cursor: default;
            }}

            .calendar .calendar-day {{ color: #666666; }}
            .calendar a .calendar-day {{ color: #cccccc; }}
            .calendar a .calendar-mark-complete,
            .calendar a .calendar-mark-verycomplete {{visibility: hidden;}}
            .calendar a.calendar-complete     .calendar-mark-complete,
            .calendar a.calendar-verycomplete .calendar-mark-complete {{ visibility: visible; color: #ffff66; }}
            .calendar a.calendar-verycomplete .calendar-mark-verycomplete {{ visibility: visible; color: #ffff66; }}

        ");

        document.Head.Append(q);

        foreach (var item in document.QuerySelectorAll("link").ToList()) {
            item.Remove();
        }

        foreach (var item in document.QuerySelectorAll("script").ToList()) {
            item.Remove();
        }

        var calendar = document.QuerySelector(".calendar");

        var r = new Random();
        var years = new []{
            $@"0x0000 | {year}",
            $@"/* {year} */",
            $@"int y = {year};",
            $@"/^{year}$/",
            $@"λy.{year}",
            $@"{{:year {year}}}",
            $@"sub y{{{year}}}",
            $@"// {year}",
            $@"{{'year': {year}}}",
            $@"$year = {year}"
        };

        var stYear = years[r.Next(years.Length)];

        var lines = new List<List<CalendarToken>>(){
                new List<CalendarToken>(){
                    new CalendarToken {ConsoleColor = 0x00cc00, RgbaColor = "rgba(0,204,0,1)", Text = $@"▄█▄ ▄▄█ ▄ ▄ ▄▄▄ ▄▄ ▄█▄  ▄▄▄ ▄█  ▄▄ ▄▄▄ ▄▄█ ▄▄▄"}
                },
                new List<CalendarToken>(){
                    new CalendarToken {ConsoleColor = 0x00cc00, RgbaColor = "rgba(0,204,0,1)", Text = $@"█▄█ █ █ █ █ █▄█ █ █ █   █ █ █▄  █  █ █ █ █ █▄█"}
                },
                new List<CalendarToken>(){
                    new CalendarToken {ConsoleColor = 0x00cc00, RgbaColor = "rgba(0,204,0,1)", Text = $@"█ █ █▄█ ▀▄▀ █▄▄ █ █ █▄  █▄█ █   █▄ █▄█ █▄█ █▄▄  {stYear}"}
                },
                new List<CalendarToken>(){
                    new CalendarToken {ConsoleColor = 0x00cc00, RgbaColor = "rgba(0,204,0,1)", Text = $@" "}
                }
        };

        var line = new List<CalendarToken>();
        lines.Add(line);

        foreach (var textNode in GetText(calendar)) {
            var text = textNode.Text();
            var style = textNode.ParentElement.ComputeCurrentStyle();
            var rgbaColor = style["color"];
            var bold = !string.IsNullOrEmpty(style["text-shadow"]);

            if (style["position"] == "absolute" ||
                textNode.ParentElement.ParentElement.ComputeCurrentStyle()["position"] == "absolute"
            ) {
                continue;
            }
            var widthSpec = string.IsNullOrEmpty(style["width"]) ?
                textNode.ParentElement.ParentElement.ComputeCurrentStyle()["width"] : style["width"];
            if (widthSpec != null) {

                var m = Regex.Match(widthSpec, "[.0-9]+");
                if (m.Success) {
                    var width = double.Parse(m.Value) * 1.7;
                    var c = (int)Math.Round(width - text.Length, MidpointRounding.AwayFromZero);
                    if (c > 0) {
                        text += new string(' ', c);
                    }
                }
            }

            var i = 0;
            while (i < text.Length) {
                var iNext = text.IndexOf("\n", i);
                if (iNext == -1) {
                    iNext = text.Length;
                }

                line.Add(new CalendarToken {
                    Text = text.Substring(i, iNext - i),
                    ConsoleColor = ParseRgbaColor(rgbaColor),
                    RgbaColor = rgbaColor,
                    Bold = bold
                });

                if (iNext < text.Length) {
                    line = new List<CalendarToken>();
                    lines.Add(line);
                }
                i = iNext + 1;
            }
        }


        return new Calendar { Year = year, Theme = theme, Lines = lines };
    }

    private static IEnumerable<INode> GetText(INode element) {
        if (element.NodeType == NodeType.Text) {
            yield return element;
        } else {
            element = element.FirstChild;
            while (element != null) {
                foreach (var text in GetText(element)) {
                    yield return text;
                }
                element = element.NextSibling;
            }
        }
    }

    private static int ParseRgbaColor(string st) {
        Regex regex = new Regex(@"rgba\((?<r>\d{1,3}), (?<g>\d{1,3}), (?<b>\d{1,3}), (?<a>\d{1,3})\)");
        Match match = regex.Match(st);
        if (match.Success) {
            int r = int.Parse(match.Groups["r"].Value);
            int g = int.Parse(match.Groups["g"].Value);
            int b = int.Parse(match.Groups["b"].Value);

            return (r << 16) + (g << 8) + b;
        }
        return 0x888888;
    }

    public string ToSvg() {

        string font = Convert.ToBase64String(File.ReadAllBytes("Lib/SourceCodePro-Regular.woff2"));

        var sb = new StringBuilder();
        var height = 0;
        var width = 0;
        sb.AppendLine($@"
                <style>
                    @font-face {{
                        font-family: ""SourceCodePro"";
                        src: url(""data:application/font-woff;charset=utf-8;base64,{font}"");
                    }}
                    text {{
                         font-family: SourceCodePro;
                         font-size: 13.2px;
                    }}
                </style>");
        sb.AppendLine(@"<text xml:space=""preserve"">");
        foreach (var line in this.Lines) {
            sb.Append($@"<tspan x=""0"" dy=""1.2em"">");
            var lineWidth = 0;
            foreach (var token in line) {
                var text = token.Text
                    .Replace("<", "&lt;")
                    .Replace(">", "&gt;")
                    .Replace(" ", "&#160;");
                sb.Append($@"<tspan fill=""{token.RgbaColor}"">{text}</tspan>");

                lineWidth += token.Text.Length;
            }
            width = Math.Max(width, lineWidth);
            sb.AppendLine("</tspan>");
            height++;
        }
        sb.AppendLine("</text>");
        return $@"<svg viewBox=""-16 -16 {(width+4) * 8} {(height+2) *16}"" style=""background-color:black"" xmlns=""http://www.w3.org/2000/svg"">
            {sb.ToString()}
        </svg>";
    }
}
