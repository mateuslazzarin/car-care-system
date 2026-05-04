namespace CarCareSystem.Tests.Application.Services;

using Xunit;
using Moq;
using Domain.Entities;
using Domain.Repositories;
using Application.DTOs;
using Application.Services;

public class PaymentServiceTests
{
    private readonly Mock<IPaymentRepository> _paymentRepositoryMock;
    private readonly Mock<IClientRepository> _clientRepositoryMock;
    private readonly PaymentService _paymentService;

    public PaymentServiceTests()
    {
        _paymentRepositoryMock = new Mock<IPaymentRepository>();
        _clientRepositoryMock = new Mock<IClientRepository>();
        _paymentService = new PaymentService(_paymentRepositoryMock.Object, _clientRepositoryMock.Object);
    }

    [Fact]
    public async Task GetByIdAsync_WithValidId_ReturnsPaymentDto()
    {
        // Arrange
        var paymentId = Guid.NewGuid();
        var clientId = Guid.NewGuid();
        var payment = new Payment
        {
            Id = paymentId,
            ClientId = clientId,
            Amount = 100.00m,
            Method = PaymentMethod.CreditCard,
            Status = PaymentStatus.Completed,
            PaymentDate = DateTime.UtcNow,
            CreatedAt = DateTime.UtcNow
        };

        _paymentRepositoryMock
            .Setup(r => r.GetByIdAsync(paymentId))
            .ReturnsAsync(payment);

        // Act
        var result = await _paymentService.GetByIdAsync(paymentId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(paymentId, result.Id);
        Assert.Equal(100.00m, result.Amount);
        _paymentRepositoryMock.Verify(r => r.GetByIdAsync(paymentId), Times.Once);
    }

    [Fact]
    public async Task CreateAsync_WithValidData_CreatesPayment()
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

        var createDto = new CreatePaymentDto
        {
            ClientId = clientId,
            Amount = 150.00m,
            Method = PaymentMethodDto.Pix
        };

        _clientRepositoryMock
            .Setup(r => r.GetByIdAsync(clientId))
            .ReturnsAsync(client);

        _paymentRepositoryMock
            .Setup(r => r.AddAsync(It.IsAny<Payment>()))
            .Returns(Task.CompletedTask);

        _paymentRepositoryMock
            .Setup(r => r.SaveChangesAsync())
            .Returns(Task.CompletedTask);

        // Act
        var result = await _paymentService.CreateAsync(createDto);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(clientId, result.ClientId);
        Assert.Equal(150.00m, result.Amount);
        Assert.Equal(PaymentStatusDto.Pending, result.Status);
        _paymentRepositoryMock.Verify(r => r.AddAsync(It.IsAny<Payment>()), Times.Once);
    }

    [Fact]
    public async Task CreateAsync_WithZeroAmount_ThrowsArgumentException()
    {
        // Arrange
        var createDto = new CreatePaymentDto
        {
            ClientId = Guid.NewGuid(),
            Amount = 0,
            Method = PaymentMethodDto.Cash
        };

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() => _paymentService.CreateAsync(createDto));
    }

    [Fact]
    public async Task GetByClientIdAsync_ReturnsClientPayments()
    {
        // Arrange
        var clientId = Guid.NewGuid();
        var payments = new List<Payment>
        {
            new Payment { Id = Guid.NewGuid(), ClientId = clientId, Amount = 100, Status = PaymentStatus.Completed, PaymentDate = DateTime.UtcNow, CreatedAt = DateTime.UtcNow },
            new Payment { Id = Guid.NewGuid(), ClientId = clientId, Amount = 200, Status = PaymentStatus.Pending, PaymentDate = DateTime.UtcNow, CreatedAt = DateTime.UtcNow }
        };

        _paymentRepositoryMock
            .Setup(r => r.GetByClientIdAsync(clientId))
            .ReturnsAsync(payments);

        // Act
        var result = await _paymentService.GetByClientIdAsync(clientId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count());
        _paymentRepositoryMock.Verify(r => r.GetByClientIdAsync(clientId), Times.Once);
    }
}
