using System.Linq.Expressions;

namespace MagicVilla_API.Repository.IRepository
{
    public interface IRepository<T> where T : class
    {
        Task Create(T entity);
        Task<List<T>> GetAll(Expression<Func<T, bool>>? filter = null, string? propertiesInclude = null);
        Task<T> Get(Expression<Func<T, bool>>? filter = null, bool tracked = true, string? propertiesInclude = null);
        Task Remove(T entity);
        Task Save();
    }
}
