using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.Drawing;
using System.Windows.Forms;

namespace TFY
{
    internal class Lexem
    {
        public Regex regex;
        public string Type;

        public Lexem(string _regex, string _type)
        {
            regex = new Regex(_regex, RegexOptions.Multiline);
            Type = _type;
        }

        public MatchCollection Find(string text)
        {
            return regex.Matches(text);
        }
    }
}
