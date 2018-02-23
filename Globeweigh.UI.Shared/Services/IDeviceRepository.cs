using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Globeweigh.Model;
using System.Data.Entity;

namespace Globeweigh.UI.Shared.Services
{
    public interface IDeviceRepository
    {
        Task<Device> GetCurrentDevice();
        Task<Device> AddDeviceAsync(Device device);
        Task<Device> UpdateDeviceAsync(Device device);
    }

    public class DeviceRepository : IDeviceRepository
    {
        public async Task<Device> GetCurrentDevice()
        {
            using (var context = new GlobeweighEntities(GlobalVariables.ConnectionString))
            {
                return await context.Devices.Where(a => a.DeviceName == Environment.MachineName).FirstOrDefaultAsync();
            }
        }

        public async Task<Device> AddDeviceAsync(Device device)
        {
            using (var context = new GlobeweighEntities(GlobalVariables.ConnectionString))
            {
                context.Devices.Add(device);
                await context.SaveChangesAsync();
            }
            return device;
        }

        public async Task<Device> UpdateDeviceAsync(Device device)
        {
            using (var context = new GlobeweighEntities(GlobalVariables.ConnectionString))
            {
                context.Devices.Attach(device);
                context.Entry(device).State = EntityState.Modified;
                await context.SaveChangesAsync();
                return device;
            }
        }
    }
}
