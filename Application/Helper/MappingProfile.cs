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
             CreateMap<Flat, FlatDTO>()
             .ForMember(x => x.Images, o => o.MapFrom(s => s.Images))
             .ForMember(x => x.BuildingNumber,o => o.MapFrom(s => s.Building.BuildingNumber));
             CreateMap<FlatImage,ImageDTO>();
             CreateMap<AppUser,UserDTO>();
             CreateMap<AllotMent,AllotmentDTO>()
             .ForMember( x => x.CustomerName,o => o.MapFrom(s => s.User.FirstName+" "+s.User.LastName));
             CreateMap<AppUser,CustomerDTO>()
              .ForMember(x => x.Fullname,o => o.MapFrom(s => s.FirstName+" "+s.LastName));
             CreateMap<Booking,BookingDTO>()
             .ForMember(x => x.CustomerName, o => o.MapFrom(s => s.User.FirstName));
             CreateMap<Transfer,TransferDTO>()
              .ForMember(x => x.TransferredFrom,o => o.MapFrom(s => s.Transmitter.FirstName))
              .ForMember(x => x.TransferredTo,o => o.MapFrom(s => s.Reciever.FirstName));
             CreateMap<Building,BuildingDTO>()
             .ForMember(x =>x.Flats,o=> o.MapFrom(s => s.Flats));
            
        }
    }
}