using DataAccessLayer.Repository;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace DataAccessLayer.Interfaces
{
    public interface IRepo<TEntity>
    {
        IQueryable<TEntity> Get(Expression<Func<TEntity, bool>> predicate);

        IQueryable<TEntity> GetAll();

        Task<int> Delete(TEntity entity);

        Task<int> Update(TEntity entity);

        Task<int> Add(TEntity entity);

        AppDBContext GetObj();
    }
}