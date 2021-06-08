// using Application.Interfaces;
// using AutoMapper;
// using MediatR;
// using Microsoft.EntityFrameworkCore;
// using Persistence;

// namespace Application.Customer
// {
//     public class UnpaidOrder
//     {
//         public class Query : IRequest<TransferDTO> { }
//         public class Handler : IRequestHandler<Query, TransferDTO>
//         {
//             private readonly HomelandContext _context;
//             private readonly IMapper _mapper;
//             private readonly IUserAccessor _userAccessor;

//             public Handler(HomelandContext context, IMapper mapper, IUserAccessor userAccessor)
//             {
//                 _userAccessor = userAccessor;
//                 _mapper = mapper;
//                 _context = context;
//             }

//             public async Task<List<TransferDTO>> Handle(Query request, CancellationToken cancellationToken)
//             {
//                 var role = _userAccessor.GetUserRole();
//                 if (role == "User")
//                 {
//                     var user = await _context.Users.FirstOrDefaultAsync(x => x.PhoneNumber == _userAccessor.GetUserPhoneNo());

//                     var transfers = await _context.TransferredFlats
//                     .Include(x => x.Flat)
//                     .Include(x => x.Transmitter)
//                     .Include(x => x.Reciever)
//                     .Where(x => x.Transmitter.PhoneNumber == user.PhoneNumber)
//                     .ToListAsync();


//                     //  if (allotments.Capacity == 0) throw new RestException(HttpStatusCode.NoContent, new { error = "No Allotments available right now" });

//                     var mappedBookings = _mapper.Map<List<Transfer>, List<TransferDTO>>(transfers);

//                     return mappedBookings;
//                 }
//                 else throw new RestException(HttpStatusCode.Forbidden,new {error ="Power er misuse kora uchit na . ei Api shudhu Customer er"});
//             }
//         }
//     }
// }