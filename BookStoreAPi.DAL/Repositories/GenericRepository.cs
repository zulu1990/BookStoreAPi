using BookStoreApi.Contracts;
using BookStoreApi.Contracts.Entity;
using BookStoreAPi.DAL.Database;
using BookStoreAPi.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BookStoreAPi.DAL.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
    {
        private readonly DbSet<T> _dbSet;

        public GenericRepository(ApplicationDbContext context)
        {

            _dbSet = context.Set<T>();

        }
        public async Task<Result<T>> Add(T entity)
        {
            var addedEntity = await _dbSet.AddAsync(entity);
            return Result<T>.Succeed(addedEntity.Entity);
        }

        public async Task<Result> AddRange(IEnumerable<T> entities)
        {
            await _dbSet.AddRangeAsync(entities);
            return Result.Succeed();
        }

        public Result DeleteEntity(T entity)
        {
            try
            {
                var result = _dbSet.Remove(entity);
                return Result.Succeed();
            }
            catch (Exception e)
            {
                return Result.Fail(e.Message);
            }
        }

        public async Task<Result> DeleteById(long id)
        {
            var entity = await _dbSet.FindAsync(id);

            if (entity != null)
                return DeleteEntity(entity);

            return Result.Fail("NotFound");

        }
        public Result Update(T entity)
        {
            try
            {
                var result = _dbSet.Update(entity);
                return Result.Succeed();
            }
            catch (Exception e)
            {
                return Result.Fail(e.Message);
            }
        }
        public async Task<Result<T>> GetById(long id)
        {
            var foundEntity = await _dbSet.FindAsync(id);

            return foundEntity == null ? Result<T>.Fail("Not Found") : Result<T>.Succeed(foundEntity);
        }

        public async Task<Result<IList<T>>> ListAsync(Expression<Func<T, bool>> expression = null, List<string> includes = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null)
        {
            IQueryable<T> query = _dbSet;

            includes?.ForEach(x => query.Include(x));

            if (expression != null)
                query = query.Where(expression);

            if (orderBy != null)
                query = orderBy(query);

            var result = await query.AsNoTracking().ToListAsync();

            return result.Count > 0 ? Result<IList<T>>.Succeed(result) : Result<IList<T>>.Fail("Not Found");
        }

        public async Task<Result<T>> GetByExpression(Expression<Func<T, bool>> expression, List<string> includes = null)
        {
            IQueryable<T> query = _dbSet;
            if (includes != null)
            {
                query = includes.Aggregate(query, (current, include) => current.Include(include));
            }

            var item = await query.AsNoTracking().FirstOrDefaultAsync(expression);

            return item != null ? Result<T>.Succeed(item) : Result<T>.Fail("Not Found");
        }
    }

}
