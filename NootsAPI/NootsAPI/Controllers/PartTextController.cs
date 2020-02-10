﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Noots.BusinessLogic.Services;
using Shared.DTO.PartText;
using Shared.DTO.PartUnknown;

namespace NootsAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PartTextController : ControllerBase
    {
        private readonly PartTextService partTextService;
        public PartTextController(PartTextService partTextService)
        {
            this.partTextService = partTextService;
        }

        [HttpPost("new")]
        public async Task New(DTONewPartText partText)
        {
            await partTextService.New(partText);
        }
        
        [HttpPost("unknown")]
        public async Task New([FromBody] PartNewUnknown partUnknown)
        {
            await partTextService.NewUnknown(partUnknown);
        }
    }
}