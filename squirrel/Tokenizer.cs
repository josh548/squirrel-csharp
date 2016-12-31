using System;
using System.Collections.Generic;
using Squirrel.Exceptions;
using Squirrel.Tokens;

namespace Squirrel
{
    public class Tokenizer
    {
        private readonly string _text;
        private int _offset, _line, _column;
        private char? _current, _next;

        public Tokenizer(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                throw new TokenizerException("empty input");
            }
            _text = text.Replace("\r\n", "\n");
            _current = _text[_offset];
            _next = Peek();
        }

        private SourceLocation GetCurrentLocation() => new SourceLocation(_offset, _line, _column);

        private char? Peek()
        {
            if (_offset + 1 < _text.Length)
            {
                return _text[_offset + 1];
            }
            return null;
        }

        private void Advance()
        {
            if (_current == '\n')
            {
                _line++;
                _column = -1;
            }

            _offset++;
            _column++;

            if (_offset < _text.Length)
            {
                _current = _text[_offset];
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
            var lexeme = string.Empty;

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
            var lexeme = string.Empty;

            while (_current.HasValue)
            {
                if (char.IsLetter(_current.Value) ||
                    (_current.Value == '-' && _next.HasValue && char.IsLetter(_next.Value)))
                {
                    lexeme += _current;
                    Advance();
                }
                else
                {
                    break;
                }
            }

            return lexeme;
        }

        private string ReadCharacter()
        {
            var lexeme = _current?.ToString();
            Advance();
            return lexeme;
        }

        private Token Read(TokenType tokenType, Func<string> readMethod)
        {
            var start = GetCurrentLocation();
            var lexeme = readMethod.Invoke();
            var end = GetCurrentLocation();
            var span = new SourceSpan(start, end);

            return new Token(tokenType, span, lexeme);
        }

        private Token? GetNextToken()
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
                    return Read(TokenType.Integer, ReadInteger);
                }

                if (_current.Value == '+' || _current.Value == '-')
                {
                    if (_next.HasValue && char.IsDigit(_next.Value))
                    {
                        return Read(TokenType.Integer, ReadInteger);
                    }
                }

                if (char.IsLetter(_current.Value))
                {
                    return Read(TokenType.Symbol, ReadWord);
                }

                switch (_current.Value)
                {
                    case '(':
                    {
                        return Read(TokenType.LeftParenthesis, ReadCharacter);
                    }
                    case ')':
                    {
                        return Read(TokenType.RightParenthesis, ReadCharacter);
                    }
                    case '{':
                    {
                        return Read(TokenType.LeftCurlyBrace, ReadCharacter);
                    }
                    case '}':
                    {
                        return Read(TokenType.RightCurlyBrace, ReadCharacter);
                    }
                    default:
                        throw new TokenizerException($"invalid character found at index {_offset}: '{_current.Value}'");
                }
            }

            return null;
        }

        public List<Token> Tokenize()
        {
            var tokens = new List<Token>();

            while (true)
            {
                var token = GetNextToken();

                if (token.HasValue)
                {
                    tokens.Add(token.Value);
                }
                else
                {
                    return tokens;
                }
            }
        }
    }
}
