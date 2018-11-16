using Ioc.Repository.Repositories.Models;
using IoC.Interfaces;
using Microsoft.EntityFrameworkCore;
using StackExchange.Profiling;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IoC.Services
{
    public class DataServices : IDataServices
    {
        readonly EF_DEMOContext eF_DEMOContext;
        public DataServices(EF_DEMOContext eF_DEMOContext)
        {
            this.eF_DEMOContext = eF_DEMOContext;
        }

        public async Task<Hotel> GetHotelById(int id)
        {
            using (MiniProfiler.Current.Step("GetHotelById"))
            {
                return await this.eF_DEMOContext.Hotel
                    .Include(x => x.City)
                    .FirstOrDefaultAsync(x => x.HotelId == id);
            }
        }
        public async Task<List<Hotel>> GetAllData()
        {
            using (MiniProfiler.Current.Step("GetAllData"))
            {
                return await this.eF_DEMOContext.Hotel
                .Include(x => x.City)
                .ToListAsync();
            }
        }
    }
}
