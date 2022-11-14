using Booking.Domain.Models;
using System.Collections.Generic;

namespace Booking.Api
{
    public class DataSource
    {
        public DataSource()
        {
            Hotels = GetHotels();
        }
        public List<Hotel> Hotels { get; set; }

        private List<Hotel> GetHotels()
        {
            return new List<Hotel>
            {
                new Hotel
                {
                    HotelId = 1,
                    Name = "Marco Polo",
                    Stars = 5,
                    Country = "Phillipines",
                    Address = "Claveria",
                    City = "Davao",
                    Description = "this is description"
                },

                new Hotel {
                    HotelId = 2,
                    Name = "Ayala Land",
                    Stars = 4,
                    Country = "Phillipines",
                    Address = "Claveria",
                    City = "Davao",
                    Description = "this is description"

                }
            };
        }
    }
}
