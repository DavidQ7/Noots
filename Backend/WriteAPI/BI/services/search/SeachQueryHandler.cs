﻿using AutoMapper;
using Common.DTO.search;
using Domain.Queries.search;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WriteContext.Repositories;

namespace BI.services.search
{
    public class SeachQueryHandler 
        : IRequestHandler<GetUsersForSharingModalQuery, List<ShortUserForShareModal>>
    {
        private readonly UserRepository userRepository;
        private readonly IMapper mapper;
        public SeachQueryHandler(UserRepository userRepository, IMapper mapper)
        {
            this.userRepository = userRepository;
            this.mapper = mapper;
        }

        public async Task<List<ShortUserForShareModal>> Handle(GetUsersForSharingModalQuery request, CancellationToken cancellationToken)
        {
            var users = await userRepository.SearchByEmailAndName(request.SearchString);
            return mapper.Map<List<ShortUserForShareModal>>(users);
        }
    }
}