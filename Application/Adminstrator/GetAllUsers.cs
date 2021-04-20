using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Domain.DTOs;
using Domain.Errors;
using Domain.UserAuth;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Adminstrator
{
    public class GetAllUsers
    {
        public class Query : IRequest<List<UserDTO>>
        {
            public string PhoneNumber { get; set; }
        }
        public class Handler : IRequestHandler<Query, List<UserDTO>>
        {
            private readonly HomelandContext _context;
            private readonly IMapper _mapper;
            private readonly UserManager<AppUser> _userManager;

            public Handler(HomelandContext context, IMapper mapper, UserManager<AppUser> userManager)
            {
                _userManager = userManager;
                _mapper = mapper;
                _context = context;
            }

            public async Task<List<UserDTO>> Handle(Query request, CancellationToken cancellationToken)
            {
                var users = await _context.Users.ToListAsync();

                if (users == null) throw new RestException(HttpStatusCode.NotFound, new { error = "No Users Found" });

                var mappedUsers = _mapper.Map<List<AppUser>, List<UserDTO>>(users);
                var i = 0;
                while (i < mappedUsers.Count)
                {
                    mappedUsers[i].Role = await _userManager.IsInRoleAsync(users[i],"User")
                     ? "User" : await _userManager.IsInRoleAsync(users[i],"Admin") 
                     ? "Admin" : "Super Admin";
                     i++;
                 }
                return mappedUsers;
            }
        }
    }
}