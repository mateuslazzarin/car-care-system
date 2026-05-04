namespace CarCareSystem.Infrastructure.Repositories;

using Microsoft.EntityFrameworkCore;
using Domain.Entities;
using Domain.Repositories;
using Persistence;

public class ServiceRepository : IServiceRepository
{
    private readonly CarCareSystemDbContext _context;

    public ServiceRepository(CarCareSystemDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task<Service?> GetByIdAsync(Guid id)
    {
        return await _context.Services.FindAsync(id);
    }

    public async Task<IEnumerable<Service>> GetAllAsync()
    {
        return await _context.Services.ToListAsync();
    }

    public async Task<IEnumerable<Service>> GetByClientIdAsync(Guid clientId)
    {
        return await _context.Services
            .Where(s => s.ClientId == clientId)
            .ToListAsync();
    }

    public async Task<IEnumerable<Service>> GetByCategoryAsync(ServiceCategory category)
    {
        return await _context.Services
            .Where(s => s.Category == category)
            .ToListAsync();
    }

    public async Task<IEnumerable<Service>> GetActiveServicesAsync()
    {
        return await _context.Services
            .Where(s => s.IsActive)
            .ToListAsync();
    }

    public async Task AddAsync(Service entity)
    {
        await _context.Services.AddAsync(entity);
    }

    public void Update(Service entity)
    {
        _context.Services.Update(entity);
    }

    public void Delete(Service entity)
    {
        _context.Services.Remove(entity);
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}
