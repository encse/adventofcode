using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode2017.Generator {

    public static class StringExtensions {
        public static string StripMargin(this string st, string margin = "|") {
            return string.Join("\n",
                from line in st.Split('\n')
                select Regex.Replace(line, @"^\s*"+Regex.Escape(margin), "")
            );
        }

        public static string Indent(this string st, int l) {
            return string.Join("\n" + new string(' ', l),
                from line in st.Split('\n')
                select Regex.Replace(line, @"^\s*\|", "")
            );
        }
    }
}