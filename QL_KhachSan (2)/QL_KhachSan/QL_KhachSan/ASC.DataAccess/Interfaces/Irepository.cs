using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using ASC.Model.BaseTypes;

namespace ASC.DataAccess.Interfaces
{
    public interface IRepository<T> where T : BaseEntity
    {
        Task<T> AddAsync(T entity);
        void Update(T entity);
        void Delete(T entity);
        Task<T> FindAsync(Guid id);
        Task<IEnumerable<T>> FindAllByKeyAsync(string key);
        Task<IEnumerable<T>> FindAllAsync();
        Task<IEnumerable<T>> FindAllByQuery(Expression<Func<T, bool>> filter); // Overload cũ
        Task<IEnumerable<T>> FindAllInAuditByQuery(Expression<Func<T, bool>> filter);

        // Overload mới đã có
        Task<IEnumerable<T>> FindAllByQuery(
            Expression<Func<T, bool>> filter,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            Func<IQueryable<T>, IQueryable<T>> include = null
        );

        IQueryable<T> GetQueryable();
    }
}
