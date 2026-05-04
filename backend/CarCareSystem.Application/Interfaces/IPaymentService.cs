namespace CarCareSystem.Application.Interfaces;

using DTOs;

public interface IPaymentService
{
    Task<PaymentDto?> GetByIdAsync(Guid id);
    Task<IEnumerable<PaymentDto>> GetAllAsync();
    Task<IEnumerable<PaymentDto>> GetByClientIdAsync(Guid clientId);
    Task<IEnumerable<PaymentDto>> GetByStatusAsync(PaymentStatusDto status);
    Task<PaymentDto> CreateAsync(CreatePaymentDto dto);
    Task<PaymentDto> UpdateAsync(Guid id, UpdatePaymentDto dto);
    Task DeleteAsync(Guid id);
}
