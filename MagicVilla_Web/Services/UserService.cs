using MagicVilla_Utils;
using MagicVilla_Web.Models;
using MagicVilla_Web.Models.Dto;
using MagicVilla_Web.Services.IServices;

namespace MagicVilla_Web.Services
{
    public class UserService : BaseService, IUserService
    {
        private readonly IHttpClientFactory _httpClient;
        private string _villaUrl;

        public UserService(IHttpClientFactory httpClient, IConfiguration configuration) : base(httpClient)
        {
            _httpClient = httpClient;
            _villaUrl = configuration.GetValue<string>("ServiceUrls:API_URL");
        }

        public Task<T> Login<T>(LoginRequestDto dto)
        {
            return SendAsync<T>(new APIRequest()
            {
                APIType = DS.APIType.POST,
                Data = dto,
                Url = _villaUrl + "/api/user/login"
            });
        }

        public Task<T> Register<T>(RegisterRequestDto dto)
        {
            return SendAsync<T>(new APIRequest()
            {
                APIType = DS.APIType.POST,
                Data = dto,
                Url = _villaUrl + "/api/user/register"
            });
        }
    }
}
