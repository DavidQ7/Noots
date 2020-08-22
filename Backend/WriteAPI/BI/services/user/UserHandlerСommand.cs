﻿using AutoMapper;
using BI.helpers;
using Common;
using Common.DatabaseModels.helpers;
using Common.DatabaseModels.models;
using Domain.Commands.users;
using MediatR;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WriteContext.Repositories;

namespace BI.services.user
{
    public class UserHandlerСommand :
        IRequestHandler<NewUserCommand, Unit>,
        IRequestHandler<UpdateMainUserInfoCommand, Unit>,
        IRequestHandler<UpdatePhotoCommand, Unit>,
        IRequestHandler<UpdateLanguageCommand, Unit>
    {
        private readonly UserRepository userRepository;
        private readonly IMapper imapper;
        private readonly PhotoHelpers photoHelpers;
        public UserHandlerСommand(UserRepository userRepository, IMapper imapper, PhotoHelpers photoHelpers)
        {
            this.userRepository = userRepository;
            this.imapper = imapper;
            this.photoHelpers = photoHelpers;
        }

        public async Task<Unit> Handle(NewUserCommand request, CancellationToken cancellationToken)
        {
            var user = imapper.Map<User>(request);
            user.PersonalitionSettings = new PersonalitionSetting()
            {
                Theme = Theme.Dark,
                FontSize = FontSize.Medium,
            };
            await userRepository.Add(user);
            return Unit.Value;
        }

        public async Task<Unit> Handle(UpdateMainUserInfoCommand request, CancellationToken cancellationToken)
        {
            var user = await userRepository.GetUserByEmail(request.Email);
            user.Name = request.Name;
            await userRepository.Update(user);
            return Unit.Value;
        }

        public async Task<Unit> Handle(UpdatePhotoCommand request, CancellationToken cancellationToken)
        {
            var user = await userRepository.GetUserByEmail(request.Email);
            user.PhotoId = await photoHelpers.GetBase64(request.File);
            await userRepository.Update(user);
            return Unit.Value;
        }

        public async Task<Unit> Handle(UpdateLanguageCommand request, CancellationToken cancellationToken)
        {
            var user = await userRepository.GetUserByEmail(request.Email);
            user.Language = request.Language;
            await userRepository.Update(user);
            return Unit.Value;
        }
    }
}
