namespace squirrel.Tokens
{
    public struct Token
    {
        public readonly TokenType Type;
        public readonly SourceSpan? Span;
        public readonly string Lexeme;

        public Token(TokenType type, SourceSpan? span, string lexeme)
        {
            Type = type;
            Span = span;
            Lexeme = lexeme;
        }

        public override string ToString() => Type == TokenType.EndOfFile
            ? $"{nameof(Type)}: {Type}"
            : $"{nameof(Type)}: {Type}, {nameof(Span)}: {Span}, {nameof(Lexeme)}: \"{Lexeme}\"";
    }
}
