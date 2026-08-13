using ProductManagement.Domain.Entities;
namespace ProductManagement.Application.Interface;

public interface IOrderRepository
{
    Task<Order?> GetByIdAsync(Guid id, CancellationToken ct);
    Task AddAsync(Order order, CancellationToken ct);
}
