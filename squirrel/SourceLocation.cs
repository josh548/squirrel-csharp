﻿namespace squirrel
{
    public struct SourceLocation
    {
        public readonly int Offset, Line, Column;

        public SourceLocation(int offset, int line, int column)
        {
            Offset = offset;
            Line = line;
            Column = column;
        }

        public override string ToString()
        {
            return $"{Line}:{Column}";
        }
    }
}
