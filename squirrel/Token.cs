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
            if (Lexeme == null)
            {
                return $"{nameof(Category)}: {Category}";
            }
            else
            {
                return $"{nameof(Category)}: {Category}, {nameof(Lexeme)}: {Lexeme}";
            }
        }
    }
}
