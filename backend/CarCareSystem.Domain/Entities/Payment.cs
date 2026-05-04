namespace CarCareSystem.Domain.Entities;

public class Payment
{
    public Guid Id { get; set; }
    public Guid ClientId { get; set; }
    public Guid? AppointmentId { get; set; }
    public decimal Amount { get; set; }
    public PaymentMethod Method { get; set; }
    public PaymentStatus Status { get; set; }
    public DateTime PaymentDate { get; set; }
    public string? Reference { get; set; }
    public string? Notes { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }

    // Navigation properties
    public Client Client { get; set; } = null!;
    public Appointment? Appointment { get; set; }
}

public enum PaymentMethod
{
    Cash = 1,
    CreditCard = 2,
    DebitCard = 3,
    BankTransfer = 4,
    Pix = 5
}

public enum PaymentStatus
{
    Pending = 1,
    Completed = 2,
    Failed = 3,
    Refunded = 4
}
