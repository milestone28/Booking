using Booking.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace Booking.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HotelController : Controller
    {
        private readonly DataSource _dataSource;

        public HotelController(DataSource dataSource)
        {
            _dataSource = dataSource;
        }

        [HttpGet]
        public IActionResult GetAllHotels()
        {
            var hotels = _dataSource.Hotels;
            return Ok(hotels);
        }

        [Route("{id}")]
        [HttpGet]

        public IActionResult GetHotelById(int id)
        {
            var hotels = _dataSource.Hotels;
            var hotel = hotels.FirstOrDefault(h => h.HotelId == id);
            if(hotel == null)
            {
                return NotFound();
            }
            return Ok(hotel);
        }

        [HttpPost]
        public IActionResult CreateHotel([FromBody] Hotel hotel)
        {
            var hotels = _dataSource.Hotels;
            hotels.Add(hotel);

            return CreatedAtAction(nameof(GetHotelById),new { id = hotel.HotelId}, hotel);
        }

        [Route("{id}")]
        [HttpPut]
        public IActionResult UpdateHotel([FromBody] Hotel updated, int id)
        {
            var hotels = _dataSource.Hotels;
            var oldHotel = hotels.FirstOrDefault(h => h.HotelId == id);

            if (oldHotel == null)
                return NotFound("No resource with the corresponding ID found");

            hotels.Remove(oldHotel);
            hotels.Add(updated);
            return NoContent();
        }

        [Route("{id}")]
        [HttpDelete]
        public IActionResult DeleteHotel(int id)
        {
            var hotels = _dataSource.Hotels;
            var hotel = hotels.FirstOrDefault(x => x.HotelId == id);

            if (hotel == null)
                return NotFound("No resource with the corresponding ID found");

            hotels.Remove(hotel);
            return NoContent();
        }

        
    }
}
