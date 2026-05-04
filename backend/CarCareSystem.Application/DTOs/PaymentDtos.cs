namespace CarCareSystem.Application.DTOs;

public class CreatePaymentDto
{
    public Guid ClientId { get; set; }
    public Guid? AppointmentId { get; set; }
    public decimal Amount { get; set; }
    public PaymentMethodDto Method { get; set; }
    public string? Reference { get; set; }
    public string? Notes { get; set; }
}

public class UpdatePaymentDto
{
    public PaymentStatusDto Status { get; set; }
    public string? Notes { get; set; }
}

public class PaymentDto
{
    public Guid Id { get; set; }
    public Guid ClientId { get; set; }
    public Guid? AppointmentId { get; set; }
    public decimal Amount { get; set; }
    public PaymentMethodDto Method { get; set; }
    public PaymentStatusDto Status { get; set; }
    public DateTime PaymentDate { get; set; }
    public string? Reference { get; set; }
    public string? Notes { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}

public enum PaymentMethodDto
{
    Cash = 1,
    CreditCard = 2,
    DebitCard = 3,
    BankTransfer = 4,
    Pix = 5
}

public enum PaymentStatusDto
{
    Pending = 1,
    Completed = 2,
    Failed = 3,
    Refunded = 4
}
