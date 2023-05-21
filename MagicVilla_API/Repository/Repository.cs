using MagicVilla_API.Data;
using MagicVilla_API.Models.Specifications;
using MagicVilla_API.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace MagicVilla_API.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly ApplicationDbContext _db;
        internal DbSet<T> dbSet;

        public Repository(ApplicationDbContext db)
        {
            _db = db;
            this.dbSet = _db.Set<T>();
        }

        public async Task Create(T entity)
        {
            await dbSet.AddAsync(entity);
            await Save();
        }

        public async Task<T> Get(Expression<Func<T, bool>>? filter = null, bool tracked = true, string? propertiesInclude = null)
        {
            IQueryable<T> query = dbSet;

            if (!tracked)
                query = query.AsNoTracking();

            if (filter != null)
                query = query.Where(filter);

            if(propertiesInclude != null)
            {
                foreach(var propInclude in propertiesInclude.Split(new char[] {','}, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(propInclude);
                }
            }

            return await query.FirstOrDefaultAsync();
        }

        public async Task<List<T>> GetAll(Expression<Func<T, bool>>? filter = null, string? propertiesInclude = null)
        {
            IQueryable<T> query = dbSet;

            if (filter != null)
                query = query.Where(filter);

            if (propertiesInclude != null)
            {
                foreach (var propInclude in propertiesInclude.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(propInclude);
                }
            }

            return await query.ToListAsync();
        }

        public PagedList<T> GetAllPaginated(Parameters parameters, Expression<Func<T, bool>> filter = null, string propertiesInclude = null)
        {
            IQueryable<T> query = dbSet;

            if (filter != null)
                query = query.Where(filter);

            if (propertiesInclude != null)
            {
                foreach (var propInclude in propertiesInclude.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(propInclude);
                }
            }

            return PagedList<T>.ToPagedList(query, parameters.PageNumber, parameters.PageSize);
        }

        public async Task Remove(T entity)
        {
            _db.Remove(entity);
            await Save();
        }

        public async Task Save()
        {
            await _db.SaveChangesAsync();
        }
    }
}
