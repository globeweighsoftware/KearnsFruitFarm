using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Globeweigh.Model;

namespace Globeweigh.UI.Shared.Services
{
    public interface IPortionRepository
    {
        Task<List<vwPortionView>> GetPortionsAsync(int batchId);
        List<vwPortionView> GetPortions(int batchId);
        Task<Portion> AddUpdatePortionAsync(Portion portion);
        Task<Portion> AddDummyPortionAsync(int scaleNo, int operatorId, int batchId, int? lowerLimit, int? upperLimit);
        Task<Portion> AddPortionAsync(int scaleNo, int operatorId, int batchId, int weight, int tare);
        List<spOperatorStats_Result> GetOperatorStats(DateTime dateFrom, DateTime dateTo);
    }

    public class PortionRepository : IPortionRepository
    {

        public List<spOperatorStats_Result> GetOperatorStats(DateTime dateFrom, DateTime dateTo)
        {
            using (var context = new GlobeweighEntities(GlobalVariables.ConnectionString))
            {
                return context.spOperatorStats(dateFrom, dateTo).ToList();
            }
        }

        public async Task<List<vwPortionView>> GetPortionsAsync(int batchId)
        {
            using (var context = new GlobeweighEntities(GlobalVariables.ConnectionString))
            {
                return await context.vwPortionViews.Where(a => a.BatchId == batchId && !a.Deleted).OrderByDescending(a => a.id).ToListAsync();
            }
        }

        public List<vwPortionView> GetPortions(int batchId)
        {
            using (var context = new GlobeweighEntities(GlobalVariables.ConnectionString))
            {
                return context.vwPortionViews.Where(a => a.BatchId == batchId && !a.Deleted).OrderByDescending(a => a.id).ToList();
            }
        }

        public async Task<Portion> AddDummyPortionAsync(int scaleNo, int operatorId, int batchId, int? lowerLimit, int? upperLimit)
        {
            Random random = new Random();
            int randomNumber = random.Next((int)lowerLimit, (int)upperLimit);

            var portion = new Portion();
            portion.OperatorId = operatorId;
            portion.ScaleNo = scaleNo;
            portion.BatchId = batchId;
            portion.Weight = randomNumber;
            return await AddUpdatePortionAsync(portion);
        }

        public async Task<Portion> AddPortionAsync(int scaleNo, int operatorId, int batchId, int weight, int tare)
        {
            var portion = new Portion();
            portion.OperatorId = operatorId;
            portion.ScaleNo = scaleNo;
            portion.BatchId = batchId;
            portion.Weight = weight;
            portion.ScaleTare = tare;
            return await AddUpdatePortionAsync(portion);
        }

        public async Task<Portion> AddUpdatePortionAsync(Portion portion)
        {
            using (var context = new GlobeweighEntities(GlobalVariables.ConnectionString))
            {
                if (portion.id == 0)
                {
                    portion.DateCreated = DateTime.Now;
                    context.Portions.Add(portion);
                }
                else
                {
                    context.Portions.Attach(portion);
                    context.Entry(portion).State = EntityState.Modified;
                }
                await context.SaveChangesAsync();
                return portion;
            }
        }

        //        public async Task<bool> MarkPortionAsDeletedAsync(int PortionId)
        //        {
        //            using (var context = new GlobeweighEntities(GlobalVariables.ConnectionString))
        //            {
        //                var PortionToUpdate = await context.Portions.FirstOrDefaultAsync(a => a.id == PortionId);
        //                if (PortionToUpdate != null)
        //                {
        //                    PortionToUpdate.Deleted = true;
        //                    PortionToUpdate.DateDeleted = DateTime.Now;
        //                    context.Portions.Attach(PortionToUpdate);
        //                    context.Entry(PortionToUpdate).State = EntityState.Modified;
        //                    await context.SaveChangesAsync();
        //                }
        //                return true;
        //            }
        //        }
    }
}
