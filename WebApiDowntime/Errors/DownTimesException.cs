namespace WebApiDowntime.Errors
{
    public class DownTimesException : Exception
    {
        public string ErrorCode { get; }

        public DownTimesException(string message, string errorCode)
            : base(message)
        {
            ErrorCode = errorCode;
        }

        public DownTimesException(string message, string errorCode, Exception innerException)
            : base(message, innerException)
        {
            ErrorCode = errorCode;
        }
    }
}
