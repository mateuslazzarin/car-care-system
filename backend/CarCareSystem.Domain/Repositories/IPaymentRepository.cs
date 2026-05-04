namespace CarCareSystem.Domain.Repositories;

using Entities;

public interface IPaymentRepository : IRepository<Payment>
{
    Task<IEnumerable<Payment>> GetByClientIdAsync(Guid clientId);
    Task<IEnumerable<Payment>> GetByStatusAsync(PaymentStatus status);
    Task<IEnumerable<Payment>> GetByMethodAsync(PaymentMethod method);
}
