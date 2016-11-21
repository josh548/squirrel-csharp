using System;
using System.Collections.Generic;
using static squirrel.Category;

namespace squirrel
{
    public class Tokenizer
    {
        private string _text;
        private int _index = 0;
        private char? _current;

        public Tokenizer(string text)
        {
            _text = text;
            _current = _text[_index];
        }

        private char? Peek()
        {
            if (_index + 1 < _text.Length)
            {
                return _text[_index + 1];
            }
            else
            {
                return null;
            }
        }

        private void Advance()
        {
            _index++;

            if (_index < _text.Length)
            {
                _current = _text[_index];
            }
            else
            {
                _current = null;
            }
        }

        private void SkipWhiteSpace()
        {
            while (_current.HasValue && char.IsWhiteSpace(_current.Value))
            {
                Advance();
            }
        }

        private void SkipComment()
        {
            while (_current.HasValue)
            {
                Advance();

                if (_current == ']')
                {
                    Advance();
                    break;
                }
            }
        }

        private string ReadInteger()
        {
            var lexeme = "";

            if (_current == '+' || _current == '-')
            {
                lexeme += _current;
                Advance();
            }

            while (_current.HasValue && char.IsDigit(_current.Value))
            {
                lexeme += _current;
                Advance();
            }

            return lexeme;
        }

        private string ReadWord()
        {
            var lexeme = "";

            while (_current.HasValue && char.IsLetter(_current.Value))
            {
                lexeme += _current;
                Advance();
            }

            return lexeme;
        }

        public Token GetNextToken()
        {
            while (_current.HasValue)
            {
                if (char.IsWhiteSpace(_current.Value))
                {
                    SkipWhiteSpace();
                    continue;
                }

                if (_current.Value == '[')
                {
                    SkipComment();
                    continue;
                }

                if (char.IsDigit(_current.Value) || _current.Value == '+' || _current.Value == '-')
                {
                    return new Token(Integer, ReadInteger());
                }

                if (char.IsLetter(_current.Value))
                {
                    return new Token(Word, ReadWord());
                }

                if (_current.Value == '(')
                {
                    var token = new Token(LeftParenthesis, "(");
                    Advance();
                    return token;
                }

                if (_current.Value == ')')
                {
                    var token = new Token(RightParenthesis, ")");
                    Advance();
                    return token;
                }

                if (_current.Value == '{')
                {
                    var token = new Token(LeftCurlyBrace, "{");
                    Advance();
                    return token;
                }

                if (_current.Value == '}')
                {
                    var token = new Token(RightCurlyBrace, "}");
                    Advance();
                    return token;
                }

                throw new Exception($"invalid character found at index {_index}: '{_current.Value}'");
            }

            return new Token(EndOfFile, null);
        }

        public List<Token> GetAllTokens()
        {
            var tokens = new List<Token>();

            while (true)
            {
                var token = GetNextToken();
                tokens.Add(token);
                if (token.Category == EndOfFile)
                {
                    return tokens;
                }
            }
        }
    }
}
