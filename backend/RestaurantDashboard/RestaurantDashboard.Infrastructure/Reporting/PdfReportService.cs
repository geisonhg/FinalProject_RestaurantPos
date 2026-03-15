using Microsoft.AspNetCore.Hosting;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using RestaurantDashboard.Application.Common.Interfaces;
using RestaurantDashboard.Application.Employees.Dtos;
using RestaurantDashboard.Application.Sales.Dtos;

namespace RestaurantDashboard.Infrastructure.Reporting;

public sealed class PdfReportService : IReportService
{
    private readonly string _reportsDir;

    public PdfReportService(IWebHostEnvironment env)
    {
        _reportsDir = Path.Combine(env.WebRootPath, "reports");
        Directory.CreateDirectory(_reportsDir);
    }

    public Task<string> GenerateWeeklyReportAsync(
        SalesSummaryDto summary,
        IEnumerable<EmployeePerformanceDto> staffPerformance,
        CancellationToken ct = default)
        => GenerateAsync("weekly", summary, staffPerformance);

    public Task<string> GenerateMonthlyReportAsync(
        SalesSummaryDto summary,
        IEnumerable<EmployeePerformanceDto> staffPerformance,
        CancellationToken ct = default)
        => GenerateAsync("monthly", summary, staffPerformance);

