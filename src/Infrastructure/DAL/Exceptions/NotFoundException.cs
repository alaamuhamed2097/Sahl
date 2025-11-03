using Serilog;

namespace DAL.Exceptions
{
    public class NotFoundException : DataAccessException
    {
        public NotFoundException(string message, ILogger logger) : base(message, logger) { }

        public NotFoundException(string message, Exception inner, ILogger logger) : base(message, inner, logger) { }
    }
}
