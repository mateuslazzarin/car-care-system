namespace CarCareSystem.Domain.Repositories;

using Entities;

public interface IServiceRepository : IRepository<Service>
{
    Task<IEnumerable<Service>> GetByClientIdAsync(Guid clientId);
    Task<IEnumerable<Service>> GetByCategoryAsync(ServiceCategory category);
    Task<IEnumerable<Service>> GetActiveServicesAsync();
}
