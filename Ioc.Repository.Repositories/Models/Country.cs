using System;
using System.Collections.Generic;

namespace Ioc.Repository.Repositories.Models
{
    public partial class Country
    {
        public Country()
        {
            City = new HashSet<City>();
            Hotel = new HashSet<Hotel>();
        }

        public string CountryCode { get; set; }
        public string CountryName { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }

        public ICollection<City> City { get; set; }
        public ICollection<Hotel> Hotel { get; set; }
    }
}
