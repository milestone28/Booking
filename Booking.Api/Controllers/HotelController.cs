
using AutoMapper;
using Booking.Api.DTOs;
using Booking.Dal;
using Booking.Domain.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Booking.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HotelController : Controller
    {
        private readonly ILogger<HotelController> _logger;
        private readonly HttpContext _http;
        private readonly DataContext _dataContext;
        private readonly IMapper _mapper;
        public HotelController(ILogger<HotelController> logger, IHttpContextAccessor httpContextAccessor, DataContext dataContext, IMapper mapper)
        {
            _logger = logger;
            _http = httpContextAccessor.HttpContext;
            _dataContext = dataContext; 
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllHotels() // when ever we do network that make processor slow we will use async async Task<>
        {
            var hotels = await _dataContext.hotels.ToListAsync(); // we use await and Async

            var hotelsGet = _mapper.Map<List<HotelGetDto>>(hotels); // should be map to List

            return Ok(hotelsGet);
        }

        [Route("{id}")]
        [HttpGet]

        public async Task<IActionResult> GetHotelById(int id)
        {
           var hotel = await _dataContext.hotels.FirstOrDefaultAsync(x => x.HotelId == id);
            if (hotel == null) return NotFound();

            var res = _mapper.Map<HotelGetDto>(hotel); 
            return Ok(res);
        }

        [HttpPost]
        public async Task<IActionResult> CreateHotel([FromBody] HotelCreateDto hotel) // we user HotelCreateDto for the client input instead using Hotel from the domain
        {
            var domainHotel = _mapper.Map<Hotel>(hotel);
            _dataContext.hotels.Add(domainHotel); //this will just add to memory
            await _dataContext.SaveChangesAsync(); //this will save the database

            var hotelGet = _mapper.Map<HotelGetDto>(domainHotel);

           
            

            // this remove because of the automapper injected
            //Hotel domainHotel = new Hotel(); // this will call model from the domain

            //domainHotel.Address = hotel.Address;
            //domainHotel.City = hotel.City;
            //domainHotel.Country = hotel.Country;
            //domainHotel.Name = hotel.Name;
            //domainHotel.Description = hotel.Description;
            //domainHotel.Stars = hotel.Stars;

           
            //HotelGetDto hotelGet = new HotelGetDto();
            //hotelGet.Address = domainHotel.Address;
            //hotelGet.City = domainHotel.City;
            //hotelGet.Country = domainHotel.Country;
            //hotelGet.Name = domainHotel.Name;
            //hotelGet.Description = domainHotel.Description;
            //hotelGet.Stars = domainHotel.Stars;
            //hotelGet.HotelId = domainHotel.HotelId;

            return CreatedAtAction(nameof(GetHotelById), new { id = domainHotel.HotelId}, hotelGet); // after created it will display the created value
            
        }

        [Route("{id}")]
        [HttpPut]
        public async Task<IActionResult> UpdateHotel([FromBody] HotelCreateDto updated, int id)
        {
          
            var toUpdate = _mapper.Map<Hotel>(updated);
            toUpdate.HotelId = id;


           // var Update = await _dataContext.hotels.FirstOrDefaultAsync(h => h.HotelId == toUpdate.HotelId);

            //remove because applied mapper
            //hotel.Stars = updated.Stars;
            //hotel.Description = updated.Description;
            //hotel.Name = updated.Name;

            _dataContext.Update(toUpdate); //updated on memory but not save in database
           await _dataContext.SaveChangesAsync(); //this will save to database always use async
          

            return NoContent();
        }

        [Route("{id}")]
        [HttpDelete]
        public async Task<IActionResult> DeleteHotel(int id)
        {
            var hotel = await _dataContext.hotels.FirstOrDefaultAsync(x => x.HotelId == id);
            if (hotel == null) return NotFound();
            _dataContext.Remove(hotel);  //updated on memory but not save in database
            await _dataContext.SaveChangesAsync(); //this will save to database always use async
            return NoContent();
        }

        
    }
}
