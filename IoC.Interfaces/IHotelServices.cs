using IoC.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace IoC.Interfaces
{
    public interface IHotelServices
    {
        Hotel GetHotelById(int hotelId);
        List<Hotel> GetHotelAll();
    }
}
