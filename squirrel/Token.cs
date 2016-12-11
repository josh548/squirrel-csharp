namespace squirrel
{
    public struct Token
    {
        public Category Category;
        public string Lexeme;

        public Token(Category category, string lexeme)
        {
            Category = category;
            Lexeme = lexeme;
        }

        public override string ToString()
        {
            return Lexeme == null
                ? $"{nameof(Category)}: {Category}"
                : $"{nameof(Category)}: {Category}, {nameof(Lexeme)}: \"{Lexeme}\"";
        }
    }
}
