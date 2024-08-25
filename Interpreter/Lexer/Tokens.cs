using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Interpreter
{
    public class Token
    {
        public Token(TokenType type, string value, int line, int column)
        {
            Type = type;
            Value = value;
            Line = line;
            Column = column;
        }

        public TokenType Type { get; }
        public string Value { get; }
        public int Line { get; }
        public int Column { get; }

        public static Dictionary<TokenType, Regex> TokenStringDictionary = new Dictionary<TokenType, Regex>
        {
            {TokenType.Number, new Regex(@"^(?:\d+(?:\.\d+)?)\b(?![a-zA-Z0-9])")},
            {TokenType.String, new Regex("^\\\"(.*?)\\\"")},
            {TokenType.True, new Regex(@"^true")},
            {TokenType.False, new Regex(@"^false")},
            {TokenType.Null, new Regex(@"^null")},
            {TokenType.Lambda, new Regex(@"^=>")},
            {TokenType.Plus_Plus,new Regex(@"^\+\+")},
            {TokenType.Plus,new Regex(@"^\+")},
            {TokenType.Minus_Minus,new Regex(@"^--")},
            {TokenType.Minus,new Regex(@"^\-")},
            {TokenType.Division,new Regex(@"^\/")},
            {TokenType.Multiplication,new Regex(@"^\*")},
            {TokenType.Pow,new Regex(@"^\^")},
            {TokenType.WhiteSpace_Concat,new Regex(@"^@@")},
            {TokenType.Concat,new Regex(@"^@")},
            {TokenType.Colon,new Regex(@"^:")},
            {TokenType.Less_Equal,new Regex(@"^<=")},
            {TokenType.Greater_Equal,new Regex(@"^>=")},
            {TokenType.Less,new Regex(@"^<")},
            {TokenType.Greater,new Regex( @"^>")},
            {TokenType.Not_Equal,new Regex( @"^!=")},
            {TokenType.Equal_Equal,new Regex( @"^==")},
            {TokenType.Equal,new Regex(@"^=")},
            {TokenType.And,new Regex( @"^&&")},
            {TokenType.Or,new Regex( @"^(\|\|)")},
            {TokenType.Dot,new Regex(@"^\.")},
            {TokenType.Comma,new Regex(@"^,")},
            {TokenType.Semicolon,new Regex(@"^;")},
            {TokenType.Left_Paren,new Regex(@"^\(")},
            {TokenType.Right_Paren,new Regex(@"^\)")},
            {TokenType.Left_Brace,new Regex(@"^{")},
            {TokenType.Right_Brace,new Regex(@"^}")},
            {TokenType.Left_Brackets,new Regex(@"^\[")},
            {TokenType.Right_Brackets,new Regex(@"^]")},
            {TokenType.For,new Regex(@"^\bfor\b")},
            {TokenType.In,new Regex(@"^\bin\b")},
            {TokenType.While,new Regex(@"^\bwhile\b")},
            {TokenType.Print,new Regex(@"^\bprint\b")},
            {TokenType.Var,new Regex(@"^\bvar\b")},
            {TokenType.Effect,new Regex(@"^\beffect\b")},
            {TokenType.Card,new Regex(@"^\bcard\b")},
            {TokenType.Type,new Regex(@"^\bType\b")},
            {TokenType.Name,new Regex(@"^\bName\b")},
            {TokenType.Faction,new Regex(@"^\bFaction\b")},
            {TokenType.Power,new Regex(@"^\bPower\b")},
            {TokenType.Range,new Regex(@"^\bRange\b")},
            {TokenType.OnActivation,new Regex(@"^\bOnActivation\b")},
            {TokenType.Action,new Regex(@"^\bAction\b")},
            {TokenType.Params,new Regex(@"^\bParams\b")},
            {TokenType.Identifier, new Regex("^([a-zA-Z_]\\w*)")},
        };
    }

    public enum TokenType
    {
        // Literals
        Identifier, Number, String, True , False, Null,
        // ArithmeticOperator
        Plus, Plus_Plus, Minus, Minus_Minus,
        Division, Multiplication, Pow,
        // ConcatenationOperator
        Concat, WhiteSpace_Concat, Colon,
        // AssigmentOperator
        Equal,
        // ComparisonOperator
        Equal_Equal, Not_Equal, Greater, Greater_Equal, Less, Less_Equal,
        // LogicalOperator
        And, Or,
        // Delimiter
        Comma, Dot, Semicolon, Left_Brace, Right_Brace, Left_Paren, Right_Paren, Left_Brackets, Right_Brackets,
        // Keyword
        For, In, While, Effect, Card,Print,Var,Name, Type, Faction, Power, Range, OnActivation,Params,Lambda,Action,
        // End
        EOF
    }
}