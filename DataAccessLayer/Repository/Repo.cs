using DataAccessLayer.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
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

        public async Task<int> BulkInsert(List<TEntity> entities)
        {

            await this.context.Set<TEntity>().AddRangeAsync(entities);
            return await context.SaveChangesAsync();
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

        public async Task<int> Update(TEntity entity)
        {
            var entityreturned = context.Update<TEntity>(entity);
            var attached = context.Attach<TEntity>(entity: entity);
            attached.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            return await context.SaveChangesAsync();
        }

       


    }
}