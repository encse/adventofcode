using System.Text;

enum TerminalColor {
    Default = 0xbfbfbf,
    White = 0xffffff,
    Black = 0,
}

class Terminal {

    StringBuilder sb = new StringBuilder();

    public void Color(TerminalColor color) {
        sb.Append($"\u001b[38;2;{((int)color >> 16) & 255};{((int)color >> 8) & 255};{(int)color & 255}m");
    }

    public void BackgroundColor(TerminalColor color) {
        sb.Append($"\u001b[48;2;{((int)color >> 16) & 255};{((int)color >> 8) & 255};{(int)color & 255}m");
    }

    public void Up(int amount = 1) {
        sb.Append("\u001b[" + amount + "A");
    }

    public void Down(int amount = 1) {
        sb.Append("\u001b[" + amount + "B");
    }

    public void Write(object value) {
        sb.Append(value);
    }

    public void WriteLine(object value = null) {
        sb.AppendLine(value?.ToString());
    }

    public string Output {
        get {
            return sb.ToString();
        }
    }
}