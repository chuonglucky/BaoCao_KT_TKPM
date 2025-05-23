using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using ASC.DataAccess.Interfaces;
using ASC.Model.BaseTypes;
using Microsoft.EntityFrameworkCore;

namespace ASC.DataAccess
{
    public class Repository<T> : IRepository<T> where T : BaseEntity, new()
    {
        private readonly DbContext _dbContext;

        public Repository(DbContext dbContext)
        {
            this._dbContext = dbContext;
        }

        public async Task<T> AddAsync(T entity)
        {
            entity.CreatedDate = DateTime.UtcNow;
            entity.UpdatedDate = DateTime.UtcNow;
            await _dbContext.Set<T>().AddAsync(entity);
            return entity;
        }

        public void Update(T entity)
        {
            entity.UpdatedDate = DateTime.UtcNow;
            _dbContext.Set<T>().Update(entity);
        }

        public void Delete(T entity)
        {
            entity.IsDeleted = true;
            entity.UpdatedDate = DateTime.UtcNow;
            _dbContext.Set<T>().Update(entity);
        }

        public async Task<T> FindAsync(Guid id)
        {
            return await _dbContext.Set<T>().FindAsync(id);
        }

        public async Task<IEnumerable<T>> FindAllByKeyAsync(string key)
        {
            return await _dbContext.Set<T>()
                .Where(t => t.CreatedBy == key || t.Id.ToString() == key)
                .ToListAsync();
        }

        public async Task<IEnumerable<T>> FindAllAsync()
        {
            return await _dbContext.Set<T>().Where(t => !t.IsDeleted).ToListAsync();
        }

        // Overload cũ (có thể giữ lại hoặc bỏ đi nếu overload mới đã bao phủ)
        public async Task<IEnumerable<T>> FindAllByQuery(Expression<Func<T, bool>> filter)
        {
            // Gọi overload mới với các tham số mặc định
            return await FindAllByQuery(filter, null, null);
        }

        public async Task<IEnumerable<T>> FindAllInAuditByQuery(Expression<Func<T, bool>> filter)
        {
          
            return await _dbContext.Set<T>()
                .Where(t => !t.IsDeleted) // Giữ lại logic này nếu audit cũng chỉ trên non-deleted items
                .Where(filter)
                .ToListAsync();
        }

        public async Task<IEnumerable<T>> FindAllByQuery(
            Expression<Func<T, bool>> filter,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            Func<IQueryable<T>, IQueryable<T>> include = null)
        {
            IQueryable<T> query = _dbContext.Set<T>();

            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (include != null)
            {
                query = include(query);
            }

            if (orderBy != null)
            {
                return await orderBy(query).ToListAsync();
            }
            return await query.ToListAsync();
        }

        public IQueryable<T> GetQueryable() // << TRIỂN KHAI PHƯƠNG THỨC NÀY
        {
            return _dbContext.Set<T>();
        }
    }
}
