﻿using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Common.DTO.Personalization;
using Domain.Commands.Personalizations;
using Domain.Queries.Personalization;
using WriteAPI.ControllerConfig;
using Microsoft.AspNetCore.Authorization;

namespace WriteAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class PersonalizationController : ControllerBase
    {
        private readonly IMediator _mediator;

        public PersonalizationController(IMediator _mediator)
        {
            this._mediator = _mediator;
        }


        [HttpGet]
        public async Task<PersonalizationSettingDTO> GetUserPersonalizationSettings()
        {
            var query = new GetUserPersonalizationSettingsQuery(this.GetUserEmail());
            return await _mediator.Send(query);
        }


        [HttpPatch]
        public async Task UpdateUserPersonalizationSettings(UpdatePersonalizationSettingsCommand command)
        {
            command.Email = this.GetUserEmail();
            await _mediator.Send(command);
        }
    }
}
