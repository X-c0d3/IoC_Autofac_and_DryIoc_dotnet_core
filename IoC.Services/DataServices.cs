using IoC.Interfaces;
using IoC.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace IoC.Services
{
    public class DataServices : IDataServices
    {
        public List<Hotel> GetAllData()
        {
            var res = new List<Hotel>();
            for (int i = 1; i <= 20; i++)
            {
                res.Add(new Hotel
                {
                    HotelId = i,
                    HotelName = "Hotel " + i,
                    IsActive = true
                });
        
            }
            return res;
        }
    }
}
