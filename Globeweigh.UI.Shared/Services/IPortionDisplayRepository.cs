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
    public interface IPortionDisplayRepository
    {
        Task UpdateDisplayUserList(List<Scale> activeScales);
        Task ClearDisplayUserList();
        Task<List<UserDisplay>> GetDisplayUserList();
    }

    public class PortionDisplayRepository : IPortionDisplayRepository
    {
        public async Task ClearDisplayUserList()
        {

            using (var context = new GlobeweighEntities(GlobalVariables.ConnectionString))
            {
                var displayList = await context.UserDisplays.ToListAsync();
                foreach (var userDisplay in displayList)
                {
                    userDisplay.OperatorName = null;
                    userDisplay.PacksPerMin = 0;
                    userDisplay.PacksPerMin = 0;
                    userDisplay.NoOfPacks = 0;
                    userDisplay.TimeLoggedOn = null;
                }

                await context.SaveChangesAsync();
            }
        }

        public async Task<List<UserDisplay>> GetDisplayUserList()
        {

            using (var context = new GlobeweighEntities(GlobalVariables.ConnectionString))
            {
                var displayList = await context.UserDisplays.Where(a => a.OperatorName != null)
                    .OrderByDescending(a => a.PacksPerMin).ToListAsync();
                return displayList;
            }
        }


        public async Task UpdateDisplayUserList(List<Scale> activeScales)
        {

            using (var context = new GlobeweighEntities(GlobalVariables.ConnectionString))
            {
                var displayList = await context.UserDisplays.ToListAsync();
                foreach (var userDisplay in displayList)
                {
                    var match = activeScales.FirstOrDefault(a => a.ScaleNumber == userDisplay.ScaleNo);
                    if (match != null)
                    {
                        userDisplay.OperatorName = match.OperatorName;
                        userDisplay.PacksPerMin = match.UserPacksPerMin;
                        userDisplay.NoOfPacks = match.UserPackCount;
                        userDisplay.TimeLoggedOn = match.TimeElapsedDisplay;
                    }
                    else
                    {
                        userDisplay.OperatorName = null;
                        userDisplay.PacksPerMin = 0;
                        userDisplay.NoOfPacks = 0;
                        userDisplay.TimeLoggedOn = null;
                    }
                }

                await context.SaveChangesAsync();
            }
        }
    }
}
