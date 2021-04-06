using AutoMapper;
using Domain.DTOs;
using Domain.UnitBooking;
using Domain.UserAuth;

namespace Application.Helper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
             CreateMap<Flat, FlatDTO>();
             CreateMap<AppUser,CustomerDTO>()
              .ForMember(x => x.Fullname,o => o.MapFrom(s => s.FirstName+" "+s.LastName));
             CreateMap<Booking,BookingDTO>()
             .ForMember(x => x.CustomerName, o => o.MapFrom(s => s.User.FirstName));
             CreateMap<TransferredFlat,TransferDTO>()
              .ForMember(x => x.TransferredFrom,o => o.MapFrom(s => s.Transmitter.FirstName))
              .ForMember(x => x.TransferredTo,o => o.MapFrom(s => s.Reciever.FirstName));
        }
    }
}