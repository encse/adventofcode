using System.Text;
namespace AdventOfCode;

public class AocCommuncationError : System.Exception {
    public readonly string Title;
    public readonly System.Net.HttpStatusCode? Status;
    public readonly string Details;
    public AocCommuncationError(string title, System.Net.HttpStatusCode? status = null, string details = "") {
        Title = title;
        Status = status;
        Details = details;
    }

    public override string Message {
        get {
            var sb = new StringBuilder();
            sb.AppendLine(Title);
            if (Status != null) {
                sb.Append($"[{Status}] ");
            }
            sb.AppendLine(Details);
            return sb.ToString();
        }
    }
}