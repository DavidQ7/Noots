﻿using Common.Attributes;
using MediatR;
using System;
using System.ComponentModel.DataAnnotations;


namespace Domain.Commands.users
{
    public class UpdateThemeCommand : BaseCommandEntity, IRequest<Unit>
    {
        [ValidationGuidAttribute]
        public Guid Id { set; get; }

        public UpdateThemeCommand(Guid Id, string Email)
            : base(Email)
        {
            this.Id = Id;
        }
    }
}
