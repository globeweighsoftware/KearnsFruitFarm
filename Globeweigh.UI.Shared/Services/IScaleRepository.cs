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
        Task<bool> RefreshSlaveOperators(List<Scale> scaleList);
        Task<bool> RefreshMasterOperators(List<Scale> scaleList);
        Task<List<Scale>> GetScales();
    }

    public class ScaleRepository : IScaleRepository
    {
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

        public async Task<bool> RefreshSlaveOperators(List<Scale> scaleList)
        {
            using (var context = new GlobeweighEntities(GlobalVariables.ConnectionString))
            {
                var slaveScales = await context.Scales.Where(a => a.Slave).ToListAsync();
                bool changes = false;
                foreach (var scale in slaveScales)
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

        public async Task<bool> RefreshMasterOperators(List<Scale> scaleList)
        {
            using (var context = new GlobeweighEntities(GlobalVariables.ConnectionString))
            {
                var masterScales = await context.Scales.Where(a => !a.Slave).ToListAsync();
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
    }
}
