using Ioc.Repository.Repositories.Models;
using IoC.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
            return await this._IDataServices.GetAllData();
        }


        public async Task<Hotel> GetHotelById(int hotelId)
        {
            return await this._IDataServices.GetHotelById(hotelId);
        }
    }
}
