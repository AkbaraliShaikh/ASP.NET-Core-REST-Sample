using Core.Api.DB;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Core.Api.Services
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly CoreDbContext _coreDbContext;

        public Repository(CoreDbContext coreDbContext)
        {
            _coreDbContext = coreDbContext;
        }

        public async Task<IList<T>> GetAsync()
        {
            return await _coreDbContext.Set<T>().ToListAsync();
        }

        public async Task SaveAsync(IList<T> entity)
        {
            await _coreDbContext.Set<T>().AddRangeAsync(entity);
            await _coreDbContext.SaveChangesAsync();
        }

        public async Task SaveAsync(T entity)
        {
            await _coreDbContext.Set<T>().AddAsync(entity);
            await _coreDbContext.SaveChangesAsync();
        }

        public async Task UpdateAsync(T entity)
        {
            _coreDbContext.Set<T>().Update(entity);
            await _coreDbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(T entity)
        {
            _coreDbContext.Set<T>().Remove(entity);
            await _coreDbContext.SaveChangesAsync();
        }
    }
}
