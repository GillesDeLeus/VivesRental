﻿using System;
using System.Collections.Generic;
using VivesRental.Model;

namespace VivesRental.Services.Contracts
{
    public interface IOrderLineService
    {
        OrderLine Get(Guid id);
        bool Rent(Guid orderId, Guid articleId);
        bool Return(Guid orderLineId, DateTime returnedAt);
	    IList<OrderLine> FindByOrderId(Guid orderId);

    }
}
