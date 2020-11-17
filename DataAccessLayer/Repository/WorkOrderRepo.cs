using DataAccessLayer.Interfaces;
using DataEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Repository
{
    public class WorkOrderRepo : IWorkorderRepo
    {
        private readonly AppDBContext context;
        public WorkOrderRepo(AppDBContext context)
        {
            this.context = context;
        }
        public async Task<int> Add(WorkOrder entity)
        {
             await context.WorkOrders.AddAsync(entity: entity);
            return await context.SaveChangesAsync();
        }

        public Task<int> BulkInsert(List<WorkOrder> entities)
        {
            throw new NotImplementedException();
        }

        public async Task<int> Delete(WorkOrder entity)
        {
             context.WorkOrders.Remove(entity: entity);
            return await context.SaveChangesAsync();
        }

        public IQueryable<WorkOrder> Get(Expression<Func<WorkOrder, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public IQueryable<WorkOrder> GetAll()
        {
            throw new NotImplementedException();
        }

        public async Task<int> Update(WorkOrder entity)
        {
            var workorder= context.WorkOrders.Attach(entity: entity);
            workorder.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            return await context.SaveChangesAsync();
        }
    }
}
