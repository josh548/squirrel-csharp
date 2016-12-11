using System;
using System.Collections.Generic;
using static squirrel.Category;

namespace squirrel
{
    public class Tokenizer
    {
        private readonly string _text;
        private int _index;
        private char? _current, _next;

        public Tokenizer(string text)
        {
            _text = text;
            _current = _text[_index];
            _next = Peek();
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
                _next = Peek();
            }
            else
            {
                _current = null;
                _next = null;
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
            do
            {
                Advance();
            } while (_current.HasValue && _current.Value != ']');

            Advance();
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

                if (char.IsDigit(_current.Value))
                {
                    return new Token(Integer, ReadInteger());
                }

                if (_current.Value == '+' || _current.Value == '-')
                {
                    if (_next.HasValue && char.IsDigit(_next.Value))
                    {
                        return new Token(Integer, ReadInteger());
                    }
                }

                if (char.IsLetter(_current.Value))
                {
                    return new Token(Word, ReadWord());
                }

                switch (_current.Value)
                {
                    case '(':
                    {
                        var token = new Token(LeftParenthesis, "(");
                        Advance();
                        return token;
                    }
                    case ')':
                    {
                        var token = new Token(RightParenthesis, ")");
                        Advance();
                        return token;
                    }
                    case '{':
                    {
                        var token = new Token(LeftCurlyBrace, "{");
                        Advance();
                        return token;
                    }
                    case '}':
                    {
                        var token = new Token(RightCurlyBrace, "}");
                        Advance();
                        return token;
                    }
                    default:
                        throw new Exception($"invalid character found at index {_index}: '{_current.Value}'");
                }
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
