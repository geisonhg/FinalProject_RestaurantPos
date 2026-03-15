using MediatR;
using RestaurantDashboard.Application.Common.Exceptions;
using RestaurantDashboard.Application.Common.Interfaces;
using RestaurantDashboard.Application.Sales.Dtos;
using RestaurantDashboard.Domain.Entities;
using RestaurantDashboard.Domain.Repositories;

namespace RestaurantDashboard.Application.Sales.Commands.GenerateOrderReceipt;

public sealed class GenerateOrderReceiptCommandHandler : IRequestHandler<GenerateOrderReceiptCommand, string>
{
    private readonly IOrderRepository _orders;
    private readonly ISaleRepository _sales;
    private readonly IEmployeeRepository _employees;
    private readonly IReportService _reportService;

    public GenerateOrderReceiptCommandHandler(
        IOrderRepository orders,
        ISaleRepository sales,
        IEmployeeRepository employees,
        IReportService reportService)
    {
        _orders = orders;
        _sales = sales;
        _employees = employees;
        _reportService = reportService;
    }

    public async Task<string> Handle(GenerateOrderReceiptCommand request, CancellationToken cancellationToken)
    {
        var order = await _orders.GetByIdWithItemsAsync(request.OrderId, cancellationToken)
            ?? throw new NotFoundException(nameof(Order), request.OrderId);

        var sale = await _sales.GetByOrderIdAsync(order.Id, cancellationToken);
        var employee = await _employees.GetByIdAsync(order.EmployeeId, cancellationToken);

        var dto = new OrderDto
        {
            Id = order.Id,
            TableNumber = order.TableNumber,
            EmployeeName = employee?.FullName ?? "Unknown",
            Status = order.Status.ToString(),
            OpenedAt = order.OpenedAt,
            ClosedAt = order.ClosedAt,
            Subtotal = order.Subtotal.Amount,
            DiscountAmount = order.DiscountAmount,
            TipAmount = sale?.TipAmount.Amount ?? 0m,
            PaymentMethod = sale?.PaymentMethod.ToString(),
            Notes = order.Notes,
            Items = order.Items
                .Select(i => new OrderItemDto
                {
                    Id = i.Id,
                    MenuItemId = i.MenuItemId,
                    MenuItemName = i.MenuItemName,
                    Quantity = i.Quantity,
                    UnitPrice = i.UnitPrice.Amount,
                    LineTotal = i.LineTotal.Amount
                })
                .ToList()
        };

        return await _reportService.GenerateOrderReceiptAsync(dto, cancellationToken);
    }
}
