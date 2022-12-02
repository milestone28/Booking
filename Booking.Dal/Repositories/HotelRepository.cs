using Booking.Domain.Abstractions.Repositories;
using Booking.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Booking.Dal.Repositories
{
    public class HotelRepository : IHotelsRepository
    {
        private readonly DataContext _dataContext;

        public HotelRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task<Hotel> CreateHotelAsync(Hotel hotel)
        {
            _dataContext.Hotels.Add(hotel); //this will just add to memory
            await _dataContext.SaveChangesAsync(); //this will save the database
            return hotel;
        }

        public async Task<Room> CreateHotelRoomAsync(int hotelId, Room room)
        {
            var hotel = await _dataContext.Hotels.Include(r => r.Rooms).FirstOrDefaultAsync(h => h.HotelId == hotelId);
            hotel.Rooms.Add(room);

            await _dataContext.SaveChangesAsync(); //this will save the database

            return room;
        }

        public async Task<Hotel> DeleteHotelAsync(int id)
        {
            var hotel = await _dataContext.Hotels.FirstOrDefaultAsync(x => x.HotelId == id);
            if (hotel == null) return null;
            _dataContext.Remove(hotel);  //updated on memory but not save in database
            await _dataContext.SaveChangesAsync(); //this will save to database always use async
            return hotel;
        }

        public async Task<Room> DeleteHotelRoomAsync(int hotelId, int roomId)
        {
            var room = await _dataContext.Rooms.FirstOrDefaultAsync(x => x.RoomId == roomId && x.HotelId == hotelId);
            if (room == null) return null;
            _dataContext.Rooms.Remove(room);


            await _dataContext.SaveChangesAsync();
            return room;
        }

        public async Task<List<Hotel>> GetAllHotelsAsync()
        {
           return await _dataContext.Hotels.ToListAsync();
        }

        public async Task<Room> GetHotelRoomByIdAsync(int hotelId, int roomId)
        {
            var room = await _dataContext.Rooms.FirstOrDefaultAsync(x => x.RoomId == roomId && x.HotelId == hotelId);
            if (room == null) return null;
            
            return room;
        }

        public async Task<Hotel> GetHotelsByIdAsync(int id)
        {
            var hotel = await _dataContext.Hotels.FirstOrDefaultAsync(x => x.HotelId == id);
            if (hotel == null) return null;
            return hotel;
        }

        public async Task<List<Room>> ListHotelRoomAsync(int hotelId)
        {
            return await _dataContext.Rooms.Where(x => x.HotelId == hotelId).ToListAsync();
        }

        public async Task<Hotel> UpdateHotelAsync(Hotel updatedHotel)
        {
            _dataContext.Update(updatedHotel);
            await _dataContext.SaveChangesAsync();
           return updatedHotel;
        }

        public async Task<Room> UpdateHotelRoomAsync(int hotelId, Room updatedRoom)
        {
            _dataContext.Update(updatedRoom);
            await _dataContext.SaveChangesAsync();
            return updatedRoom;

        }
    }
}
