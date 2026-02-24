using Application.Interfaces.Repositories;
using Domain.Common;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Infrastructure.Data.Repositories
{
    public class GenericRepository<T>(ApplicationDbContext context) : IGenericRepository<T> where T : BaseEntity
    {
     
        public Task<T?> GetByIdAsync(int id) => context.Set<T>().AsNoTracking().FirstOrDefaultAsync(e => e.Id == id);
        public async Task<IEnumerable<T>> GetAllAsync() => await context.Set<T>().AsNoTracking().ToListAsync();

        public async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate) => await context.Set<T>().Where(predicate).AsNoTracking().ToListAsync();
        public async Task AddAsync(T entity) => await context.Set<T>().AddAsync(entity);

        public void Update(T entity) => context.Set<T>().Update(entity);

        public void Delete(T entity) => context.Set<T>().Remove(entity);
    }
}
