namespace CarCareSystem.Tests.Application.Services;

using Xunit;
using Moq;
using Domain.Entities;
using Domain.Repositories;
using Application.DTOs;
using Application.Services;

public class AppointmentServiceTests
{
    private readonly Mock<IAppointmentRepository> _appointmentRepositoryMock;
    private readonly Mock<IClientRepository> _clientRepositoryMock;
    private readonly AppointmentService _appointmentService;

    public AppointmentServiceTests()
    {
        _appointmentRepositoryMock = new Mock<IAppointmentRepository>();
        _clientRepositoryMock = new Mock<IClientRepository>();
        _appointmentService = new AppointmentService(_appointmentRepositoryMock.Object, _clientRepositoryMock.Object);
    }

    [Fact]
    public async Task GetByIdAsync_WithValidId_ReturnsAppointmentDto()
    {
        // Arrange
        var appointmentId = Guid.NewGuid();
        var clientId = Guid.NewGuid();
        var appointment = new Appointment
        {
            Id = appointmentId,
            ClientId = clientId,
            ScheduledDate = DateTime.UtcNow.AddDays(1),
            Status = AppointmentStatus.Scheduled,
            TotalPrice = 0,
            CreatedAt = DateTime.UtcNow
        };

        _appointmentRepositoryMock
            .Setup(r => r.GetByIdAsync(appointmentId))
            .ReturnsAsync(appointment);

        // Act
        var result = await _appointmentService.GetByIdAsync(appointmentId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(appointmentId, result.Id);
        Assert.Equal(AppointmentStatusDto.Scheduled, result.Status);
        _appointmentRepositoryMock.Verify(r => r.GetByIdAsync(appointmentId), Times.Once);
    }

    [Fact]
    public async Task CreateAsync_WithValidData_CreatesAppointment()
    {
        // Arrange
        var clientId = Guid.NewGuid();
        var client = new Client
        {
            Id = clientId,
            Name = "Test",
            Email = "test@example.com",
            PhoneNumber = "123",
            CreatedAt = DateTime.UtcNow
        };

        var futureDate = DateTime.UtcNow.AddDays(1);
        var createDto = new CreateAppointmentDto
        {
            ClientId = clientId,
            ScheduledDate = futureDate
        };

        _clientRepositoryMock
            .Setup(r => r.GetByIdAsync(clientId))
            .ReturnsAsync(client);

        _appointmentRepositoryMock
            .Setup(r => r.AddAsync(It.IsAny<Appointment>()))
            .Returns(Task.CompletedTask);

        _appointmentRepositoryMock
            .Setup(r => r.SaveChangesAsync())
            .Returns(Task.CompletedTask);

        // Act
        var result = await _appointmentService.CreateAsync(createDto);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(clientId, result.ClientId);
        Assert.Equal(AppointmentStatusDto.Scheduled, result.Status);
        _appointmentRepositoryMock.Verify(r => r.AddAsync(It.IsAny<Appointment>()), Times.Once);
    }

    [Fact]
    public async Task CreateAsync_WithPastDate_ThrowsArgumentException()
    {
        // Arrange
        var clientId = Guid.NewGuid();
        var pastDate = DateTime.UtcNow.AddHours(-1);
        var createDto = new CreateAppointmentDto
        {
            ClientId = clientId,
            ScheduledDate = pastDate
        };

        var client = new Client
        {
            Id = clientId,
            Name = "Test",
            Email = "test@example.com",
            PhoneNumber = "123",
            CreatedAt = DateTime.UtcNow
        };

        _clientRepositoryMock
            .Setup(r => r.GetByIdAsync(clientId))
            .ReturnsAsync(client);

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() => _appointmentService.CreateAsync(createDto));
    }

    [Fact]
    public async Task GetByStatusAsync_ReturnsAppointmentsByStatus()
    {
        // Arrange
        var appointments = new List<Appointment>
        {
            new Appointment { Id = Guid.NewGuid(), ClientId = Guid.NewGuid(), ScheduledDate = DateTime.UtcNow.AddDays(1), Status = AppointmentStatus.Scheduled, TotalPrice = 0, CreatedAt = DateTime.UtcNow },
            new Appointment { Id = Guid.NewGuid(), ClientId = Guid.NewGuid(), ScheduledDate = DateTime.UtcNow.AddDays(2), Status = AppointmentStatus.Scheduled, TotalPrice = 0, CreatedAt = DateTime.UtcNow }
        };

        _appointmentRepositoryMock
            .Setup(r => r.GetByStatusAsync(AppointmentStatus.Scheduled))
            .ReturnsAsync(appointments);

        // Act
        var result = await _appointmentService.GetByStatusAsync(AppointmentStatusDto.Scheduled);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count());
        _appointmentRepositoryMock.Verify(r => r.GetByStatusAsync(AppointmentStatus.Scheduled), Times.Once);
    }
}
