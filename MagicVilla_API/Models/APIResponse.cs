using System.Net;

namespace MagicVilla_API.Models
{
    public class APIResponse
    {
        public APIResponse()
        {
            ErrorMessages = new List<string>();
        }

        public HttpStatusCode statusCode { get; set; }
        public bool IsSuccessful { get; set; } = true;
        public List<string> ErrorMessages { get; set; }
        public string SingleErrorMessage { get; set; }
        public object Result { get; set; }
        public int TotalPages { get; set; }
    }
}
