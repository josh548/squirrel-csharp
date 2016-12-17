using System;
using System.Collections.Generic;

namespace squirrel
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

        public AstNode Parse()
        {
            var result = Expression();
            Consume(TokenType.EndOfFile);
            return result;
        }

        private AstNode Expression()
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

        private AstNode QuotedExpression()
        {
            Consume(TokenType.LeftCurlyBrace);

            var children = new List<AstNode>();
            while (_currentToken.Type != TokenType.RightCurlyBrace)
            {
                children.Add(Expression());
            }

            Consume(TokenType.RightCurlyBrace);

            return new AstNode(NodeType.QuotedExpression, children, null);
        }

        private AstNode SymbolicExpression()
        {
            Consume(TokenType.LeftParenthesis);

            var children = new List<AstNode>();
            while (_currentToken.Type != TokenType.RightParenthesis)
            {
                children.Add(Expression());
            }

            Consume(TokenType.RightParenthesis);

            return new AstNode(NodeType.SymbolicExpression, children, null);
        }

        private AstNode Symbol()
        {
            var token = _currentToken;
            Consume(TokenType.Symbol);
            return new AstNode(NodeType.Symbol, null, token.Lexeme);
        }

        private AstNode Integer()
        {
            var token = _currentToken;
            Consume(TokenType.Integer);
            return new AstNode(NodeType.Integer, null, int.Parse(token.Lexeme));
        }
    }
}
