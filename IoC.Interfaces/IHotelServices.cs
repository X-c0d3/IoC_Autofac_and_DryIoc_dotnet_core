using Ioc.Repository.Repositories.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace IoC.Interfaces
{
    public interface IHotelServices
    {
        Task<Hotel> GetHotelById(int hotelId);
        Task<List<Hotel>> GetHotelAll();
    }
}
