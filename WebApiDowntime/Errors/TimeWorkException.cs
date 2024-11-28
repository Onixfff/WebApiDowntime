namespace WebApiDowntime.Errors
{
    public class TimeWorkException : Exception
    {
        public string ErrorCode { get; }

        public TimeWorkException(string message, string errorCode)
            : base(message)
        {
            ErrorCode = errorCode;
        }

        public TimeWorkException(string message, string errorCode, Exception innerException)
            : base(message, innerException)
        {
            ErrorCode = errorCode;
        }
    }
}
