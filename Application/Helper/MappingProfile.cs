using AutoMapper;
using Domain.DTOs;
using Domain.UnitBooking;

namespace Application.Helper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
             CreateMap<Flat, FlatDTO>();
             CreateMap<Booking,BookingDTO>()
             .ForMember(x => x.CustomerName, o => o.MapFrom(s => s.User.FirstName))
             .ForMember(x => x.FlatId,o => o.MapFrom(s => s.FlatId));
        }
    }
}