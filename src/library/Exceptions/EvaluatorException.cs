using System;

namespace Squirrel.Exceptions
{
    public class EvaluatorException : Exception
    {
        public EvaluatorException()
        {
        }

        public EvaluatorException(string message) : base(message)
        {
        }

        public EvaluatorException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
