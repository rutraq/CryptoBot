using System.Net.Http;

namespace LibraryCex
{
    public class InvalidApiKeyException : ApiException
    {
        public InvalidApiKeyException(HttpResponseMessage response, string message) :
            base(response, message)
        {

        }
    }
}
