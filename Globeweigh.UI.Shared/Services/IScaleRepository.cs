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
    public interface IScaleRepository
    {
        Task AddOperatorIdToScale(int? operatorId, int scaleId);
        Task<List<Scale>> GetScales();
        Task<Scale> DeletedScaleAsync(int scaleId);
        Task<Scale> AddUpdateScaleAsync(Scale scale);
        Task<Scale> GetScale(int scaleId);
        Task<List<Scale>> GetAllScales();
        Task<bool> RefreshOperatorsForDisplayDevice(List<Scale> scaleList);
    }

    public class ScaleRepository : IScaleRepository
    {
        public async Task<bool> RefreshOperatorsForDisplayDevice(List<Scale> scaleList)
        {
            using (var context = new GlobeweighEntities(GlobalVariables.ConnectionString))
            {
                var masterScales = await context.Scales.Where(a => a.Active).ToListAsync();
                bool changes = false;
                foreach (var scale in masterScales)
                {
                    var matchedScale = scaleList.FirstOrDefault(a => a.id == scale.id);
                    if (matchedScale == null) continue;
                    matchedScale.OperatorChanged = false;
                    if (scale.OperatorId != matchedScale.OperatorId)
                    {
                        matchedScale.OperatorId = scale.OperatorId;
                        matchedScale.OperatorChanged = true;
                        changes = true;
                    }
                }
                return changes;
            }
        }


        public async Task<Scale> DeletedScaleAsync(int scaleId)
        {
            using (var context = new GlobeweighEntities(GlobalVariables.ConnectionString))
            {
                var scale = await context.Scales.FirstOrDefaultAsync(a => a.id == scaleId);
                if (scale != null)
                {

                    context.Scales.Remove(scale);
                    context.Entry(scale).State = EntityState.Deleted;
                    await context.SaveChangesAsync();
                }
                return scale;
            }
        }

        public async Task<Scale> GetScale(int scaleId)
        {
            using (var context = new GlobeweighEntities(GlobalVariables.ConnectionString))
            {
                return await context.Scales.Where(a => a.id == scaleId).FirstOrDefaultAsync();
            }
        }


        public async Task<Scale> AddUpdateScaleAsync(Scale scale)
        {
            using (var context = new GlobeweighEntities(GlobalVariables.ConnectionString))
            {
                if (scale.id == 0)
                {
                    context.Scales.Add(scale);
                }
                else
                {
                    context.Scales.Attach(scale);
                    context.Entry(scale).State = EntityState.Modified;
                }
                await context.SaveChangesAsync();
                return scale;
            }
        }

        public async Task<List<Scale>> GetAllScales()
        {
            using (var context = new GlobeweighEntities(GlobalVariables.ConnectionString))
            {
                var scales = await context.Scales.OrderBy(a => a.ScaleNumber).ToListAsync();
                foreach (var scale in scales)
                {
                    scale.IsVisible = true;
                }
                return scales;
            }
        }


        public async Task<List<Scale>> GetScales()
        {
            using (var context = new GlobeweighEntities(GlobalVariables.ConnectionString))
            {
                var scales = await context.Scales.Where(a => a.Active).OrderBy(a => a.ScaleNumber).ToListAsync();
                foreach (var scale in scales)
                {
                    scale.IsVisible = true;
                }
                return scales;
            }
        }

        public async Task AddOperatorIdToScale(int? operatorId, int scaleId)
        {
            using (var context = new GlobeweighEntities(GlobalVariables.ConnectionString))
            {
                var scale = await context.Scales.Where(a => a.id == scaleId).FirstOrDefaultAsync();
                if (scale == null) return;
                if (scale.OperatorId == operatorId) return;
                scale.OperatorId = operatorId;
                context.Scales.Attach(scale);
                context.Entry(scale).State = EntityState.Modified;
                await context.SaveChangesAsync();
            }
        }

    }
}
