using IoC.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace IoC.Interfaces
{
    public interface IDataServices
    {
        List<Hotel> GetAllData();
    }
}
