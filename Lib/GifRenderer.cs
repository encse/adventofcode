using SkiaSharp;
using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

class RenderToGif : TextWriter {

        TextWriter console;

        float x;
        float y;

        SKPaint paint;
        SKSurface surface;
        string escape = null;
        int leftMargin = 3;
        public RenderToGif(TextWriter console) {
            this.console = console;
            surface = SKSurface.Create(new SKImageInfo(1024, 768));

            paint = new SKPaint {
                TextSize = 12.0f,
                IsAntialias = true,
                Color = new SKColor(0xbb, 0xbb, 0xbb),
                Style = SKPaintStyle.Fill,
                Typeface = SKTypeface.FromFamilyName(
                    "monaco",
                    SKFontStyleWeight.Normal,
                    SKFontStyleWidth.Normal,
                    SKFontStyleSlant.Upright)
            };

            surface.Canvas.Clear(SKColors.Black);
            x = leftMargin;
            y = paint.TextSize;
        }

        public override void Close() {
            using var output = File.OpenWrite("x.png");
            surface.Snapshot().Encode(SKEncodedImageFormat.Gif, 100)
                .SaveTo(output);

            base.Close();
        }

        public override void Write(char value) {
            if (value == '\u001b') {
                escape = "";
                return;
            }
            if (escape != null) {
                escape += value;
                if (value == 'm') {
                    Regex regex = new Regex(@"\[38;2;(?<r>\d{1,3});(?<g>\d{1,3});(?<b>\d{1,3})(?<bold>;1)?m");
                    Match match = regex.Match(escape);
                    if (match.Success) {
                        byte r = byte.Parse(match.Groups["r"].Value);
                        byte g = byte.Parse(match.Groups["g"].Value);
                        byte b = byte.Parse(match.Groups["b"].Value);

                        paint.Color = new SKColor(r, g, b);

                    } else {
                        Console.Error.WriteLine(escape);
                    }

                    escape = null;
                }
                return;
            }

            var st = value.ToString();



            var qqq = paint.Typeface;
            var fontManager = SKFontManager.Default;
            paint.Typeface = fontManager.MatchCharacter(paint.Typeface.FamilyName, value);
            var text = value.ToString();
            surface.Canvas.DrawText(value.ToString(), new SKPoint(x, y), paint);
            paint.Typeface = qqq;
            x += paint.GetGlyphWidths(text)[0];

            if (value == '\n') {
                y += paint.FontSpacing;

                x = leftMargin;
            }

            console.Write(value);
        }

        public override Encoding Encoding {
            get { return Encoding.Default; }
        }


    }