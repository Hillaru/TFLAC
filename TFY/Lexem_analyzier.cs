using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace TFY
{
    public struct LexemItem
    {
        public string Text;
        public string Type;
        public int StartPos;
        public int Length;
    }

    internal class Lexem_analyzier
    {
        #region OLD
        List<Lexem> lexems;

        public Lexem_analyzier()
        {
            lexems = new List<Lexem>
            {
                new Lexem(@"(\(|\))", "Скобка"),
                new Lexem(@"(\{|\})", "Фигурная скобка"),
                new Lexem(@"\;", "Символ конца выражения"),
                new Lexem(@"\,", "Запятая"),
                new Lexem(@"(\d+|(\d+.\d+))", "Число"),
                new Lexem(@"int\s+[^\u0022\|\\/\*\?\<\>\s\d\.][^\u0022\|\\/\*\?\<\>\s\.]*", "Целая переменная"),
                new Lexem(@"float\s+[^\u0022\|\\/\*\?\<\>\s\d\.][^\u0022\|\\/\*\?\<\>\s\.]*", "Вещественная переменная"),
                new Lexem(@"(\+|\-|\/|\*)", "Арифметическая операция"),
                new Lexem(@"\=", "Операция присваивания"),
            };
        }

        public List<LexemItem> Analyze(string text)
        {
            List<LexemItem> Result = new List<LexemItem>();
            foreach (Lexem lexem in lexems)
            {
                MatchCollection Collection = lexem.Find(text);
                foreach (Match match in Collection)
                {
                    LexemItem Item = new LexemItem();
                    Item.Type = lexem.Type;
                    Item.StartPos = match.Index;
                    Item.Length = match.Length;
                    Item.Text = text.Substring(Item.StartPos, Item.Length);
                    Result.Add(Item);
                }
            }
            return Result;
        } 
        #endregion


    }
}
