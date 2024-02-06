using RainfallAPI.Models;
using System.Net;

namespace RainfallAPI.Exceptions
{
    public class ErrorRequestException : Exception
    {
        public int? StatusCode { get; }
        public Error Error { get; }

        public ErrorRequestException(int statusCode, Error error)
        {
            StatusCode = statusCode;
            Error = error;
        }
    }
}
