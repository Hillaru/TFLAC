using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TFY
{
    internal class Goto_analyzier
    {
        List<Lexem> Lexemes = new List<Lexem>();
        int i = -1;
        int res_i = 0;
        Lexem Lex;
        List<List<string>> Errors = new List<List<string>>();

        private void GetNext()
        {
            i++;
            if (i > Lexemes.Count - 1)
            {
                Lex = null;
                return;
            }
            Lex = Lexemes[i];
        }

        public (List<string>, List<List<string>>) Start(List<Lexem> _lexems)
        {
            i = -1;
            res_i = 0;
            Lexemes = _lexems;
            Errors = new List<List<string>>();
            List<string> Result= new List<string>();
            GetNext();
            while (i < Lexemes.Count)
            {
                Errors.Add(new List<string>());
                Result.Add(OPERATOR());
                res_i++;
            }
            if (Result[0] == "")
                Errors[res_i].Add("Не обнаружено ключевое слово 'goto'");

            return (Result, Errors);
        }

        public string OPERATOR()
        {
            if (Lex == null)
            {
                return "";
            }

            while (i < Lexemes.Count && Lex.val != "goto")
            {
                Errors[res_i].Add($"{Lex.pos} Нераспознанная лексема '{Lex.val}'");
                GetNext();
            }

            if (Lex != null && Lex.val == "goto")
            {
                var _pos = Lex.pos;
                GetNext();
                string _OPERATOR = $"{_pos} Результат обработки: goto" + LIST() + EXPR();
                return (_OPERATOR);
            }
           
            return "";
        }

        public string LIST()
        {
            if (Lex == null)
            {
                Errors[res_i].Add($"{Lexemes.Last().pos} Ожидался список меток");
                return "<(label)>";
            }

            if (Lex.val == "(")
            {
                GetNext();
                var _A = A();
                var _B = B();
                if (Lex.val != null && Lex.val == ")")
                {
                    GetNext();
                    return (" (" + _A + _B + ") ");
                }
                else
                {
                    Errors[res_i].Add($"{Lex.pos} Пропущена закрывающая скобка");
                    return (" (" + _A + _B + "<)> ");
                }
            }
            else
            {
                Errors[res_i].Add($"{Lex.pos} Пропущена открывающая скобка");
                var _A = A();
                var _B = B();
                if (Lex.val != null && Lex.val == ")")
                {
                    GetNext();
                    return (" <(>" + _A + _B + ") ");
                }
                else
                {
                    Errors[res_i].Add($"{Lex.pos} Пропущена закрывающая скобка");
                    return (" <(>" + _A + _B + "<)> ");
                }
            }
        }

        public string A()
        {
            if (Lex == null)
            {
                Errors[res_i].Add($"{Lexemes.Last().pos} Ожидалось целочисленное неотрицательное значение");
                return "<(label)>";
            }

            if (Lex != null && Lex.id == LexType.Number && Int32.TryParse(Lex.val, out int d) && d >= 0)
            {
                var temp = Lex;
                GetNext();
                return ($"{temp.val}");
            }

            Errors[res_i].Add($"{Lex.pos} Ожидалось целочисленное неотрицательное значение");
            GetNext();
            return "<label>";
        }

        public string B()
        {
            if (Lex == null)
            {
                return "";
            }

            if (Lex.val == ",")
            {
                GetNext();
                return (", " + A() + B());
            }

            return "";
        }

        public string EXPR()
        {
            return (T() + C());
        }

        public string T()
        {
            return (O() + D());
        }

        public string C()
        {
            if (Lex == null)
            {
                return "";
            }

            switch (Lex.val)
            {
                case ("+"):
                    {
                        GetNext();
                        return (" + " + T() + C());
                    }
                case ("-"):
                    {
                        GetNext();
                        return (" - " + T() + C());
                    }
                default:
                    {
                        return ("");
                    }                 
            }
        }

        public string D()
        {
            if (Lex == null)
            {
                return "";
            }

            switch (Lex.val)
            {
                case ("*"):
                    {
                        GetNext();
                        return (" * " + O() + D());
                    }
                case ("/"):
                    {
                        GetNext();
                        return (" / " + O() + D());
                    }
                default:
                    return ("");
            }
        }

        public string O()
        {
            if (Lex == null)
            {
                Errors[res_i].Add($"{Lexemes.Last().pos} Ожидался оператор или выражение");
                return "<id>";
            }

            var temp = Lex;
            switch (Lex.id)
            {
                case (LexType.Number):
                    GetNext();
                    return ($"{temp.val}");
                case (LexType.Identificator):
                    GetNext();
                    return ($"{temp.val}");
                default:
                    if (Lex.val == "(")
                    {
                        GetNext();
                        var _EXPR = EXPR();
                        if (Lex != null && Lex.val == ")")
                        {
                            GetNext();
                            return (" (" + _EXPR + ") ");
                        }
                        else
                        {
                            if (Lex != null)
                                Errors[res_i].Add($"{Lex.pos} Пропущена закрывающая скобка");
                            else
                                Errors[res_i].Add($"{Lexemes.Last().pos} Пропущена закрывающая скобка");
                            return " (" + _EXPR + "<)> ";
                        }
                    }

                    Errors[res_i].Add($"{Lex.pos} Неожиданная лексема '{Lex.val}'");
                    GetNext();
                    return "<id>";
            }
        }

    }
}
