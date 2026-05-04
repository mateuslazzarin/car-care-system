namespace CarCareSystem.Application.Services;

using Domain.Entities;
using Domain.Repositories;
using DTOs;
using Interfaces;

public class ClientService : IClientService
{
    private readonly IClientRepository _clientRepository;

    public ClientService(IClientRepository clientRepository)
    {
        _clientRepository = clientRepository ?? throw new ArgumentNullException(nameof(clientRepository));
    }

    public async Task<ClientDto?> GetByIdAsync(Guid id)
    {
        if (id == Guid.Empty)
            throw new ArgumentException("Client ID cannot be empty", nameof(id));

        var client = await _clientRepository.GetByIdAsync(id);
        return client != null ? MapToDto(client) : null;
    }

    public async Task<IEnumerable<ClientDto>> GetAllAsync()
    {
        var clients = await _clientRepository.GetAllAsync();
        return clients.Select(MapToDto);
    }

    public async Task<IEnumerable<ClientDto>> GetActiveClientsAsync()
    {
        var clients = await _clientRepository.GetActiveClientsAsync();
        return clients.Select(MapToDto);
    }

    public async Task<IEnumerable<ClientDto>> SearchAsync(string searchTerm)
    {
        if (string.IsNullOrWhiteSpace(searchTerm))
            throw new ArgumentException("Search term cannot be empty", nameof(searchTerm));

        var clients = await _clientRepository.SearchAsync(searchTerm);
        return clients.Select(MapToDto);
    }

    public async Task<ClientDto> CreateAsync(CreateClientDto dto)
    {
        ValidateCreateClientDto(dto);

        // Check if email already exists
        var existingClient = await _clientRepository.GetByEmailAsync(dto.Email);
        if (existingClient != null)
            throw new InvalidOperationException($"A client with email '{dto.Email}' already exists.");

        var client = new Client
        {
            Id = Guid.NewGuid(),
            Name = dto.Name,
            Email = dto.Email,
            PhoneNumber = dto.PhoneNumber,
            Document = dto.Document,
            Address = dto.Address,
            City = dto.City,
            State = dto.State,
            ZipCode = dto.ZipCode,
            CreatedAt = DateTime.UtcNow,
            IsActive = true
        };

        await _clientRepository.AddAsync(client);
        await _clientRepository.SaveChangesAsync();

        return MapToDto(client);
    }

    public async Task<ClientDto> UpdateAsync(Guid id, UpdateClientDto dto)
    {
        if (id == Guid.Empty)
            throw new ArgumentException("Client ID cannot be empty", nameof(id));

        ValidateUpdateClientDto(dto);

        var client = await _clientRepository.GetByIdAsync(id);
        if (client == null)
            throw new KeyNotFoundException($"Client with ID '{id}' not found.");

        // Check if email is being changed and already exists
        if (client.Email != dto.Email)
        {
            var existingClient = await _clientRepository.GetByEmailAsync(dto.Email);
            if (existingClient != null)
                throw new InvalidOperationException($"A client with email '{dto.Email}' already exists.");
        }

        client.Name = dto.Name;
        client.Email = dto.Email;
        client.PhoneNumber = dto.PhoneNumber;
        client.Document = dto.Document;
        client.Address = dto.Address;
        client.City = dto.City;
        client.State = dto.State;
        client.ZipCode = dto.ZipCode;
        client.UpdatedAt = DateTime.UtcNow;

        _clientRepository.Update(client);
        await _clientRepository.SaveChangesAsync();

        return MapToDto(client);
    }

    public async Task DeleteAsync(Guid id)
    {
        if (id == Guid.Empty)
            throw new ArgumentException("Client ID cannot be empty", nameof(id));

        var client = await _clientRepository.GetByIdAsync(id);
        if (client == null)
            throw new KeyNotFoundException($"Client with ID '{id}' not found.");

        client.IsActive = false;
        client.UpdatedAt = DateTime.UtcNow;

        _clientRepository.Update(client);
        await _clientRepository.SaveChangesAsync();
    }

    private void ValidateCreateClientDto(CreateClientDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Name))
            throw new ArgumentException("Client name is required.", nameof(dto.Name));

        if (string.IsNullOrWhiteSpace(dto.Email))
            throw new ArgumentException("Client email is required.", nameof(dto.Email));

        if (string.IsNullOrWhiteSpace(dto.PhoneNumber))
            throw new ArgumentException("Client phone number is required.", nameof(dto.PhoneNumber));

        if (!IsValidEmail(dto.Email))
            throw new ArgumentException("Client email is invalid.", nameof(dto.Email));
    }

    private void ValidateUpdateClientDto(UpdateClientDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Name))
            throw new ArgumentException("Client name is required.", nameof(dto.Name));

        if (string.IsNullOrWhiteSpace(dto.Email))
            throw new ArgumentException("Client email is required.", nameof(dto.Email));

        if (string.IsNullOrWhiteSpace(dto.PhoneNumber))
            throw new ArgumentException("Client phone number is required.", nameof(dto.PhoneNumber));

        if (!IsValidEmail(dto.Email))
            throw new ArgumentException("Client email is invalid.", nameof(dto.Email));
    }

    private static bool IsValidEmail(string email)
    {
        try
        {
            var addr = new System.Net.Mail.MailAddress(email);
            return addr.Address == email;
        }
        catch
        {
            return false;
        }
    }

    private static ClientDto MapToDto(Client client)
    {
        return new ClientDto
        {
            Id = client.Id,
            Name = client.Name,
            Email = client.Email,
            PhoneNumber = client.PhoneNumber,
            Document = client.Document,
            Address = client.Address,
            City = client.City,
            State = client.State,
            ZipCode = client.ZipCode,
            CreatedAt = client.CreatedAt,
            UpdatedAt = client.UpdatedAt,
            IsActive = client.IsActive
        };
    }
}
