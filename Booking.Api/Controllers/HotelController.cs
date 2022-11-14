
using Booking.Domain.Models;
using Microsoft.AspNetCore.Http;
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
        private readonly ILogger<HotelController> _logger;
        private readonly HttpContext _http;
        public HotelController(ILogger<HotelController> logger, IHttpContextAccessor httpContextAccessor)
        {
            _logger = logger;
            _http = httpContextAccessor.HttpContext;
    }

        [HttpGet]
        public IActionResult GetAllHotels()
        {
            HttpContext.Request.Headers.TryGetValue("my-middleware-header", out var headerDate);
            return Ok(headerDate);
        }

        [Route("{id}")]
        [HttpGet]

        public IActionResult GetHotelById(int id)
        {
           
            return Ok();
        }

        [HttpPost]
        public IActionResult CreateHotel([FromBody] Hotel hotel)
        {
         
    
            return CreatedAtAction(nameof(GetHotelById),new { id = hotel.HotelId}, hotel);
        }

        [Route("{id}")]
        [HttpPut]
        public IActionResult UpdateHotel([FromBody] Hotel updated, int id)
        {


            return NoContent();
        }

        [Route("{id}")]
        [HttpDelete]
        public IActionResult DeleteHotel(int id)
        {
      
            return NoContent();
        }

        
    }
}
