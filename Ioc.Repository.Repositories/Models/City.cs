using System;
using System.Collections.Generic;

namespace Ioc.Repository.Repositories.Models
{
    public partial class City
    {
        public City()
        {
            Hotel = new HashSet<Hotel>();
        }

        public int CityId { get; set; }
        public string CountryCode { get; set; }
        public string Name { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }

        public Country CountryCodeNavigation { get; set; }
        public ICollection<Hotel> Hotel { get; set; }
    }
}
