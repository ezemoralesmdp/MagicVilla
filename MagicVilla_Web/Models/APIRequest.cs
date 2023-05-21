using static MagicVilla_Utils.DS;

namespace MagicVilla_Web.Models
{
    public class APIRequest
    {
        public APIType APIType { get; set; } = APIType.GET;
        public string Url { get; set; }
        public object Data { get; set; }
        public string Token { get; set; }
        public Parameters Parameters { get; set; }
    }

    public class Parameters
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
    }
}
