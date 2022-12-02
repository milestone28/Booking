
using AutoMapper;
using Booking.Api.DTOs;
using Booking.Domain.Abstractions.Repositories;
using Booking.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace Booking.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HotelController : Controller
    {
        private readonly IHotelsRepository _hotelsRepository;
        private readonly IMapper _mapper;
        public HotelController(IHotelsRepository hotelsRepository, IMapper mapper)
        {
            _hotelsRepository = hotelsRepository; 
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllHotels() // when ever we do network that make processor slow we will use async async Task<>
        {
            var hotels = await _hotelsRepository.GetAllHotelsAsync();
            // this will remove because we implemented the repository method
            //var hotels = await _dataContext.Hotels.ToListAsync(); // we use await and Async

            var hotelsGet = _mapper.Map<List<HotelGetDto>>(hotels); // should be map to List

            return Ok(hotelsGet);
        }

        [Route("{id}")]
        [HttpGet]

        public async Task<IActionResult> GetHotelById(int id)
        { // this will remove because we implemented the repository method
          // var hotel = await _dataContext.Hotels.FirstOrDefaultAsync(x => x.HotelId == id);
            var hotel = await _hotelsRepository.GetHotelsByIdAsync(id);
            if (hotel == null) return NotFound();

            var res = _mapper.Map<HotelGetDto>(hotel); 
            return Ok(res);
        }

        [HttpPost]
        public async Task<IActionResult> CreateHotel([FromBody] HotelCreateDto hotel) // we user HotelCreateDto for the client input instead using Hotel from the domain
        {
            var domainHotel = _mapper.Map<Hotel>(hotel);

            // this will remove because we implemented the repository method
            //_dataContext.Hotels.Add(domainHotel); //this will just add to memory
            //await _dataContext.SaveChangesAsync(); //this will save the database
            await _hotelsRepository.CreateHotelAsync(domainHotel); //this will save the database
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
            await _hotelsRepository.UpdateHotelAsync(toUpdate); 

            // var Update = await _dataContext.hotels.FirstOrDefaultAsync(h => h.HotelId == toUpdate.HotelId);

            //remove because applied mapper
            //hotel.Stars = updated.Stars;
            //hotel.Description = updated.Description;
            //hotel.Name = updated.Name;
            // this will remove because we implemented the repository method
            // _dataContext.Update(toUpdate); //updated on memory but not save in database
            //await _dataContext.SaveChangesAsync(); //this will save to database always use async


            return NoContent();
        }

        [Route("{id}")]
        [HttpDelete]
        public async Task<IActionResult> DeleteHotel(int id)
        {
            // this will remove because we implemented the repository method
            //var hotel = await _dataContext.Hotels.FirstOrDefaultAsync(x => x.HotelId == id);
            //if (hotel == null) return NotFound();
            //_dataContext.Remove(hotel);  //updated on memory but not save in database
            //await _dataContext.SaveChangesAsync(); //this will save to database always use async
            await _hotelsRepository.DeleteHotelAsync(id);
            return NoContent();
        }

        [HttpGet]
        [Route("{hotelId}/rooms")]

        public async Task<IActionResult> GetAllHotelRooms(int hotelId)
        {
            // this will remove because we implemented the repository method
            // var rooms = await _dataContext.Rooms.Where(x => x.HotelId == hotelId).ToListAsync();
            var rooms = await _hotelsRepository.ListHotelRoomAsync(hotelId);
            var mappedRooms = _mapper.Map<List<RoomGetDto>>(rooms);

            return Ok(mappedRooms);
        }

        [HttpGet]
        [Route("{hotelId}/rooms/{roomId}")]
        public async Task<IActionResult> GetHotelRoomById(int hotelId, int roomId)
        {
            // this will remove because we implemented the repository method
            //var room = await _dataContext.Rooms.FirstOrDefaultAsync(x => x.RoomId == roomId && x.HotelId == hotelId);
            //if (room == null) return NotFound();
            var room = await _hotelsRepository.GetHotelRoomByIdAsync(hotelId, roomId);
            var mappedRoom = _mapper.Map<RoomGetDto>(room);

            return Ok(mappedRoom);
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
            //// this will remove because we implemented the repository method
            //var hotel = await _dataContext.Hotels.Include(r => r.Rooms).FirstOrDefaultAsync(h => h.HotelId == hotelId);
            //hotel.Rooms.Add(roomDomain);
            ////


            //await _dataContext.SaveChangesAsync(); //this will save the database

            await _hotelsRepository.CreateHotelRoomAsync(hotelId,roomDomain);

            var mappedRoom = _mapper.Map<RoomGetDto>(roomDomain);
            
            return CreatedAtAction(nameof(GetHotelRoomById), new { hotelId = hotelId, roomId = mappedRoom.RoomId }, mappedRoom);
        }

        [HttpPut]
        [Route("{hotelId}/rooms/{roomId}")]
        public async Task<IActionResult> UpdateHotelRoom(int hotelId, int roomId, [FromBody] RoomPostPutDto updateRoom)
        {
            // this will remove because we implemented the repository method
            var toUpdate = _mapper.Map<Room>(updateRoom);
            //toUpdate.HotelId = hotelId;
            //toUpdate.RoomId = roomId;

            // _dataContext.Update(toUpdate);

            //await _dataContext.SaveChangesAsync();
            await _hotelsRepository.UpdateHotelRoomAsync(hotelId, toUpdate);
            return NoContent();
        }

        [HttpDelete]
        [Route("{hotelId}/rooms/{roomId}")]
        public async Task<IActionResult> RemoveRoomFromHotel(int hotelId, int roomId)
        {
            //1st approach
            // this will remove because we implemented the repository method
            //var room = await _dataContext.Rooms.FirstOrDefaultAsync(x => x.RoomId == roomId && x.HotelId == hotelId);
            //if (room == null) return NotFound("Room not found");
            //_dataContext.Rooms.Remove(room);

            //2nd approach
            //var hotel = await _dataContext.Hotels.Include(r => r.Rooms).FirstOrDefaultAsync(h => h.HotelId == hotelId);
            //hotel.Rooms.Remove(hotel);


            //await _dataContext.SaveChangesAsync();

            await _hotelsRepository.DeleteHotelRoomAsync(hotelId, roomId);
            return NoContent();
        }
    }
}
