using BookStoreApi.Contracts;
using BookStoreApi.Contracts.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BookStoreAPi.DAL.Interfaces
{
    public interface IGenericRepository<T> where T : BaseEntity
    {
        Task<Result<T>> Add(T entity);
        Task<Result> AddRange(IEnumerable<T> entities);

        Result DeleteEntity(T entity);
        Task<Result> DeleteById(long id);
        Result Update(T entity);
        Task<Result<T>> GetById(long id);

        Task<Result<IList<T>>> ListAsync(Expression<Func<T, bool>> expression = null, List<string> includes = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null);

        Task<Result<T>> GetByExpression(Expression<Func<T, bool>> expression, List<string> includes = null);

    }

}
