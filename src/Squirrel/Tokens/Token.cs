namespace Squirrel.Tokens
{
    public struct Token
    {
        public readonly TokenType Type;
        public readonly SourceSpan Span;
        public readonly string Lexeme;

        public Token(TokenType type, SourceSpan span, string lexeme)
        {
            Type = type;
            Span = span;
            Lexeme = lexeme;
        }

        public override string ToString() =>
            $"{nameof(Type)}: {Type}, {nameof(Span)}: {Span}, {nameof(Lexeme)}: \"{Lexeme}\"";
    }
}
