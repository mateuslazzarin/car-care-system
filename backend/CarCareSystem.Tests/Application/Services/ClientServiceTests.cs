namespace CarCareSystem.Tests.Application.Services;

using Xunit;
using Moq;
using Domain.Entities;
using Domain.Repositories;
using Application.DTOs;
using Application.Services;

public class ClientServiceTests
{
    private readonly Mock<IClientRepository> _clientRepositoryMock;
    private readonly ClientService _clientService;

    public ClientServiceTests()
    {
        _clientRepositoryMock = new Mock<IClientRepository>();
        _clientService = new ClientService(_clientRepositoryMock.Object);
    }

    [Fact]
    public async Task GetByIdAsync_WithValidId_ReturnsClientDto()
    {
        // Arrange
        var clientId = Guid.NewGuid();
        var client = new Client
        {
            Id = clientId,
            Name = "John Doe",
            Email = "john@example.com",
            PhoneNumber = "1234567890",
            CreatedAt = DateTime.UtcNow,
            IsActive = true
        };

        _clientRepositoryMock
            .Setup(r => r.GetByIdAsync(clientId))
            .ReturnsAsync(client);

        // Act
        var result = await _clientService.GetByIdAsync(clientId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(clientId, result.Id);
        Assert.Equal("John Doe", result.Name);
        Assert.Equal("john@example.com", result.Email);
        _clientRepositoryMock.Verify(r => r.GetByIdAsync(clientId), Times.Once);
    }

    [Fact]
    public async Task GetByIdAsync_WithEmptyId_ThrowsArgumentException()
    {
        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() => _clientService.GetByIdAsync(Guid.Empty));
    }

    [Fact]
    public async Task GetAllAsync_ReturnsAllClients()
    {
        // Arrange
        var clients = new List<Client>
        {
            new Client { Id = Guid.NewGuid(), Name = "Client 1", Email = "client1@example.com", PhoneNumber = "123", CreatedAt = DateTime.UtcNow, IsActive = true },
            new Client { Id = Guid.NewGuid(), Name = "Client 2", Email = "client2@example.com", PhoneNumber = "456", CreatedAt = DateTime.UtcNow, IsActive = true }
        };

        _clientRepositoryMock
            .Setup(r => r.GetAllAsync())
            .ReturnsAsync(clients);

        // Act
        var result = await _clientService.GetAllAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count());
        _clientRepositoryMock.Verify(r => r.GetAllAsync(), Times.Once);
    }

    [Fact]
    public async Task CreateAsync_WithValidData_CreatesClient()
    {
        // Arrange
        var createDto = new CreateClientDto
        {
            Name = "New Client",
            Email = "newclient@example.com",
            PhoneNumber = "9876543210"
        };

        _clientRepositoryMock
            .Setup(r => r.GetByEmailAsync(createDto.Email))
            .ReturnsAsync((Client?)null);

        _clientRepositoryMock
            .Setup(r => r.AddAsync(It.IsAny<Client>()))
            .Returns(Task.CompletedTask);

        _clientRepositoryMock
            .Setup(r => r.SaveChangesAsync())
            .Returns(Task.CompletedTask);

        // Act
        var result = await _clientService.CreateAsync(createDto);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(createDto.Name, result.Name);
        Assert.Equal(createDto.Email, result.Email);
        Assert.Equal(createDto.PhoneNumber, result.PhoneNumber);
        Assert.True(result.IsActive);
        _clientRepositoryMock.Verify(r => r.AddAsync(It.IsAny<Client>()), Times.Once);
        _clientRepositoryMock.Verify(r => r.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task CreateAsync_WithEmptyName_ThrowsArgumentException()
    {
        // Arrange
        var createDto = new CreateClientDto
        {
            Name = string.Empty,
            Email = "test@example.com",
            PhoneNumber = "123"
        };

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() => _clientService.CreateAsync(createDto));
    }

    [Fact]
    public async Task CreateAsync_WithDuplicateEmail_ThrowsInvalidOperationException()
    {
        // Arrange
        var createDto = new CreateClientDto
        {
            Name = "Test",
            Email = "duplicate@example.com",
            PhoneNumber = "123"
        };

        var existingClient = new Client
        {
            Id = Guid.NewGuid(),
            Name = "Existing",
            Email = createDto.Email,
            PhoneNumber = "456",
            CreatedAt = DateTime.UtcNow
        };

        _clientRepositoryMock
            .Setup(r => r.GetByEmailAsync(createDto.Email))
            .ReturnsAsync(existingClient);

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => _clientService.CreateAsync(createDto));
    }

    [Fact]
    public async Task UpdateAsync_WithValidData_UpdatesClient()
    {
        // Arrange
        var clientId = Guid.NewGuid();
        var existingClient = new Client
        {
            Id = clientId,
            Name = "Old Name",
            Email = "old@example.com",
            PhoneNumber = "123",
            CreatedAt = DateTime.UtcNow,
            IsActive = true
        };

        var updateDto = new UpdateClientDto
        {
            Name = "New Name",
            Email = "new@example.com",
            PhoneNumber = "456"
        };

        _clientRepositoryMock
            .Setup(r => r.GetByIdAsync(clientId))
            .ReturnsAsync(existingClient);

        _clientRepositoryMock
            .Setup(r => r.GetByEmailAsync(updateDto.Email))
            .ReturnsAsync((Client?)null);

        _clientRepositoryMock
            .Setup(r => r.Update(It.IsAny<Client>()))
            .Callback<Client>(c => existingClient = c);

        _clientRepositoryMock
            .Setup(r => r.SaveChangesAsync())
            .Returns(Task.CompletedTask);

        // Act
        var result = await _clientService.UpdateAsync(clientId, updateDto);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(updateDto.Name, result.Name);
        Assert.Equal(updateDto.Email, result.Email);
        _clientRepositoryMock.Verify(r => r.Update(It.IsAny<Client>()), Times.Once);
        _clientRepositoryMock.Verify(r => r.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task DeleteAsync_WithValidId_DeactivatesClient()
    {
        // Arrange
        var clientId = Guid.NewGuid();
        var client = new Client
        {
            Id = clientId,
            Name = "Client",
            Email = "client@example.com",
            PhoneNumber = "123",
            CreatedAt = DateTime.UtcNow,
            IsActive = true
        };

        _clientRepositoryMock
            .Setup(r => r.GetByIdAsync(clientId))
            .ReturnsAsync(client);

        _clientRepositoryMock
            .Setup(r => r.Update(It.IsAny<Client>()))
            .Callback<Client>(c => client = c);

        _clientRepositoryMock
            .Setup(r => r.SaveChangesAsync())
            .Returns(Task.CompletedTask);

        // Act
        await _clientService.DeleteAsync(clientId);

        // Assert
        Assert.False(client.IsActive);
        _clientRepositoryMock.Verify(r => r.Update(It.IsAny<Client>()), Times.Once);
        _clientRepositoryMock.Verify(r => r.SaveChangesAsync(), Times.Once);
    }
}
