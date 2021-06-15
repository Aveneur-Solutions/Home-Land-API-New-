using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Domain.DTOs;
using Domain.Errors;
using Domain.UnitBooking;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Customer
{
    public class CustomerDetails
    {
          public class Query : IRequest<CustomerDetailsDTO>
        {
            public string PhoneNumber { get; set; }
        }
        public class Handler : IRequestHandler<Query, CustomerDetailsDTO>
        {
            private readonly HomelandContext _context;
            private readonly IMapper _mapper;
            public Handler(HomelandContext context, IMapper mapper)
            {
                _mapper = mapper;
                _context = context;
            }

            public async Task<CustomerDetailsDTO> Handle(Query request, CancellationToken cancellationToken)
            {
                var phoneNumber = "+88"+request.PhoneNumber;
                var user = await _context.Users
                .Include(x => x.Bookings)
                .Include(x => x.Allotments)
                .FirstOrDefaultAsync(x => x.PhoneNumber == phoneNumber);

                if(user == null) throw new RestException(HttpStatusCode.NotFound,new {error = "No user found"});
                
                var transfers = await _context.TransferredFlats.Where(x => x.TransmitterId == user.Id).Include(x => x.Reciever).ToListAsync();       
                 CustomerDetailsDTO customerDetails = new CustomerDetailsDTO{
                    FullName = user.FirstName+" "+user.LastName,
                    PhoneNumber = user.PhoneNumber,
                    Address = user.Address,
                    NID = user.NID,
                    // JoiningDate = user.JoiningDate,
                    Transfers = _mapper.Map<List<Transfer>,List<TransferDTO>>(transfers),
                    Bookings = _mapper.Map<ICollection<Booking>,List<BookingDTO>>(user.Bookings),
                    Allotments = _mapper.Map<ICollection<AllotMent>,List<AllotmentDTO>>(user.Allotments)                    
                 };
                return customerDetails;                
            }
        }
    }
}