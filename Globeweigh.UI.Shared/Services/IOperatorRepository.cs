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
        Task<Operator> GetOperator(int OperatorId);
        Task<Operator> AddUpdateOperatorAsync(Operator Operator);
        Task<List<Operator>> GetOperatorsForBatch(int batchId);
    }

    public class OperatorRepository : IOperatorRepository
    {
        public async Task<List<Operator>> GetOperatorsForBatch(int batchId)
        {
            using (var context = new GlobeweighEntities(GlobalVariables.ConnectionString))
            {
                return await (from t1 in context.vwOperatorRefDatas
                    from t2 in context.Batch_OperatorTime.Where(a => a.OperatorId == t1.id && a.BatchId == batchId).DefaultIfEmpty()

                    select new Operator()
                    {
                        id = t1.id,
                        TimeElapsed = (long)t2.TimeElapsedTicks,
                        TimeElapsedId = t2.id,
                        ShiftId = t1.ShiftId
                    }).ToListAsync();
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
