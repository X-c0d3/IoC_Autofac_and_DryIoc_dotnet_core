using Ioc.Repository.Repositories.Models;
using IoC.Interfaces;
using StackExchange.Profiling;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IoC.Services
{
    public class HotelServices : IHotelServices
    {
        readonly IDataServices _IDataServices;
        public HotelServices(IDataServices dataServices)
        {
            this._IDataServices = dataServices;
        }

        public async Task<List<Hotel>> GetHotelAll()
        {
            using (MiniProfiler.Current.Step("GetHotelAll"))
            {
                return await this._IDataServices.GetAllData();
            }
        }


        public async Task<Hotel> GetHotelById(int hotelId)
        {
            using (MiniProfiler.Current.Step("GetHotelById"))
            {
                return await this._IDataServices.GetHotelById(hotelId);
            }
        }
    }
}
