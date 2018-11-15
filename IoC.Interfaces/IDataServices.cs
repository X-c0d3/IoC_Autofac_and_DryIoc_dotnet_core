using Ioc.Repository.Repositories.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace IoC.Interfaces
{
    public interface IDataServices
    {
        Task<Hotel> GetHotelById(int id);
        Task<List<Hotel>> GetAllData();
    }
}
