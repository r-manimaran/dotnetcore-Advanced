using WebApi.Models.Orders;

namespace WebApi.Dtos;

public class SubmitOrderRequest
{
    public List<Guid> ProductIds { get; set; }
}
