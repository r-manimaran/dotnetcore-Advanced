using System;
using System.Collections.Generic;
using System.Text;

namespace ShopHub.Contracts.Enums;

public enum OrderStatus
{
    Pending,
    Confirmed,
    Shipped,
    Delivered,
    Cancelled,
    Returned
}
