using AutoMapper;
using Booking.Api.DTOs;
using Booking.Domain.Models;

namespace Booking.Api.Automapper
{
    public class RoomMappingProfiles : Profile
    {
        public RoomMappingProfiles() 
        {
            CreateMap<Room, RoomGetDto>();
            CreateMap<RoomPostPutDto, Room>();
        }
    }
}
