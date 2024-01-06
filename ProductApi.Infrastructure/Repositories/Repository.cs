using Microsoft.EntityFrameworkCore;
using ProductApi.Domain.Commons.BaseEntities;
using ProductApi.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductApi.Infrastructure.Repositories
{
    public abstract class Repository<T> : IRepository<T> where T : BaseEntity
    {
        private readonly InventoryDbContext _context;
        private readonly DbSet<T> _entities;

        protected Repository(InventoryDbContext context)
        {
            _context = context;
            _entities = context.Set<T>();
        }

        public virtual async Task<IEnumerable<T>> GetAllAsync() => await _entities.AsNoTracking().ToListAsync();

        public virtual async Task<T?> GetByIdAsync(int id) =>
            await _entities.AsNoTracking().SingleOrDefaultAsync(s => s.Id == id);

        public virtual async Task<bool> InsertAsync(T entity)
        {
            if (entity is null)
                throw new ArgumentNullException(nameof(entity));

            await _entities.AddAsync(entity);
            await _context.SaveChangesAsync();

            return true;
        }

        public virtual async Task<bool> UpdateAsync(T entity)
        {
            if (entity is null)
                throw new ArgumentNullException(nameof(entity));

            _entities.Update(entity);
            await _context.SaveChangesAsync();

            return true;
        }

        public virtual async Task<bool> DeleteAsync(T entity)
        {
            if (entity is null)
                throw new ArgumentNullException(nameof(entity));

            _entities.Remove(entity);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
