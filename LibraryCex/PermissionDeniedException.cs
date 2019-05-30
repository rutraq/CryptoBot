using System.Net.Http;

namespace LibraryCex
{
    public class PermissionDeniedException : ApiException
    {
        public PermissionDeniedException(HttpResponseMessage response, string message) :
            base(response, message)
        {
        }
    }
}
