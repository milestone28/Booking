
using AutoMapper;
using Booking.Api.DTOs;
using Booking.Dal;
using Booking.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace Booking.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HotelController : Controller
    {
        private readonly DataContext _dataContext;
        private readonly IMapper _mapper;
        public HotelController(DataContext dataContext, IMapper mapper)
        {
            _dataContext = dataContext; 
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllHotels() // when ever we do network that make processor slow we will use async async Task<>
        {
            var hotels = await _dataContext.Hotels.ToListAsync(); // we use await and Async

            var hotelsGet = _mapper.Map<List<HotelGetDto>>(hotels); // should be map to List

            return Ok(hotelsGet);
        }

        [Route("{id}")]
        [HttpGet]

        public async Task<IActionResult> GetHotelById(int id)
        {
           var hotel = await _dataContext.Hotels.FirstOrDefaultAsync(x => x.HotelId == id);
            if (hotel == null) return NotFound();

            var res = _mapper.Map<HotelGetDto>(hotel); 
            return Ok(res);
        }

        [HttpPost]
        public async Task<IActionResult> CreateHotel([FromBody] HotelCreateDto hotel) // we user HotelCreateDto for the client input instead using Hotel from the domain
        {
            var domainHotel = _mapper.Map<Hotel>(hotel);
            _dataContext.Hotels.Add(domainHotel); //this will just add to memory
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
            var hotel = await _dataContext.Hotels.FirstOrDefaultAsync(x => x.HotelId == id);
            if (hotel == null) return NotFound();
            _dataContext.Remove(hotel);  //updated on memory but not save in database
            await _dataContext.SaveChangesAsync(); //this will save to database always use async
            return NoContent();
        }

        [HttpGet]
        [Route("{hotelId}/rooms")]

        public async Task<IActionResult> GetAllHotelRooms(int hotelId)
        {
            var rooms = await _dataContext.Rooms.Where(x => x.HotelId == hotelId).ToListAsync();

            var mappedRooms = _mapper.Map<List<RoomGetDto>>(rooms);

            return Ok(mappedRooms);
        }

        [HttpGet]
        [Route("{hotelId}/rooms/{roomId}")]
        public async Task<IActionResult> GetHotelRoomById(int hotelId, int roomId)
        {
            var room = await _dataContext.Rooms.FirstOrDefaultAsync(x => x.RoomId == roomId && x.HotelId == hotelId);
            if (room == null) return NotFound();
            var mappedRoom = _mapper.Map<RoomGetDto>(room);

            return Ok(room);
        }

        [HttpPost]
        [Route("{hotelId}/rooms")]
        public async Task<IActionResult> AddHotelRoom(int hotelId, [FromBody] RoomPostPutDto newRoom)
        {
            var roomDomain = _mapper.Map<Room>(newRoom);

            // 2 kinds of approach that will save in database
            //1st approach - this is nice if you work directly on the table
           // roomDomain.HotelId = hotelId;
           // _dataContext.rooms.Add(roomDomain);

            //2nd approach - less prone to errors and bugs
            var hotel = await _dataContext.Hotels.Include(r => r.Rooms).FirstOrDefaultAsync(h => h.HotelId == hotelId);
            hotel.Rooms.Add(roomDomain);
            //


            await _dataContext.SaveChangesAsync(); //this will save the database
            var mappedRoom = _mapper.Map<RoomGetDto>(roomDomain);
            
            return CreatedAtAction(nameof(GetHotelRoomById), new { hotelId = hotelId, roomId = mappedRoom.RoomId }, mappedRoom);
        }

        [HttpPut]
        [Route("{hotelId}/rooms/{roomId}")]
        public async Task<IActionResult> UpdateHotelRoom(int hotelId, int roomId, [FromBody] RoomPostPutDto updateRoom)
        {
            var toUpdate = _mapper.Map<Room>(updateRoom);
            toUpdate.HotelId = hotelId;
            toUpdate.RoomId = roomId;

             _dataContext.Update(toUpdate);

            await _dataContext.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete]
        [Route("{hotelId}/rooms/{roomId}")]
        public async Task<IActionResult> RemoveRoomFromHotel(int hotelId, int roomId)
        {
            //1st approach
            var room = await _dataContext.Rooms.FirstOrDefaultAsync(x => x.RoomId == roomId && x.HotelId == hotelId);
            if (room == null) return NotFound("Room not found");
            _dataContext.Rooms.Remove(room);

            //2nd approach
            //var hotel = await _dataContext.Hotels.Include(r => r.Rooms).FirstOrDefaultAsync(h => h.HotelId == hotelId);
            //hotel.Rooms.Remove(hotel);


            await _dataContext.SaveChangesAsync();


            return NoContent();
        }
    }
}
