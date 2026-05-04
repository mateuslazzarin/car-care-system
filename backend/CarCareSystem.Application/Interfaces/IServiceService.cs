namespace CarCareSystem.Application.Interfaces;

using DTOs;

public interface IServiceService
{
    Task<ServiceDto?> GetByIdAsync(Guid id);
    Task<IEnumerable<ServiceDto>> GetAllAsync();
    Task<IEnumerable<ServiceDto>> GetByClientIdAsync(Guid clientId);
    Task<IEnumerable<ServiceDto>> GetByCategoryAsync(ServiceCategoryDto category);
    Task<IEnumerable<ServiceDto>> GetActiveServicesAsync();
    Task<ServiceDto> CreateAsync(CreateServiceDto dto);
    Task<ServiceDto> UpdateAsync(Guid id, UpdateServiceDto dto);
    Task DeleteAsync(Guid id);
}
