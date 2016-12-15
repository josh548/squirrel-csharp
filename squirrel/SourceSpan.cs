namespace squirrel
{
    public struct SourceSpan
    {
        public readonly SourceLocation Start, End;

        public SourceSpan(SourceLocation start, SourceLocation end)
        {
            Start = start;
            End = end;
        }

        public override string ToString() => $"{Start}-{End}";
    }
}
