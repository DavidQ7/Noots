﻿using System.Threading.Tasks;
using Common.DTO;
using Common.DTO.Users;
using Domain.Commands.Users;
using Domain.Queries.Files;
using Domain.Queries.Users;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WriteAPI.ConstraintsUploadFiles;
using WriteAPI.ControllerConfig;

namespace WriteAPI.Controllers.UserContollers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IMediator _mediator;
        public UserController(IMediator _mediator)
        {
            this._mediator = _mediator;
        }


        [HttpPost]
        public async Task<ShortUser> Authorize(NewUserCommand command)
        {
            var currentUserEmail = this.GetUserEmail();
            command.Email = currentUserEmail;
            await _mediator.Send(command);
            return await _mediator.Send(new GetShortUserQuery(currentUserEmail));
        }


        [HttpGet("short")]
        public async Task<ShortUser> GetShort()
        {
            var currentUserEmail = this.GetUserEmail();
            return await _mediator.Send(new GetShortUserQuery(currentUserEmail));
        }

        [HttpGet("memory")]
        public async Task<GetUserMemoryResponse> GetUsedDiskSpace()
        {
            var currentUserEmail = this.GetUserEmail();
            return await _mediator.Send(new GetUserMemoryQuery(currentUserEmail));
        }

        [HttpPut("username")]
        public async Task UpdateMainInformation([FromBody] UpdateMainUserInfoCommand info)
        {
            var currentUserEmail = this.GetUserEmail();
            info.Email = currentUserEmail;
            await _mediator.Send(info);
        }

        [HttpPost("photo")]
        public async Task<OperationResult<AnswerChangeUserPhoto>> ChangeProfilePhoto(IFormFile photo)
        {
            var validatioResult = this.ValidateFile<AnswerChangeUserPhoto>(photo, SupportFileContentTypes.Photos, FileSizeConstraints.MaxProfilePhotoSize);
            if (!validatioResult.Success)
            {
                return validatioResult;
            }

            var email = this.GetUserEmail();
            return await _mediator.Send(new UpdatePhotoCommand(photo, email));
        }

        [HttpPost("language")]
        public async Task ChangeLanguage(UpdateLanguageCommand languageCommand)
        {
            var email = this.GetUserEmail();
            languageCommand.Email = email;
            await _mediator.Send(languageCommand);
        }

        [HttpPost("theme")]
        public async Task ChangeTheme(UpdateThemeCommand themeCommand)
        {
            var email = this.GetUserEmail();
            themeCommand.Email = email;
            await _mediator.Send(themeCommand);
        }

        [HttpPost("font")]
        public async Task ChangeFontSize(UpdateFontSizeCommand fontSizeCommand)
        {
            var email = this.GetUserEmail();
            fontSizeCommand.Email = email;
            await _mediator.Send(fontSizeCommand);
        }
    }
}