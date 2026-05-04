namespace CarCareSystem.Application.Services;

using Domain.Entities;
using Domain.Repositories;
using DTOs;
using Interfaces;

public class ServiceService : IServiceService
{
    private readonly IServiceRepository _serviceRepository;
    private readonly IClientRepository _clientRepository;

    public ServiceService(IServiceRepository serviceRepository, IClientRepository clientRepository)
    {
        _serviceRepository = serviceRepository ?? throw new ArgumentNullException(nameof(serviceRepository));
        _clientRepository = clientRepository ?? throw new ArgumentNullException(nameof(clientRepository));
    }

    public async Task<ServiceDto?> GetByIdAsync(Guid id)
    {
        if (id == Guid.Empty)
            throw new ArgumentException("Service ID cannot be empty", nameof(id));

        var service = await _serviceRepository.GetByIdAsync(id);
        return service != null ? MapToDto(service) : null;
    }

    public async Task<IEnumerable<ServiceDto>> GetAllAsync()
    {
        var services = await _serviceRepository.GetAllAsync();
        return services.Select(MapToDto);
    }

    public async Task<IEnumerable<ServiceDto>> GetByClientIdAsync(Guid clientId)
    {
        if (clientId == Guid.Empty)
            throw new ArgumentException("Client ID cannot be empty", nameof(clientId));

        var services = await _serviceRepository.GetByClientIdAsync(clientId);
        return services.Select(MapToDto);
    }

    public async Task<IEnumerable<ServiceDto>> GetByCategoryAsync(ServiceCategoryDto category)
    {
        var domainCategory = (ServiceCategory)(int)category;
        var services = await _serviceRepository.GetByCategoryAsync(domainCategory);
        return services.Select(MapToDto);
    }

    public async Task<IEnumerable<ServiceDto>> GetActiveServicesAsync()
    {
        var services = await _serviceRepository.GetActiveServicesAsync();
        return services.Select(MapToDto);
    }

    public async Task<ServiceDto> CreateAsync(CreateServiceDto dto)
    {
        ValidateCreateServiceDto(dto);

        var client = await _clientRepository.GetByIdAsync(dto.ClientId);
        if (client == null)
            throw new KeyNotFoundException($"Client with ID '{dto.ClientId}' not found.");

        var service = new Service
        {
            Id = Guid.NewGuid(),
            ClientId = dto.ClientId,
            Name = dto.Name,
            Description = dto.Description,
            Category = (ServiceCategory)(int)dto.Category,
            Price = dto.Price,
            DurationMinutes = dto.DurationMinutes,
            CreatedAt = DateTime.UtcNow,
            IsActive = true
        };

        await _serviceRepository.AddAsync(service);
        await _serviceRepository.SaveChangesAsync();

        return MapToDto(service);
    }

    public async Task<ServiceDto> UpdateAsync(Guid id, UpdateServiceDto dto)
    {
        if (id == Guid.Empty)
            throw new ArgumentException("Service ID cannot be empty", nameof(id));

        ValidateUpdateServiceDto(dto);

        var service = await _serviceRepository.GetByIdAsync(id);
        if (service == null)
            throw new KeyNotFoundException($"Service with ID '{id}' not found.");

        service.Name = dto.Name;
        service.Description = dto.Description;
        service.Category = (ServiceCategory)(int)dto.Category;
        service.Price = dto.Price;
        service.DurationMinutes = dto.DurationMinutes;
        service.UpdatedAt = DateTime.UtcNow;

        _serviceRepository.Update(service);
        await _serviceRepository.SaveChangesAsync();

        return MapToDto(service);
    }

    public async Task DeleteAsync(Guid id)
    {
        if (id == Guid.Empty)
            throw new ArgumentException("Service ID cannot be empty", nameof(id));

        var service = await _serviceRepository.GetByIdAsync(id);
        if (service == null)
            throw new KeyNotFoundException($"Service with ID '{id}' not found.");

        service.IsActive = false;
        service.UpdatedAt = DateTime.UtcNow;

        _serviceRepository.Update(service);
        await _serviceRepository.SaveChangesAsync();
    }

    private void ValidateCreateServiceDto(CreateServiceDto dto)
    {
        if (dto.ClientId == Guid.Empty)
            throw new ArgumentException("Client ID is required.", nameof(dto.ClientId));

        if (string.IsNullOrWhiteSpace(dto.Name))
            throw new ArgumentException("Service name is required.", nameof(dto.Name));

        if (dto.Price < 0)
            throw new ArgumentException("Service price cannot be negative.", nameof(dto.Price));

        if (dto.DurationMinutes <= 0)
            throw new ArgumentException("Service duration must be greater than zero.", nameof(dto.DurationMinutes));
    }

    private void ValidateUpdateServiceDto(UpdateServiceDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Name))
            throw new ArgumentException("Service name is required.", nameof(dto.Name));

        if (dto.Price < 0)
            throw new ArgumentException("Service price cannot be negative.", nameof(dto.Price));

        if (dto.DurationMinutes <= 0)
            throw new ArgumentException("Service duration must be greater than zero.", nameof(dto.DurationMinutes));
    }

    private static ServiceDto MapToDto(Service service)
    {
        return new ServiceDto
        {
            Id = service.Id,
            ClientId = service.ClientId,
            Name = service.Name,
            Description = service.Description,
            Category = (ServiceCategoryDto)(int)service.Category,
            Price = service.Price,
            DurationMinutes = service.DurationMinutes,
            CreatedAt = service.CreatedAt,
            UpdatedAt = service.UpdatedAt,
            IsActive = service.IsActive
        };
    }
}
