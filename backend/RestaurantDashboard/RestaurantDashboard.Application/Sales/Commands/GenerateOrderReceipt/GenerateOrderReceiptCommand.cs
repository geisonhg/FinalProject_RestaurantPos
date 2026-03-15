using MediatR;

namespace RestaurantDashboard.Application.Sales.Commands.GenerateOrderReceipt;

public sealed record GenerateOrderReceiptCommand : IRequest<string>
{
    public Guid OrderId { get; init; }
}
