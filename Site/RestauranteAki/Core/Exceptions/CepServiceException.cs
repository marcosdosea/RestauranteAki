namespace Core.Exceptions
{
    public class CepServiceException : Exception
    {
        public CepServiceException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}