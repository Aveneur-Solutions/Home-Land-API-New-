using System.Net;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Domain.DTOs;
using Domain.Errors;
using Domain.UserAuth;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.UserAuth
{
    public class GetUser
    {
        public class Query : IRequest<CustomerDTO>
        {
            public string PhoneNumber { get; set; }
        }
        public class Handler : IRequestHandler<Query, CustomerDTO>
        {
            private readonly HomelandContext _context;
            private readonly IMapper _mapper;

            public Handler(HomelandContext context, IMapper mapper)
            {
                _mapper = mapper;
                _context = context;
            }

            public async Task<CustomerDTO> Handle(Query request, CancellationToken cancellationToken)
            {
                var user = await _context.Users.FirstOrDefaultAsync(x => x.PhoneNumber == request.PhoneNumber);

                if (user == null) throw new RestException(HttpStatusCode.NotFound, new { error = "User Doesn't exist" });


                return _mapper.Map<AppUser, CustomerDTO>(user);
            }
        }
    }
}