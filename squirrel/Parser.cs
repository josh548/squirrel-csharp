using System.Collections.Generic;
using Squirrel.Exceptions;
using Squirrel.Nodes;
using Squirrel.Tokens;

namespace Squirrel
{
    public class Parser
    {
        private readonly List<Token> _tokens;
        private int _offset;
        private Token? _currentToken;

        public Parser(List<Token> tokens)
        {
            _tokens = tokens;
            _currentToken = GetNextToken();
        }

        private Token? GetNextToken()
        {
            if (_offset < _tokens.Count)
            {
                return _tokens[_offset++];
            }
            return null;
        }

        private void Consume(TokenType expected)
        {
            if (!_currentToken.HasValue)
            {
                throw new ParserException($"expected {expected} but reached end of file");
            }

            var actual = _currentToken.Value.Type;

            if (actual == expected)
            {
                _currentToken = GetNextToken();
            }
            else
            {
                throw new ParserException($"expected {expected} but found {actual}: \"{_currentToken.Value.Lexeme}\"");
            }
        }

        public INode Parse()
        {
            var result = Expression();
            if (_currentToken.HasValue)
            {
                throw new ParserException("expected end of file after expression");
            }
            return result;
        }

        private INode Expression()
        {
            if (!_currentToken.HasValue)
            {
                throw new ParserException("expected expression before end of file");
            }

            var actual = _currentToken.Value.Type;

            switch (actual)
            {
                case TokenType.LeftCurlyBrace:
                    return QuotedExpression();
                case TokenType.LeftParenthesis:
                    return SymbolicExpression();
                case TokenType.String:
                    return String();
                case TokenType.Symbol:
                    return Symbol();
                case TokenType.Integer:
                    return Integer();
                default:
                    throw new ParserException(
                        $"expected {TokenType.LeftCurlyBrace}, {TokenType.LeftParenthesis}, {TokenType.Symbol}, or " +
                        $"{TokenType.Integer} but found {actual}: \"{_currentToken.Value.Lexeme}\"");
            }
        }

        private INode QuotedExpression()
        {
            Consume(TokenType.LeftCurlyBrace);

            var children = new List<INode>();
            while (_currentToken.HasValue && (_currentToken.Value.Type != TokenType.RightCurlyBrace))
            {
                children.Add(Expression());
            }

            Consume(TokenType.RightCurlyBrace);

            return new QuotedExpressionNode(children);
        }

        private INode SymbolicExpression()
        {
            Consume(TokenType.LeftParenthesis);

            var children = new List<INode>();
            while (_currentToken.HasValue && (_currentToken.Value.Type != TokenType.RightParenthesis))
            {
                children.Add(Expression());
            }

            Consume(TokenType.RightParenthesis);

            return new SymbolicExpressionNode(children);
        }

        private INode String()
        {
            var token = _currentToken;
            Consume(TokenType.String);
            // ReSharper disable once PossibleInvalidOperationException
            return new StringNode(token.Value.Lexeme);
        }

        private INode Symbol()
        {
            var token = _currentToken;
            Consume(TokenType.Symbol);
            // ReSharper disable once PossibleInvalidOperationException
            return new SymbolNode(token.Value.Lexeme);
        }

        private INode Integer()
        {
            var token = _currentToken;
            Consume(TokenType.Integer);
            // ReSharper disable once PossibleInvalidOperationException
            return new IntegerNode(int.Parse(token.Value.Lexeme));
        }
    }
}
