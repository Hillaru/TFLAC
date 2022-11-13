using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.IO;
using System.Security.Cryptography;
using System.Windows.Forms;

namespace TFY
{
    internal class Lexem_analyzier
    {
        public Lexem_analyzier()
        {
        }

        private string Buffer = ""; // буфер для хранения лексемы
        private char ReadenSymbol;
        private enum States { START, NUM, SYMB, ID, ERROR, NEGATIVE, BRCKT } // состояния state-машины
        private States state; // хранит текущее состояние

        private (int,int) pos;
        private (int, int) Lexem_start_pos;
        private string[] System_symbols = { ";", ",", "//", "=" };
        private string[] Brackets = { "(", ")", "{", "}", "[", "]" };
        private string[] Aryfmethic = { "+", "-", "*", "/" };
        private string[] Comparison = {"==", "<=", ">=", "!=", ">", "<" };
        private string[] Delimeters = { " ", "\t", "\0", "\r" };
        private List<Lexem> Lexemes = new List<Lexem>();
        private List<Lexem> Errors = new List<Lexem>();

        private void ClearBuf()
        {
            Buffer = "";
        }

        private void AddBuf(char symbol)
        {
            Buffer += symbol;
        }

        private bool SearchLex(string[] lexemes, string buf)
        {
            if (Array.FindIndex(lexemes, s => s.Equals(buf)) == -1)
                return false;
            return true;
        }


        private void AddLex(List<Lexem> lexes, LexType id, (int,int) pos, string val)
        {
            lexes.Add(new Lexem(id, pos, val));
        }

        public (List<Lexem>, List<Lexem>) Analyse(string text)
        {
            Lexemes = new List<Lexem>();
            Errors = new List<Lexem>();
            pos = (1, 1); 
            int i = 0;  
            while (i <= text.Length)
            {
                if (i == text.Length)
                    ReadenSymbol = '\0';
                else
                    ReadenSymbol = text[i];
                switch (state)
                {
                    case States.START:
                        if (SearchLex(Delimeters, ReadenSymbol.ToString()))
                        {
                            i++;
                            pos.Item2++;
                        }
                        else if ((SearchLex(Brackets, ReadenSymbol.ToString())))
                        {
                            state = States.BRCKT;
                        }
                        else if (ReadenSymbol == '-')
                        {
                            ClearBuf();
                            AddBuf(ReadenSymbol);
                            i++;
                            pos.Item2++;
                            state = States.NEGATIVE;
                        }
                        else if (Char.IsLetter(ReadenSymbol))
                        {
                            ClearBuf();
                            AddBuf(ReadenSymbol);
                            i++;
                            pos.Item2++;
                            state = States.ID;
                        }
                        else if (char.IsDigit(ReadenSymbol))
                        {
                            ClearBuf();
                            AddBuf(ReadenSymbol);
                            i++;
                            pos.Item2++;
                            state = States.NUM;

                        }
                        else if (ReadenSymbol == '\n')
                        {
                            pos.Item1++;
                            pos.Item2 = 1;
                            i++;
                        }
                        else
                        {
                            ClearBuf();
                            AddBuf(ReadenSymbol);
                            i++;
                            pos.Item2++;
                            state = States.SYMB;
                        }
                        break;
                    case States.ID:
                        if (Char.IsLetterOrDigit(ReadenSymbol))
                        {
                            AddBuf(ReadenSymbol);
                            i++;
                            pos.Item2++;
                        }
                        else
                        {
                            Lexem_start_pos = pos;
                            Lexem_start_pos.Item2 -= Buffer.Length;
                            AddLex(Lexemes, LexType.Identificator, Lexem_start_pos, Buffer);
                            state = States.START;
                        }
                        break;
                    case States.NEGATIVE:
                        if (Char.IsDigit(ReadenSymbol))
                            state = States.NUM;
                        else
                            state = States.SYMB;
                        break;
                    case States.BRCKT:
                        AddLex(Lexemes, LexType.Bracket, pos, ReadenSymbol.ToString());
                        i++;
                        pos.Item2++;
                        state = States.START;
                        break;
                    case States.NUM:
                        if (Char.IsDigit(ReadenSymbol) || ReadenSymbol == '.')
                        {
                            if (ReadenSymbol == '.')
                                ReadenSymbol = ',';
                            AddBuf(ReadenSymbol);
                            i++;
                            pos.Item2++;
                        }
                        else
                        {
                            if (!Double.TryParse(Buffer, out double d))
                            {
                                state = States.ERROR;
                                break;
                            }
                            Lexem_start_pos = pos;
                            Lexem_start_pos.Item2 -= Buffer.Length;
                            AddLex(Lexemes, LexType.Number, Lexem_start_pos, Buffer);
                            state = States.START;
                        }
                        break;
                    case States.SYMB:
                        if (!Char.IsLetterOrDigit(ReadenSymbol) && ReadenSymbol != '\n' && !(SearchLex(Delimeters, ReadenSymbol.ToString())))
                        {
                            AddBuf(ReadenSymbol);
                            i++;
                            pos.Item2++;
                        }
                        else
                        {
                            var lex_check = (SearchLex(System_symbols, Buffer),
                                             SearchLex(Aryfmethic, Buffer),
                                             SearchLex(Comparison, Buffer));
                            LexType temp_type;
                            if (lex_check.Item1 == true && lex_check.Item2 == false && lex_check.Item3 == false)
                            {
                                temp_type = LexType.System_symbol;
                            } 
                            else if (lex_check.Item1 == false && lex_check.Item2 == true && lex_check.Item3 == false)
                            {
                                temp_type = LexType.Aryfmethic;
                            }
                            else if (lex_check.Item1 == false && lex_check.Item2 == false && lex_check.Item3 == true)
                            {
                                temp_type = LexType.Comparison;
                            }
                            else
                            {
                                state = States.ERROR;
                                break;
                            }
                            Lexem_start_pos = pos;
                            Lexem_start_pos.Item2 -= Buffer.Length;
                            AddLex(Lexemes, temp_type, Lexem_start_pos, Buffer);
                            state = States.START;
                        }
                        break;
                    case States.ERROR:
                        Lexem_start_pos = pos;
                        Lexem_start_pos.Item2 -= Buffer.Length;
                        AddLex(Errors, LexType.Undef, Lexem_start_pos, Buffer);
                        state = States.START;
                        break;
                }
            }
            return (Lexemes, Errors);
        }
    }
}
