using Ioc.Repository.Repositories.Models.DTO;
using Ioc.Repository.Repositories.Models.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ioc.Repository.Repositories.Models.DataManager {
    public class HotelDataManager : IDataRepository<Hotel, HotelDto> {

        readonly EF_DEMOContext _DEMOContext;
        public HotelDataManager(EF_DEMOContext eF_DEMOContext) {
            _DEMOContext = eF_DEMOContext;
        }
        public void Add(Hotel entity) {
            throw new NotImplementedException();
        }

        public void Delete(Hotel entity) {
            throw new NotImplementedException();
        }

        public Hotel Get(int id) {
            _DEMOContext.ChangeTracker.LazyLoadingEnabled = false;

            var hotel = _DEMOContext.Hotel
                .SingleOrDefault(b => b.HotelId == id);

            if (hotel == null) {
                return null;
            }

            //_DEMOContext.Entry(hotel)
            //    .Collection(b => b.City)
            //    .Load();

            _DEMOContext.Entry(hotel)
               .Reference(b => b.City)
               .Load();


            //Many use : Collection
            return hotel;
        }

        public IEnumerable<Hotel> GetAll() {
            throw new NotImplementedException();
        }

        public HotelDto GetDto(int id) {
            throw new NotImplementedException();
        }

        public void Update(Hotel entityToUpdate, Hotel entity) {
            throw new NotImplementedException();
        }
    }
}
