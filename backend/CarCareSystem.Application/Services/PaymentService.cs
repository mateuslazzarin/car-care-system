namespace CarCareSystem.Application.Services;

using Domain.Entities;
using Domain.Repositories;
using DTOs;
using Interfaces;

public class PaymentService : IPaymentService
{
    private readonly IPaymentRepository _paymentRepository;
    private readonly IClientRepository _clientRepository;

    public PaymentService(IPaymentRepository paymentRepository, IClientRepository clientRepository)
    {
        _paymentRepository = paymentRepository ?? throw new ArgumentNullException(nameof(paymentRepository));
        _clientRepository = clientRepository ?? throw new ArgumentNullException(nameof(clientRepository));
    }

    public async Task<PaymentDto?> GetByIdAsync(Guid id)
    {
        if (id == Guid.Empty)
            throw new ArgumentException("Payment ID cannot be empty", nameof(id));

        var payment = await _paymentRepository.GetByIdAsync(id);
        return payment != null ? MapToDto(payment) : null;
    }

    public async Task<IEnumerable<PaymentDto>> GetAllAsync()
    {
        var payments = await _paymentRepository.GetAllAsync();
        return payments.Select(MapToDto);
    }

    public async Task<IEnumerable<PaymentDto>> GetByClientIdAsync(Guid clientId)
    {
        if (clientId == Guid.Empty)
            throw new ArgumentException("Client ID cannot be empty", nameof(clientId));

        var payments = await _paymentRepository.GetByClientIdAsync(clientId);
        return payments.Select(MapToDto);
    }

    public async Task<IEnumerable<PaymentDto>> GetByStatusAsync(PaymentStatusDto status)
    {
        var domainStatus = (PaymentStatus)(int)status;
        var payments = await _paymentRepository.GetByStatusAsync(domainStatus);
        return payments.Select(MapToDto);
    }

    public async Task<PaymentDto> CreateAsync(CreatePaymentDto dto)
    {
        ValidateCreatePaymentDto(dto);

        var client = await _clientRepository.GetByIdAsync(dto.ClientId);
        if (client == null)
            throw new KeyNotFoundException($"Client with ID '{dto.ClientId}' not found.");

        var payment = new Payment
        {
            Id = Guid.NewGuid(),
            ClientId = dto.ClientId,
            AppointmentId = dto.AppointmentId,
            Amount = dto.Amount,
            Method = (PaymentMethod)(int)dto.Method,
            Status = PaymentStatus.Pending,
            PaymentDate = DateTime.UtcNow,
            Reference = dto.Reference,
            Notes = dto.Notes,
            CreatedAt = DateTime.UtcNow
        };

        await _paymentRepository.AddAsync(payment);
        await _paymentRepository.SaveChangesAsync();

        return MapToDto(payment);
    }

    public async Task<PaymentDto> UpdateAsync(Guid id, UpdatePaymentDto dto)
    {
        if (id == Guid.Empty)
            throw new ArgumentException("Payment ID cannot be empty", nameof(id));

        var payment = await _paymentRepository.GetByIdAsync(id);
        if (payment == null)
            throw new KeyNotFoundException($"Payment with ID '{id}' not found.");

        payment.Status = (PaymentStatus)(int)dto.Status;
        payment.Notes = dto.Notes;
        payment.UpdatedAt = DateTime.UtcNow;

        _paymentRepository.Update(payment);
        await _paymentRepository.SaveChangesAsync();

        return MapToDto(payment);
    }

    public async Task DeleteAsync(Guid id)
    {
        if (id == Guid.Empty)
            throw new ArgumentException("Payment ID cannot be empty", nameof(id));

        var payment = await _paymentRepository.GetByIdAsync(id);
        if (payment == null)
            throw new KeyNotFoundException($"Payment with ID '{id}' not found.");

        _paymentRepository.Delete(payment);
        await _paymentRepository.SaveChangesAsync();
    }

    private void ValidateCreatePaymentDto(CreatePaymentDto dto)
    {
        if (dto.ClientId == Guid.Empty)
            throw new ArgumentException("Client ID is required.", nameof(dto.ClientId));

        if (dto.Amount <= 0)
            throw new ArgumentException("Amount must be greater than zero.", nameof(dto.Amount));
    }

    private static PaymentDto MapToDto(Payment payment)
    {
        return new PaymentDto
        {
            Id = payment.Id,
            ClientId = payment.ClientId,
            AppointmentId = payment.AppointmentId,
            Amount = payment.Amount,
            Method = (PaymentMethodDto)(int)payment.Method,
            Status = (PaymentStatusDto)(int)payment.Status,
            PaymentDate = payment.PaymentDate,
            Reference = payment.Reference,
            Notes = payment.Notes,
            CreatedAt = payment.CreatedAt,
            UpdatedAt = payment.UpdatedAt
        };
    }
}
