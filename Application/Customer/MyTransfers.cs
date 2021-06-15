using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Application.Interfaces;
using AutoMapper;
using Domain.DTOs;
using Domain.Errors;
using Domain.UnitBooking;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Customer
{
    public class MyTransfers
    {
        public class Query : IRequest<List<TransferDTO>> { }
        public class Handler : IRequestHandler<Query, List<TransferDTO>>
        {
            private readonly HomelandContext _context;
            private readonly IMapper _mapper;
            private readonly IUserAccessor _userAccessor;

            public Handler(HomelandContext context, IMapper mapper, IUserAccessor userAccessor)
            {
                _userAccessor = userAccessor;
                _mapper = mapper;
                _context = context;
            }

            public async Task<List<TransferDTO>> Handle(Query request, CancellationToken cancellationToken)
            {
                var role = _userAccessor.GetUserRole();
                if (role == "User")
                {
                    var user = await _context.Users.FirstOrDefaultAsync(x => x.PhoneNumber == _userAccessor.GetUserPhoneNo());

                    var transfers = await _context.TransferredFlats
                    .Include(x => x.Flat)
                    .Include(x => x.Transmitter)
                    .Include(x => x.Reciever)
                    .Where(x => x.Transmitter.PhoneNumber == user.PhoneNumber)
                    .ToListAsync();


                    //  if (allotments.Capacity == 0) throw new RestException(HttpStatusCode.NoContent, new { error = "No Allotments available right now" });

                    var mappedBookings = _mapper.Map<List<Transfer>, List<TransferDTO>>(transfers);

                    return mappedBookings;
                }
                else throw new RestException(HttpStatusCode.Forbidden,new {error ="You have no transfers"});
            }
        }
    }
}