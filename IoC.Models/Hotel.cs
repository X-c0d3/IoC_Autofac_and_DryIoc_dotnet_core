using System;

namespace IoC.Models
{
    public class Hotel
    {
        public int HotelId { get; set; }
        public string HotelName { get; set; }
        public Room[] Rooms { get; set; }
        public string Address { get; set; }
        public bool IsActive { get; set; }
    }

    public class Room
    {
        public int Adult { get; set; }
        public int Child { get; set; }
        public int[] Infant { get; set; }
    }
}
