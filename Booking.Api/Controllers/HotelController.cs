using Booking.Api.Services;
using Booking.Api.Services.Abstraction;
using Booking.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;

namespace Booking.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HotelController : Controller
    {
        private readonly MyFirstService _myFirstService;
        private readonly ISingletonOperation _singleton;
        private readonly ITransientOperation _transient;
        private readonly IScopedOperation _scoped;
        private readonly ILogger<HotelController> _logger;
        public HotelController(MyFirstService myFirstService, ISingletonOperation singleton, ITransientOperation transient, IScopedOperation scoped, ILogger<HotelController> logger)
        {
            _myFirstService = myFirstService;
            _singleton = singleton;
            _transient = transient;
            _scoped = scoped;
            _logger = logger;
    }

        [HttpGet]
        public IActionResult GetAllHotels()
        {
            _logger.LogInformation($"GUID of singleton: {_singleton.Guid}");
            _logger.LogInformation($"GUID of transient: {_transient.Guid}");
            _logger.LogInformation($"GUID of scoped: {_scoped.Guid}");

            var hotels = _myFirstService.GetHotels();
            return Ok(hotels);
        }

        [Route("{id}")]
        [HttpGet]

        public IActionResult GetHotelById(int id)
        {
            var hotels = _myFirstService.GetHotels();
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
            var hotels = _myFirstService.GetHotels();
            hotels.Add(hotel);

            return CreatedAtAction(nameof(GetHotelById),new { id = hotel.HotelId}, hotel);
        }

        [Route("{id}")]
        [HttpPut]
        public IActionResult UpdateHotel([FromBody] Hotel updated, int id)
        {
            var hotels = _myFirstService.GetHotels();
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
            var hotels = _myFirstService.GetHotels();
            var hotel = hotels.FirstOrDefault(x => x.HotelId == id);

            if (hotel == null)
                return NotFound("No resource with the corresponding ID found");

            hotels.Remove(hotel);
            return NoContent();
        }

        
    }
}
