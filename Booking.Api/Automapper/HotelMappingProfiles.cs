using AutoMapper;
using Booking.Api.DTOs;
using Booking.Domain.Models;

namespace Booking.Api.Automapper
{
    public class HotelMappingProfiles : Profile
    {
       public HotelMappingProfiles()
        {
            CreateMap<HotelCreateDto, Hotel>();
            CreateMap<Hotel, HotelGetDto>();
        }
    }
}
