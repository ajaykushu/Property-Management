using DataAccessLayer.Interfaces;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace DataAccessLayer.Repository
{
    public class Repo<TEntity> : IRepo<TEntity> where TEntity : class
    {
        private readonly AppDBContext context;

        public Repo(AppDBContext context)
        {
            this.context = context;
        }

        public Task<int> Add(TEntity entity)
        {
            this.context.Add<TEntity>(entity);
            return this.context.SaveChangesAsync();
        }

        public Task<int> Delete(TEntity entity)
        {
            this.context.Remove<TEntity>(entity);
            return this.context.SaveChangesAsync();
        }

        public IQueryable<TEntity> Get(Expression<Func<TEntity, bool>> predicate)
        {
            IQueryable<TEntity> query = context.Set<TEntity>().Where(predicate);
            return query;
        }

        public IQueryable<TEntity> GetAll()
        {
            IQueryable<TEntity> query = context.Set<TEntity>();
            return query;
        }

        public Task<int> Update(TEntity entity)
        {
            context.Update<TEntity>(entity);
            return context.SaveChangesAsync();
        }

        //public Task<TEntity> ExecuteSql()
        //{
        //    context.ExecuteQuery<TEntity>(sql, "ALFKI");
        //}
    }
}