    private Task<string> GenerateAsync(
        string type,
        SalesSummaryDto summary,
        IEnumerable<EmployeePerformanceDto> staffPerformance)
    {
        var fileName = $"report_{type}_{summary.PeriodStart:yyyyMMdd}_{summary.PeriodEnd:yyyyMMdd}_{Guid.NewGuid():N}.pdf";
        var fullPath = Path.Combine(_reportsDir, fileName);
        var relativePath = $"/reports/{fileName}";

        var staff = staffPerformance.ToList();

        Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Size(PageSizes.A4);
                page.Margin(40);
                page.DefaultTextStyle(x => x.FontSize(10).FontFamily("Arial"));

                page.Header().Element(ComposeHeader(summary, type));
                page.Content().Element(ComposeContent(summary, staff));
                page.Footer().Element(ComposeFooter());
            });
        }).GeneratePdf(fullPath);

        return Task.FromResult(relativePath);
    }

    // ── Header ──────────────────────────────────────────────────────────────

    private static Action<IContainer> ComposeHeader(SalesSummaryDto summary, string type) =>
        header => header
            .PaddingBottom(16)
            .Row(row =>
            {
                row.RelativeItem().Column(col =>
                {
                    col.Item().Text("🍽 Restaurant POS")
                        .FontSize(20).Bold().FontColor(Colors.Grey.Darken3);
                    col.Item().Text($"{char.ToUpper(type[0])}{type[1..]} Performance Report")
                        .FontSize(12).FontColor(Colors.Grey.Medium);
                });
                row.ConstantItem(160).AlignRight().Column(col =>
                {
                    col.Item().Text($"{summary.PeriodStart:dd MMM yyyy} — {summary.PeriodEnd:dd MMM yyyy}")
                        .FontSize(10).FontColor(Colors.Grey.Medium);
                    col.Item().Text($"Generated: {DateTime.Now:dd MMM yyyy HH:mm}")
                        .FontSize(9).FontColor(Colors.Grey.Lighten1);
                });
            });

    // ── Content ─────────────────────────────────────────────────────────────

    private static Action<IContainer> ComposeContent(SalesSummaryDto summary, List<EmployeePerformanceDto> staff) =>
        content => content.Column(col =>
        {
            col.Spacing(16);

            // Financial summary
            col.Item().Element(FinancialSummarySection(summary));

            // Revenue by payment method
            if (summary.RevenueByPaymentMethod.Any())
                col.Item().Element(PaymentMethodSection(summary));

            // Top selling items
            if (summary.TopSellingItems.Any())
                col.Item().Element(TopItemsSection(summary));

            // Expenses by category
            if (summary.ExpensesByCategory.Any())
                col.Item().Element(ExpensesSection(summary));

            // Staff performance
            if (staff.Any())
                col.Item().Element(StaffSection(staff));
        });

    // ── Financial Summary Section ────────────────────────────────────────────

    private static Action<IContainer> FinancialSummarySection(SalesSummaryDto s) =>
        c => c.Column(col =>
        {
            col.Item().Text("Financial Summary").FontSize(13).Bold().FontColor(Colors.Grey.Darken2);
            col.Item().PaddingTop(4).Grid(grid =>
            {
                grid.Columns(3);
                grid.Spacing(8);

                KpiCell(grid, "Total Revenue", s.TotalRevenue.ToString("C"), Colors.Green.Darken2);
                KpiCell(grid, "Net Profit", s.NetProfit.ToString("C"),
                    s.NetProfit >= 0 ? Colors.Blue.Darken2 : Colors.Red.Darken2);
                KpiCell(grid, "Total Expenses", s.TotalExpenses.ToString("C"), Colors.Red.Darken1);
                KpiCell(grid, "Total Tips", s.TotalTips.ToString("C"), Colors.Orange.Darken2);
                KpiCell(grid, "Transactions", s.TotalTransactions.ToString(), Colors.Purple.Darken2);
                KpiCell(grid, "Avg. Order Value", s.AverageOrderValue.ToString("C"), Colors.Cyan.Darken2);
            });
        });

    private static void KpiCell(GridDescriptor grid, string label, string value, string color) =>
        grid.Item().Border(1).BorderColor(Colors.Grey.Lighten2).Padding(10).Column(col =>
        {
            col.Item().Text(label).FontSize(8).FontColor(Colors.Grey.Medium).Bold();
            col.Item().PaddingTop(4).Text(value).FontSize(16).Bold().FontColor(color);
        });

    // ── Payment Method Section ───────────────────────────────────────────────

    private static Action<IContainer> PaymentMethodSection(SalesSummaryDto s) =>
        c => c.Column(col =>
        {
            col.Item().Text("Revenue by Payment Method").FontSize(13).Bold().FontColor(Colors.Grey.Darken2);
            col.Item().PaddingTop(4).Table(table =>
            {
                table.ColumnsDefinition(cols =>
                {
                    cols.RelativeColumn(3);
                    cols.RelativeColumn(2);
                    cols.RelativeColumn(2);
                });

                table.Header(header =>
                {
                    foreach (var h in new[] { "Payment Method", "Revenue", "Transactions" })
                        header.Cell().Background(Colors.Grey.Lighten3).Padding(6)
                            .Text(h).Bold().FontSize(9);
                });

                foreach (var (method, revenue) in s.RevenueByPaymentMethod.OrderByDescending(x => x.Value))
                {
                    table.Cell().BorderBottom(1).BorderColor(Colors.Grey.Lighten3).Padding(6).Text(method);
                    table.Cell().BorderBottom(1).BorderColor(Colors.Grey.Lighten3).Padding(6)
                        .Text(revenue.ToString("C")).FontColor(Colors.Green.Darken2).Bold();
                    table.Cell().BorderBottom(1).BorderColor(Colors.Grey.Lighten3).Padding(6)
                        .Text(s.TransactionsByPaymentMethod.GetValueOrDefault(method, 0).ToString());
                }
            });
        });

    // ── Top Items Section ────────────────────────────────────────────────────

    private static Action<IContainer> TopItemsSection(SalesSummaryDto s) =>
        c => c.Column(col =>
        {
            col.Item().Text("Top Selling Items").FontSize(13).Bold().FontColor(Colors.Grey.Darken2);
            col.Item().PaddingTop(4).Table(table =>
            {
                table.ColumnsDefinition(cols =>
                {
                    cols.ConstantColumn(30);
                    cols.RelativeColumn();
                    cols.RelativeColumn(2);
                });

                table.Header(header =>
                {
                    foreach (var h in new[] { "#", "Item", "Sales" })
                        header.Cell().Background(Colors.Grey.Lighten3).Padding(6)
                            .Text(h).Bold().FontSize(9);
                });

                int rank = 1;
                foreach (var (item, sales) in s.TopSellingItems.OrderByDescending(x => x.Value).Take(10))
                {
                    table.Cell().BorderBottom(1).BorderColor(Colors.Grey.Lighten3).Padding(6)
                        .Text(rank.ToString()).FontColor(Colors.Grey.Medium);
                    table.Cell().BorderBottom(1).BorderColor(Colors.Grey.Lighten3).Padding(6).Text(item);
                    table.Cell().BorderBottom(1).BorderColor(Colors.Grey.Lighten3).Padding(6)
                        .Text(sales.ToString("C")).FontColor(Colors.Green.Darken2).Bold();
                    rank++;
                }
            });
        });

    // ── Expenses Section ─────────────────────────────────────────────────────

    private static Action<IContainer> ExpensesSection(SalesSummaryDto s) =>
        c => c.Column(col =>
        {
            col.Item().Text("Expenses by Category").FontSize(13).Bold().FontColor(Colors.Grey.Darken2);
            col.Item().PaddingTop(4).Table(table =>
            {
                table.ColumnsDefinition(cols =>
                {
                    cols.RelativeColumn(3);
                    cols.RelativeColumn(2);
                    cols.RelativeColumn(1);
                });

                table.Header(header =>
                {
                    foreach (var h in new[] { "Category", "Amount", "% of Total" })
                        header.Cell().Background(Colors.Grey.Lighten3).Padding(6)
                            .Text(h).Bold().FontSize(9);
                });

                foreach (var (cat, amount) in s.ExpensesByCategory.OrderByDescending(x => x.Value))
                {
                    var pct = s.TotalExpenses > 0 ? (int)(amount / s.TotalExpenses * 100) : 0;
                    table.Cell().BorderBottom(1).BorderColor(Colors.Grey.Lighten3).Padding(6).Text(cat);
                    table.Cell().BorderBottom(1).BorderColor(Colors.Grey.Lighten3).Padding(6)
                        .Text(amount.ToString("C")).FontColor(Colors.Red.Darken1).Bold();
                    table.Cell().BorderBottom(1).BorderColor(Colors.Grey.Lighten3).Padding(6)
                        .Text($"{pct}%").FontColor(Colors.Grey.Medium);
                }
            });
        });

    // ── Staff Section ────────────────────────────────────────────────────────

    private static Action<IContainer> StaffSection(List<EmployeePerformanceDto> staff) =>
        c => c.Column(col =>
        {
            col.Item().Text("Staff Performance").FontSize(13).Bold().FontColor(Colors.Grey.Darken2);
            col.Item().PaddingTop(4).Table(table =>
            {
                table.ColumnsDefinition(cols =>
                {
                    cols.RelativeColumn(3);
                    cols.RelativeColumn(2);
                    cols.RelativeColumn(1);
                    cols.RelativeColumn(2);
                    cols.RelativeColumn(2);
                });

                table.Header(header =>
                {
                    foreach (var h in new[] { "Employee", "Role", "Shifts", "Sales", "Tips" })
                        header.Cell().Background(Colors.Grey.Lighten3).Padding(6)
                            .Text(h).Bold().FontSize(9);
                });

                foreach (var emp in staff.OrderByDescending(e => e.TotalSalesAmount))
                {
                    table.Cell().BorderBottom(1).BorderColor(Colors.Grey.Lighten3).Padding(6).Text(emp.FullName);
                    table.Cell().BorderBottom(1).BorderColor(Colors.Grey.Lighten3).Padding(6).Text(emp.Role);
                    table.Cell().BorderBottom(1).BorderColor(Colors.Grey.Lighten3).Padding(6)
                        .Text(emp.TotalShifts.ToString());
                    table.Cell().BorderBottom(1).BorderColor(Colors.Grey.Lighten3).Padding(6)
                        .Text(emp.TotalSalesAmount.ToString("C")).FontColor(Colors.Green.Darken2);
                    table.Cell().BorderBottom(1).BorderColor(Colors.Grey.Lighten3).Padding(6)
                        .Text(emp.TotalTipsEarned.ToString("C")).FontColor(Colors.Orange.Darken2);
                }
            });
        });

    // ── Order Receipt ────────────────────────────────────────────────────────

    public Task<string> GenerateOrderReceiptAsync(OrderDto order, CancellationToken ct = default)
    {
        var fileName = $"receipt_{order.Id:N}.pdf";
        var fullPath = Path.Combine(_reportsDir, fileName);
        var relativePath = $"/reports/{fileName}";

        Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Size(PageSizes.A5);
                page.Margin(30);
                page.DefaultTextStyle(x => x.FontSize(10).FontFamily("Arial"));
                page.Content().Element(ComposeReceipt(order));
                page.Footer().Element(ComposeFooter());
            });
        }).GeneratePdf(fullPath);

        return Task.FromResult(relativePath);
    }

    private static Action<IContainer> ComposeReceipt(OrderDto order) =>
        c => c.Column(col =>
        {
            col.Spacing(10);

            col.Item().AlignCenter().Text("Restaurant POS").FontSize(20).Bold().FontColor(Colors.Grey.Darken3);
            col.Item().AlignCenter().Text("ORDER RECEIPT").FontSize(11).FontColor(Colors.Grey.Medium);
            col.Item().LineHorizontal(1).LineColor(Colors.Grey.Lighten2);

            col.Item().Table(table =>
            {
                table.ColumnsDefinition(cols => { cols.RelativeColumn(); cols.RelativeColumn(2); });
                ReceiptInfoRow(table, "Table:", $"Table {order.TableNumber}");
                ReceiptInfoRow(table, "Served by:", order.EmployeeName);
                ReceiptInfoRow(table, "Opened:", order.OpenedAt.ToString("dd MMM yyyy HH:mm"));
                if (order.ClosedAt.HasValue)
                    ReceiptInfoRow(table, "Closed:", order.ClosedAt.Value.ToString("dd MMM yyyy HH:mm"));
                if (!string.IsNullOrEmpty(order.PaymentMethod))
                    ReceiptInfoRow(table, "Payment:", order.PaymentMethod);
                if (!string.IsNullOrEmpty(order.Notes))
                    ReceiptInfoRow(table, "Notes:", order.Notes);
            });

            col.Item().LineHorizontal(1).LineColor(Colors.Grey.Lighten2);
            col.Item().Text("Items").FontSize(11).Bold().FontColor(Colors.Grey.Darken2);

            col.Item().Table(table =>
            {
                table.ColumnsDefinition(cols =>
                {
                    cols.RelativeColumn(3);
                    cols.ConstantColumn(28);
                    cols.RelativeColumn(2);
                    cols.RelativeColumn(2);
                });

                table.Header(header =>
                {
                    foreach (var h in new[] { "Item", "Qty", "Unit", "Total" })
                        header.Cell().Background(Colors.Grey.Lighten3).Padding(5)
                            .Text(h).Bold().FontSize(9).FontColor(Colors.Grey.Medium);
                });

                foreach (var item in order.Items)
                {
                    table.Cell().BorderBottom(1).BorderColor(Colors.Grey.Lighten3).Padding(5)
                        .Text(item.MenuItemName);
                    table.Cell().BorderBottom(1).BorderColor(Colors.Grey.Lighten3).Padding(5)
                        .AlignCenter().Text(item.Quantity.ToString());
                    table.Cell().BorderBottom(1).BorderColor(Colors.Grey.Lighten3).Padding(5)
                        .AlignRight().Text(item.UnitPrice.ToString("C"));
                    table.Cell().BorderBottom(1).BorderColor(Colors.Grey.Lighten3).Padding(5)
                        .AlignRight().Text(item.LineTotal.ToString("C")).Bold().FontColor(Colors.Green.Darken2);
                }
            });

            col.Item().LineHorizontal(1).LineColor(Colors.Grey.Lighten2);

            col.Item().Table(table =>
            {
                table.ColumnsDefinition(cols => { cols.RelativeColumn(); cols.RelativeColumn(); });

                table.Cell().Padding(4).AlignRight().Text("Subtotal:").FontSize(9).FontColor(Colors.Grey.Medium);
                table.Cell().Padding(4).AlignRight().Text(order.Subtotal.ToString("C")).FontSize(10);

                if (order.DiscountAmount > 0)
                {
                    table.Cell().Padding(4).AlignRight().Text("Discount:").FontSize(9).FontColor(Colors.Grey.Medium);
                    table.Cell().Padding(4).AlignRight().Text($"-{order.DiscountAmount:C}").FontSize(10).FontColor(Colors.Red.Darken1);
                }

                if (order.TipAmount > 0)
                {
                    table.Cell().Padding(4).AlignRight().Text("Tip:").FontSize(9).FontColor(Colors.Grey.Medium);
                    table.Cell().Padding(4).AlignRight().Text(order.TipAmount.ToString("C")).FontSize(10).FontColor(Colors.Orange.Darken2);
                }

                var total = order.Subtotal - order.DiscountAmount + order.TipAmount;
                table.Cell().Padding(4).AlignRight().Text("TOTAL:").FontSize(13).Bold().FontColor(Colors.Grey.Darken3);
                table.Cell().Padding(4).AlignRight().Text(total.ToString("C")).FontSize(13).Bold().FontColor(Colors.Green.Darken2);
            });

            col.Item().PaddingTop(8).AlignCenter()
                .Text("Thank you for dining with us!")
                .FontSize(9).Italic().FontColor(Colors.Grey.Medium);
        });

    private static void ReceiptInfoRow(TableDescriptor table, string label, string value)
    {
        table.Cell().Padding(3).Text(label).FontSize(9).FontColor(Colors.Grey.Medium).Bold();
        table.Cell().Padding(3).Text(value).FontSize(9);
    }

    // ── Payroll Report ───────────────────────────────────────────────────────

    public Task<string> GeneratePayrollReportAsync(
        IEnumerable<EmployeePayrollDto> rows,
        DateOnly from,
        DateOnly to,
        CancellationToken ct = default)
    {
        var fileName = $"payroll_{from:yyyyMMdd}_{to:yyyyMMdd}_{Guid.NewGuid():N}.pdf";
        var fullPath = Path.Combine(_reportsDir, fileName);
        var relativePath = $"/reports/{fileName}";

        var rowList = rows.ToList();

        Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Size(PageSizes.A4);
                page.Margin(40);
                page.DefaultTextStyle(x => x.FontSize(10).FontFamily("Arial"));
                page.Header().Element(ComposePayrollHeader(from, to));
                page.Content().Element(ComposePayrollContent(rowList));
                page.Footer().Element(ComposeFooter());
            });
        }).GeneratePdf(fullPath);

        return Task.FromResult(relativePath);
    }

    private static Action<IContainer> ComposePayrollHeader(DateOnly from, DateOnly to) =>
        header => header.PaddingBottom(16).Row(row =>
        {
            row.RelativeItem().Column(col =>
            {
                col.Item().Text("Restaurant POS").FontSize(20).Bold().FontColor(Colors.Grey.Darken3);
                col.Item().Text("Payroll Summary").FontSize(12).FontColor(Colors.Grey.Medium);
            });
            row.ConstantItem(160).AlignRight().Column(col =>
            {
                col.Item().Text($"{from:dd MMM yyyy} — {to:dd MMM yyyy}")
                    .FontSize(10).FontColor(Colors.Grey.Medium);
                col.Item().Text($"Generated: {DateTime.Now:dd MMM yyyy HH:mm}")
                    .FontSize(9).FontColor(Colors.Grey.Lighten1);
            });
        });

    private static Action<IContainer> ComposePayrollContent(List<EmployeePayrollDto> rows) =>
        content => content.Column(col =>
        {
            col.Spacing(16);

            col.Item().Grid(grid =>
            {
                grid.Columns(4);
                grid.Spacing(8);
                KpiCell(grid, "Employees", rows.Count.ToString(), Colors.Blue.Darken2);
                KpiCell(grid, "Total Hours", rows.Sum(r => r.TotalHours).ToString("N1") + "h", Colors.Blue.Darken2);
                KpiCell(grid, "Total Shifts", rows.Sum(r => r.ShiftCount).ToString(), Colors.Purple.Darken2);
                KpiCell(grid, "Total Tips", rows.Sum(r => r.TotalTips).ToString("C"), Colors.Orange.Darken2);
            });

            col.Item().Table(table =>
            {
                table.ColumnsDefinition(cols =>
                {
                    cols.RelativeColumn(3);
                    cols.RelativeColumn(2);
                    cols.ConstantColumn(50);
                    cols.ConstantColumn(65);
                    cols.ConstantColumn(65);
                    cols.RelativeColumn(2);
                });

                table.Header(header =>
                {
                    foreach (var h in new[] { "Employee", "Role", "Shifts", "Hours", "Avg h/shift", "Tips" })
                        header.Cell().Background(Colors.Grey.Lighten3).Padding(7)
                            .Text(h).Bold().FontSize(9);
                });

                foreach (var r in rows)
                {
                    var avg = r.ShiftCount > 0
                        ? (r.TotalHours / r.ShiftCount).ToString("N1") + "h"
                        : "—";
                    table.Cell().BorderBottom(1).BorderColor(Colors.Grey.Lighten3).Padding(7).Text(r.FullName).Bold();
                    table.Cell().BorderBottom(1).BorderColor(Colors.Grey.Lighten3).Padding(7).Text(r.Role).FontColor(Colors.Grey.Medium);
                    table.Cell().BorderBottom(1).BorderColor(Colors.Grey.Lighten3).Padding(7).AlignCenter().Text(r.ShiftCount.ToString());
                    table.Cell().BorderBottom(1).BorderColor(Colors.Grey.Lighten3).Padding(7).AlignRight().Text(r.TotalHours.ToString("N1") + "h").Bold().FontColor(Colors.Blue.Darken2);
                    table.Cell().BorderBottom(1).BorderColor(Colors.Grey.Lighten3).Padding(7).AlignRight().Text(avg).FontColor(Colors.Grey.Medium);
                    table.Cell().BorderBottom(1).BorderColor(Colors.Grey.Lighten3).Padding(7).AlignRight().Text(r.TotalTips.ToString("C")).Bold().FontColor(Colors.Orange.Darken2);
                }

                table.Cell().Background(Colors.Grey.Lighten3).Padding(7).Text("Total").Bold().FontSize(9);
                table.Cell().Background(Colors.Grey.Lighten3).Padding(7).Text(string.Empty).Bold().FontSize(9);
                table.Cell().Background(Colors.Grey.Lighten3).Padding(7).AlignCenter().Text(rows.Sum(r => r.ShiftCount).ToString()).Bold();
                table.Cell().Background(Colors.Grey.Lighten3).Padding(7).AlignRight().Text(rows.Sum(r => r.TotalHours).ToString("N1") + "h").Bold().FontColor(Colors.Blue.Darken2);
                table.Cell().Background(Colors.Grey.Lighten3).Padding(7).Text(string.Empty).Bold();
                table.Cell().Background(Colors.Grey.Lighten3).Padding(7).AlignRight().Text(rows.Sum(r => r.TotalTips).ToString("C")).Bold().FontColor(Colors.Orange.Darken2);
            });
        });

    // ── Footer ───────────────────────────────────────────────────────────────

    private static Action<IContainer> ComposeFooter() =>
        footer => footer
            .PaddingTop(8)
            .BorderTop(1).BorderColor(Colors.Grey.Lighten2)
            .PaddingTop(4)
            .Row(row =>
            {
                row.RelativeItem().Text("Restaurant POS — Confidential")
                    .FontSize(8).FontColor(Colors.Grey.Lighten1);
                row.ConstantItem(80).AlignRight()
                    .Text(text =>
                    {
                        text.Span("Page ").FontSize(8).FontColor(Colors.Grey.Lighten1);
                        text.CurrentPageNumber().FontSize(8).FontColor(Colors.Grey.Lighten1);
                        text.Span(" of ").FontSize(8).FontColor(Colors.Grey.Lighten1);
                        text.TotalPages().FontSize(8).FontColor(Colors.Grey.Lighten1);
                    });
            });
}
