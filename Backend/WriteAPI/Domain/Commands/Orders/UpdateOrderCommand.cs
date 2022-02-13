﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Common.Attributes;
using Common.DTO.Orders;
using MediatR;

namespace Domain.Commands.Orders
{
    public class UpdateOrderCommand : BaseCommandEntity, IRequest<List<UpdateOrderEntityResponse>>
    {
        [RequiredEnumField(ErrorMessage = "Order type is required.")]
        public OrderEntity OrderEntity { set; get; }

        [Required]
        public int Position { set; get; }

        [ValidationGuid]
        public Guid EntityId { set; get; }
    }
}