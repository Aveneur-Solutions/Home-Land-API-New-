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
        }
    }
}