using MagicVilla_Web.Models.Dto;

namespace MagicVilla_Web.Services.IServices
{
    public interface IVillaService
    {
        Task<T> GetAll<T>(string token);
        Task<T> Get<T>(int id, string token);
        Task<T> Create<T>(VillaCreateDto dto, string token);
        Task<T> Update<T>(VillaUpdateDto dto, string token);
        Task<T> Remove<T>(int id, string token);
    }
}
