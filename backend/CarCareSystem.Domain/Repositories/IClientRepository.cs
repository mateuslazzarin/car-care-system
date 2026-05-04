namespace CarCareSystem.Domain.Repositories;

using Entities;

public interface IClientRepository : IRepository<Client>
{
    Task<Client?> GetByEmailAsync(string email);
    Task<Client?> GetByDocumentAsync(string document);
    Task<IEnumerable<Client>> GetActiveClientsAsync();
    Task<IEnumerable<Client>> SearchAsync(string searchTerm);
}
