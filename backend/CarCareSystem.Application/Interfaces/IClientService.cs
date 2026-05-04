namespace CarCareSystem.Application.Interfaces;

using DTOs;

public interface IClientService
{
    Task<ClientDto?> GetByIdAsync(Guid id);
    Task<IEnumerable<ClientDto>> GetAllAsync();
    Task<IEnumerable<ClientDto>> GetActiveClientsAsync();
    Task<IEnumerable<ClientDto>> SearchAsync(string searchTerm);
    Task<ClientDto> CreateAsync(CreateClientDto dto);
    Task<ClientDto> UpdateAsync(Guid id, UpdateClientDto dto);
    Task DeleteAsync(Guid id);
}
