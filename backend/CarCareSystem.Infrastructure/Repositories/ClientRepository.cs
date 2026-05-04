namespace CarCareSystem.Infrastructure.Repositories;

using Microsoft.EntityFrameworkCore;
using Domain.Entities;
using Domain.Repositories;
using Persistence;

public class ClientRepository : IClientRepository
{
    private readonly CarCareSystemDbContext _context;

    public ClientRepository(CarCareSystemDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task<Client?> GetByIdAsync(Guid id)
    {
        return await _context.Clients.FindAsync(id);
    }

    public async Task<IEnumerable<Client>> GetAllAsync()
    {
        return await _context.Clients.ToListAsync();
    }

    public async Task<Client?> GetByEmailAsync(string email)
    {
        return await _context.Clients.FirstOrDefaultAsync(c => c.Email == email);
    }

    public async Task<Client?> GetByDocumentAsync(string document)
    {
        return await _context.Clients.FirstOrDefaultAsync(c => c.Document == document);
    }

    public async Task<IEnumerable<Client>> GetActiveClientsAsync()
    {
        return await _context.Clients
            .Where(c => c.IsActive)
            .ToListAsync();
    }

    public async Task<IEnumerable<Client>> SearchAsync(string searchTerm)
    {
        var lowerSearchTerm = searchTerm.ToLower();
        return await _context.Clients
            .Where(c => c.Name.ToLower().Contains(lowerSearchTerm) ||
                        c.Email.ToLower().Contains(lowerSearchTerm) ||
                        c.PhoneNumber.Contains(searchTerm))
            .ToListAsync();
    }

    public async Task AddAsync(Client entity)
    {
        await _context.Clients.AddAsync(entity);
    }

    public void Update(Client entity)
    {
        _context.Clients.Update(entity);
    }

    public void Delete(Client entity)
    {
        _context.Clients.Remove(entity);
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}
