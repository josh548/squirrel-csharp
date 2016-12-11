using static squirrel.Category;

namespace squirrel
{
    public struct Token
    {
        public Category Category;
        public SourceSpan? SourceSpan;
        public string Lexeme;

        public Token(Category category, SourceSpan? sourceSpan, string lexeme)
        {
            Category = category;
            SourceSpan = sourceSpan;
            Lexeme = lexeme;
        }

        public override string ToString()
        {
            return Category == EndOfFile
                ? $"{nameof(Category)}: {Category}"
                : $"{nameof(Category)}: {Category}, {nameof(SourceSpan)}: {SourceSpan}, {nameof(Lexeme)}: \"{Lexeme}\"";
        }
    }
}
