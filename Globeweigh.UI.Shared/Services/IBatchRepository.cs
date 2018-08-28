using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Globeweigh.Model;
using System.Data.Entity;
using System;
using Globeweigh.UI.Shared.Helpers;

namespace Globeweigh.UI.Shared.Services
{
    public interface IBatchRepository
    {
        Task<List<vwBatchView>> GetBatchesAsync(DateTime dateFrom, DateTime dateTo);
        Task<Batch> GetBatchAsync(int id);
        Task<Batch> AddUpdateBatchAsync(Batch Batch);
        Task<Batch> CloseOpenBatch(int id, bool closed);
        Task<bool> MarkBatchAsDeletedAsync(int BatchId);
        Task<Batch_OperatorTime> AddBatchOperatorTimeAsync(int batchId, int operatorId);
        Task<Batch_OperatorTime> UpdateTimeElapsedAsync(int batchId, int operatorId, long ticks);
        Task<Batch> UpdateBatchTimeElapsedAsync(int batchId, long ticks, int fiftyCount);
        List<spOperatorBatch_Result> GetOperatorBatchSummary(int batchId);
        Task<vwBatchView> GetLiveBatchAsync();
        Task<Batch> UpdateBatchLiveFlagAsync(int batchId, bool isLive);
        Task<bool> IsBatchLive(int batchId);
    }



    public class BatchRepository : IBatchRepository
    {
        public async Task<Batch> UpdateBatchLiveFlagAsync(int batchId, bool isLive)
        {

            using (var context = new GlobeweighEntities(GlobalVariables.ConnectionString))
            {
                //check to see if there are any batches left Live in error
                if (isLive)
                {
                    var batchLive = await context.Batches.Where(a => a.IsLive).FirstOrDefaultAsync();
                    if (batchLive != null)
                    {
                        batchLive.IsLive = false;
                        context.Batches.Attach(batchLive);
                        context.Entry(batchLive).State = EntityState.Modified;
                        //                        await context.SaveChangesAsync();
                    }
                }

                var batch = await context.Batches.Where(a => a.id == batchId).FirstOrDefaultAsync();
                if (batch == null) return null;
                batch.IsLive = isLive;
                context.Batches.Attach(batch);
                context.Entry(batch).State = EntityState.Modified;
                await context.SaveChangesAsync();
                return batch;
            }
        }

        public async Task<vwBatchView> GetLiveBatchAsync()
        {
            using (var context = new GlobeweighEntities(GlobalVariables.ConnectionString))
            {
                return await context.vwBatchViews.FirstOrDefaultAsync(c => c.IsLive);
            }
        }

        public async Task<bool> IsBatchLive(int batchId)
        {
            using (var context = new GlobeweighEntities(GlobalVariables.ConnectionString))
            {
                var batch = await context.vwBatchViews.FirstOrDefaultAsync(c => c.id == batchId);
                if (batch == null) return false;
                return batch.IsLive;
            }
        }

        public async Task<List<vwBatchView>> GetBatchesAsync(DateTime dateFrom, DateTime dateTo)
        {
            try
            {
                using (var context = new GlobeweighEntities(GlobalVariables.ConnectionString))
                {
                    return await context.vwBatchViews.Where(c => c.DateCreated >= dateFrom && c.DateCreated <= dateTo && !c.Deleted).OrderByDescending(a => a.DateCreated).ToListAsync();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

        }

        public async Task<Batch_OperatorTime> AddBatchOperatorTimeAsync(int batchId, int operatorId)
        {
            using (var context = new GlobeweighEntities(GlobalVariables.ConnectionString))
            {
                Batch_OperatorTime batchOperatorTime = new Batch_OperatorTime();
                batchOperatorTime.BatchId = batchId;
                batchOperatorTime.OperatorId = operatorId;
                batchOperatorTime.DateCreated = DateTime.Now;
                context.Batch_OperatorTime.Add(batchOperatorTime);

                try
                {
                    context.SaveChanges();
                }
                catch (Exception ex)
                {
                }

                return batchOperatorTime;
            }
        }

        public async Task<Batch> GetBatchAsync(int id)
        {
            using (var context = new GlobeweighEntities(GlobalVariables.ConnectionString))
            {
                return await context.Batches.FirstOrDefaultAsync(c => c.id == id);
            }
        }

        public async Task<Batch> AddUpdateBatchAsync(Batch Batch)
        {
            using (var context = new GlobeweighEntities(GlobalVariables.ConnectionString))
            {
                if (Batch.id == 0)
                {
                    Batch.DateCreated = DateTime.Now;
                    context.Batches.Add(Batch);
                }
                else
                {
                    context.Batches.Attach(Batch);
                    context.Entry(Batch).State = EntityState.Modified;
                }
                await context.SaveChangesAsync();
                return Batch;
            }
        }

        public async Task<Batch_OperatorTime> UpdateTimeElapsedAsync(int batchId, int operatorId, long ticks)
        {
            using (var context = new GlobeweighEntities(GlobalVariables.ConnectionString))
            {
                var batch = await context.Batch_OperatorTime.Where(a => a.BatchId == batchId && a.OperatorId == operatorId).FirstOrDefaultAsync();
                if (batch == null) return null;
                batch.TimeElapsedTicks = ticks;
                context.Batch_OperatorTime.Attach(batch);
                context.Entry(batch).State = EntityState.Modified;
                await context.SaveChangesAsync();
                return batch;
            }
        }
        public async Task<Batch> UpdateBatchTimeElapsedAsync(int batchId, long ticks, int fifytyCount)
        {
            using (var context = new GlobeweighEntities(GlobalVariables.ConnectionString))
            {
                var batch = await context.Batches.Where(a => a.id == batchId).FirstOrDefaultAsync();
                if (batch == null) return null;
                batch.TimeElapsedTicks = ticks;
                //batch.FiftyCount = fifytyCount;
                context.Batches.Attach(batch);
                context.Entry(batch).State = EntityState.Modified;
                await context.SaveChangesAsync();
                return batch;
            }
        }

        public async Task<Batch> CloseOpenBatch(int id, bool closed)
        {

            using (var context = new GlobeweighEntities(GlobalVariables.ConnectionString))
            {
                var batch = await context.Batches.Where(a => a.id == id).FirstOrDefaultAsync();
                if (batch == null) return null;
                batch.Closed = closed;
                if (closed)
                    batch.DateClosed = DateTime.Now;
                else
                    batch.DateClosed = null;

                context.Batches.Attach(batch);
                context.Entry(batch).State = EntityState.Modified;
                await context.SaveChangesAsync();
                return batch;
            }
        }

        public async Task<bool> MarkBatchAsDeletedAsync(int BatchId)
        {
            using (var context = new GlobeweighEntities(GlobalVariables.ConnectionString))
            {
                var batchToUpdate = await context.Batches.FirstOrDefaultAsync(a => a.id == BatchId);
                if (batchToUpdate != null)
                {
                    batchToUpdate.Deleted = true;
                    batchToUpdate.DateDeleted = DateTime.Now;
                    context.Batches.Attach(batchToUpdate);
                    context.Entry(batchToUpdate).State = EntityState.Modified;
                    await context.SaveChangesAsync();
                }
                return true;
            }
        }

        public List<spOperatorBatch_Result> GetOperatorBatchSummary(int batchId)
        {
            using (var context = new GlobeweighEntities(GlobalVariables.ConnectionString))
            {
                var list = context.spOperatorBatch(batchId).ToList();
                return list;

                return null;
            }
        }
    }
}
