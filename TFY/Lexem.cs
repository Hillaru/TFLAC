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
    public enum LexType
    {
        Undef = 1,
        System_symbol = 2,
        Identificator = 3,
        Number = 4,
        Bracket = 5,
        Aryfmethic = 6,
        Comparison = 7
    }

    internal class Lexem
    {
        public LexType id;
        public (int,int) pos;
        public string val;

        public Lexem(LexType _id, (int,int) _pos, string _val)
        {
            id = _id;
            pos = _pos;
            val = _val;
        }
    }
}
