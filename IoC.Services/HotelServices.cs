using IoC.Interfaces;
using IoC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IoC.Services
{
    public class HotelServices : IHotelServices
    {
        readonly IDataServices _IDataServices;
        public HotelServices(IDataServices dataServices)
        {
            this._IDataServices = dataServices;
        }

        public List<Hotel> GetHotelAll()
        {
            return this._IDataServices.GetAllData();
        }

        public Hotel GetHotelById(int hotelId)
        {
            return this._IDataServices.GetAllData()
                .FirstOrDefault(x => x.HotelId == hotelId);
        }

    }
}
