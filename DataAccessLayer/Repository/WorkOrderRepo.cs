using DataAccessLayer.Interfaces;
using DataEntity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Models.ResponseModels;
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

        public Task<int> Add(WorkOrder entity)
        {
            throw new NotImplementedException();
        }

        public Task<int> BulkInsert(List<WorkOrder> entities)
        {
            throw new NotImplementedException();
        }

        public Task<int> Delete(WorkOrder entity)
        {
            throw new NotImplementedException();
        }

        public EntityEntry ExplicitLoad(object entity)
        {
            throw new NotImplementedException();
        }

        public async Task<WorkOrderAssigned> FilterData()
        {
            //set db to workorder
            var obj = context.WorkOrders;
            //filter data by running the normal ado .ner
            using(var con = context.Database.GetDbConnection())
            {
                await con.OpenAsync();
                using (var command = con.CreateCommand())
                {
                    command.CommandText = "DELETE FROM [Blogs]";
                    var result = await command.ExecuteNonQueryAsync();
                }
            }
            return null;
        }

        public IQueryable<WorkOrder> Get(Expression<Func<WorkOrder, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public IQueryable<WorkOrder> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<int> Update(WorkOrder entity)
        {
            throw new NotImplementedException();
        }
    }
}
