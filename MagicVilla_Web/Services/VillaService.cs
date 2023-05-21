using MagicVilla_Utils;
using MagicVilla_Web.Models;
using MagicVilla_Web.Models.Dto;
using MagicVilla_Web.Services.IServices;

namespace MagicVilla_Web.Services
{
    public class VillaService : BaseService, IVillaService
    {
        public readonly IHttpClientFactory _httpClient;
        private string _villaUrl;
        public VillaService(IHttpClientFactory httpClient, IConfiguration configuration) : base(httpClient)
        {
            this._httpClient = httpClient;
            this._villaUrl = configuration.GetValue<string>("ServiceUrls:API_URL");
        }

        public Task<T> Create<T>(VillaCreateDto dto, string token)
        {
            return SendAsync<T>(new APIRequest()
            {
                APIType = DS.APIType.POST,
                Data = dto,
                Url = _villaUrl + "/api/v1/Villa",
                Token = token
            });
        }

        public Task<T> Get<T>(int id, string token)
        {
            return SendAsync<T>(new APIRequest()
            {
                APIType = DS.APIType.GET,
                Url = _villaUrl + $"/api/v1/Villa/{id}",
                Token = token
            });
        }

        public Task<T> GetAll<T>(string token)
        {
            return SendAsync<T>(new APIRequest()
            {
                APIType = DS.APIType.GET,
                Url = _villaUrl + $"/api/v1/Villa",
                Token = token
            });
        }

        public Task<T> GetAllPaginated<T>(string token, int pageNumber = 1, int pageSize = 4)
        {
            return SendAsync<T>(new APIRequest()
            {
                APIType = DS.APIType.GET,
                Url = _villaUrl + $"/api/v1/Villa/VillasPaginated",
                Token = token,
                Parameters = new Parameters() { PageNumber = pageNumber, PageSize = pageSize }
            });
        }

        public Task<T> Remove<T>(int id, string token)
        {
            return SendAsync<T>(new APIRequest()
            {
                APIType = DS.APIType.DELETE,
                Url = _villaUrl + $"/api/v1/Villa/{id}",
                Token = token
            });
        }

        public Task<T> Update<T>(VillaUpdateDto dto, string token)
        {
            return SendAsync<T>(new APIRequest()
            {
                APIType = DS.APIType.PUT,
                Data = dto,
                Url = _villaUrl + $"/api/v1/Villa/{dto.Id}",
                Token = token
            });
        }
    }
}
