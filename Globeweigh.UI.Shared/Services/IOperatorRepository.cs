using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Globeweigh.Model;
using Globeweigh.UI.Shared.Helpers;

namespace Globeweigh.UI.Shared.Services
{
    public interface IOperatorRepository
    {
        Task<List<Operator>> GetOperators();
        Task<Operator> MarkOperatorAsDeletedAsync(int OperatorId);
        Task<Operator> GetOperator(int operatorId);
        Task<Operator> AddUpdateOperatorAsync(Operator Operator);
        Task<List<vwOperatorBatch>> GetOperatorsForBatch(int batchId);
    }

    public class OperatorRepository : IOperatorRepository
    {
        public async Task<List<vwOperatorBatch>> GetOperatorsForBatch(int batchId)
        {
            using (var context = new GlobeweighEntities(GlobalVariables.ConnectionString))
            {
                return await context.vwOperatorBatches.Where(a => a.BatchId == batchId || a.BatchId == null).ToListAsync();
            }
        }

        public async Task<List<Operator>> GetOperators()
        {
            using (var context = new GlobeweighEntities(GlobalVariables.ConnectionString))
            {
                return await context.Operators.Where(a=>a.DateDeleted ==null).ToListAsync();
            }
        }

        public async Task<Operator> GetOperator(int OperatorId)
        {
            using (var context = new GlobeweighEntities(GlobalVariables.ConnectionString))
            {
                return await context.Operators.Where(a => a.id == OperatorId).FirstOrDefaultAsync();
            }
        }

        public async Task<Operator> AddUpdateOperatorAsync(Operator Operator)
        {
            using (var context = new GlobeweighEntities(GlobalVariables.ConnectionString))
            {
                if (Operator.id == 0)
                {
                    Operator.DateCreated = DateTime.Now;
                    context.Operators.Add(Operator);
                }
                else
                {
                    context.Operators.Attach(Operator);
                    context.Entry(Operator).State = EntityState.Modified;
                }
                await context.SaveChangesAsync();
                return Operator;
            }
        }

        public async Task<Operator> MarkOperatorAsDeletedAsync(int OperatorId)
        {
            using (var context = new GlobeweighEntities(GlobalVariables.ConnectionString))
            {
                var OperatorToUpdate = await context.Operators.FirstOrDefaultAsync(a => a.id == OperatorId);
                if (OperatorToUpdate != null)
                {
                    OperatorToUpdate.DateDeleted = DateTime.Now;
                    context.Operators.Attach(OperatorToUpdate);
                    context.Entry(OperatorToUpdate).State = EntityState.Modified;
                    await context.SaveChangesAsync();
                }
                return OperatorToUpdate;
            }
        }
    }
}
