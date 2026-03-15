using MediatR;
using RestaurantDashboard.Application.Common.Exceptions;
using RestaurantDashboard.Domain.Entities;
using RestaurantDashboard.Domain.Repositories;

namespace RestaurantDashboard.Application.Sales.Commands.RemoveOrderItem;

public sealed class RemoveOrderItemCommandHandler : IRequestHandler<RemoveOrderItemCommand, Unit>
{
    private readonly IOrderRepository _orders;
    private readonly IUnitOfWork _uow;

    public RemoveOrderItemCommandHandler(IOrderRepository orders, IUnitOfWork uow)
    {
        _orders = orders;
        _uow = uow;
    }

    public async Task<Unit> Handle(RemoveOrderItemCommand request, CancellationToken cancellationToken)
    {
        var order = await _orders.GetByIdWithItemsAsync(request.OrderId, cancellationToken)
            ?? throw new NotFoundException(nameof(Order), request.OrderId);

        order.RemoveItem(request.OrderItemId);

        _orders.Update(order);
        await _uow.CommitAsync(cancellationToken);
        return Unit.Value;
    }
}
