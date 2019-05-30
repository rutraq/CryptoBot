using System.Net.Http;

namespace LibraryCex
{
    public class ApiKeyIsMissingException : ApiException
    {
        public ApiKeyIsMissingException(HttpResponseMessage response, string message) :
            base(response, message)
        {
        }
    }
}
