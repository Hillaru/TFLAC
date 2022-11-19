using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.IO;
using System.Security.Cryptography;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace TFY
{
    internal class Recursive_descent
    {
        public Recursive_descent()
        {
            
        }

        List<Lexem> Lexemes = new List<Lexem>();
        int i = -1;
        int BrcktCount = 0;
        Lexem Lex;

        private void GetNext()
        {
            if (i == Lexemes.Count - 1)
            {
                Lex = null;
                return;
            }
            i++;

            while (Lexemes[i].val == ")" && BrcktCount > 0 && i < Lexemes.Count - 1)
                {
                    BrcktCount--;
                    i++;
                }

            while (Lexemes[i].id == LexType.Undef && i < Lexemes.Count - 1)
                i++;
            Lex = Lexemes[i];
        }

        public string Start(List<Lexem> _lexems)
        {
            i = -1;
            BrcktCount = 0;
            Lexemes = _lexems;
            GetNext();
            return (E() + "EOF");
        }

        public string E()
        {
            string left = T();
            string right = A();
            return ("E -> " + left + right);
        }

        public string T()
        {
            string left = O();
            string right = B();
            return ("T -> " + left + right);
        }

        public string A()
        {
            if (Lex == null)
                return "A -> e -> ";

            switch (Lex.val)
            {
                case ("+"):
                    {
                        GetNext();
                        return ("A -> + -> " + T() + A());
                    }
                case ("-"):
                    {
                        GetNext();
                        return ("A -> - -> " + T() + A());
                    }                 
                default:
                    return ("A -> e -> ");
            }
        }

        public string B()
        {
            if (Lex == null)
                return "B -> e -> ";

            switch (Lex.val)
            {
                case ("*"):
                    {
                        GetNext();
                        return ("B -> * -> " + O() + B());
                    }
                case ("/"):
                    {
                        GetNext();
                        return ("B -> / -> " + O() + B());
                    }
                default:
                    return ("B -> e -> ");
            }
        }

        public string O()
        {
            if (Lex == null)
                return "O(ERROR) -> ";          

            if (Lex.val == "(")
            {
                GetNext();
                BrcktCount++;
                return ("O -> (E) -> " + E());
            }

            string Val = Lex.val;
            switch (Lex.id)
            {
                case (LexType.Number):
                    GetNext();
                    return ($"O -> num[{Val}] -> ");
                case (LexType.Identificator):
                    GetNext();
                    return ($"O -> id[{Val}] -> ");
                default:
                    GetNext();
                    return "O(ERROR) -> ";
            }
        }
    }
}