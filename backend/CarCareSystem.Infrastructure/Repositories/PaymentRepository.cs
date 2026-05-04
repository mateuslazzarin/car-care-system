namespace CarCareSystem.Infrastructure.Repositories;

using Microsoft.EntityFrameworkCore;
using Domain.Entities;
using Domain.Repositories;
using Persistence;

public class PaymentRepository : IPaymentRepository
{
    private readonly CarCareSystemDbContext _context;

    public PaymentRepository(CarCareSystemDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task<Payment?> GetByIdAsync(Guid id)
    {
        return await _context.Payments.FindAsync(id);
    }

    public async Task<IEnumerable<Payment>> GetAllAsync()
    {
        return await _context.Payments.ToListAsync();
    }

    public async Task<IEnumerable<Payment>> GetByClientIdAsync(Guid clientId)
    {
        return await _context.Payments
            .Where(p => p.ClientId == clientId)
            .OrderByDescending(p => p.PaymentDate)
            .ToListAsync();
    }

    public async Task<IEnumerable<Payment>> GetByStatusAsync(PaymentStatus status)
    {
        return await _context.Payments
            .Where(p => p.Status == status)
            .OrderByDescending(p => p.PaymentDate)
            .ToListAsync();
    }

    public async Task<IEnumerable<Payment>> GetByMethodAsync(PaymentMethod method)
    {
        return await _context.Payments
            .Where(p => p.Method == method)
            .OrderByDescending(p => p.PaymentDate)
            .ToListAsync();
    }

    public async Task AddAsync(Payment entity)
    {
        await _context.Payments.AddAsync(entity);
    }

    public void Update(Payment entity)
    {
        _context.Payments.Update(entity);
    }

    public void Delete(Payment entity)
    {
        _context.Payments.Remove(entity);
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}
