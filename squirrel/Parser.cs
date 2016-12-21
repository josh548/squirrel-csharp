using System;
using System.Collections.Generic;
using Squirrel.Nodes;
using Squirrel.Tokens;

namespace Squirrel
{
    public class Parser
    {
        private readonly Tokenizer _tokenizer;
        private Token _currentToken;

        public Parser(Tokenizer tokenizer)
        {
            _tokenizer = tokenizer;
            _currentToken = _tokenizer.GetNextToken();
        }

        private void Consume(TokenType expected)
        {
            var actual = _currentToken.Type;

            if (actual == expected)
            {
                _currentToken = _tokenizer.GetNextToken();
            }
            else
            {
                throw new Exception($"expected token of type {expected} " +
                                    $"but found {actual}: \"{_currentToken.Lexeme}\"");
            }
        }

        public INode Parse()
        {
            var result = Expression();
            Consume(TokenType.EndOfFile);
            return result;
        }

        private INode Expression()
        {
            var actual = _currentToken.Type;

            switch (actual)
            {
                case TokenType.LeftCurlyBrace:
                    return QuotedExpression();
                case TokenType.LeftParenthesis:
                    return SymbolicExpression();
                case TokenType.Symbol:
                    return Symbol();
                case TokenType.Integer:
                    return Integer();
                default:
                    throw new Exception(
                        $"expected token of type {TokenType.LeftCurlyBrace}, {TokenType.LeftParenthesis}, " +
                        $"{TokenType.Symbol}, {TokenType.Integer} but found {actual}: \"{_currentToken.Lexeme}\"");
            }
        }

        private INode QuotedExpression()
        {
            Consume(TokenType.LeftCurlyBrace);

            var children = new List<INode>();
            while (_currentToken.Type != TokenType.RightCurlyBrace)
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
            while (_currentToken.Type != TokenType.RightParenthesis)
            {
                children.Add(Expression());
            }

            Consume(TokenType.RightParenthesis);

            return new SymbolicExpressionNode(children);
        }

        private INode Symbol()
        {
            var token = _currentToken;
            Consume(TokenType.Symbol);
            return new SymbolNode(token.Lexeme);
        }

        private INode Integer()
        {
            var token = _currentToken;
            Consume(TokenType.Integer);
            return new IntegerNode(int.Parse(token.Lexeme));
        }
    }
}
