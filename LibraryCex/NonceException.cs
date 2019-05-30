using System.Net.Http;

namespace LibraryCex
{
    public class NonceException : ApiException
    {
        public NonceException(HttpResponseMessage response, string message) :
            base(response, message)
        {
        }
    }
}
