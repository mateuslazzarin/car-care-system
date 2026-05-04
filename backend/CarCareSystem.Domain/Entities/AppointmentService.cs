namespace CarCareSystem.Domain.Entities;

public class AppointmentService
{
    public Guid Id { get; set; }
    public Guid AppointmentId { get; set; }
    public Guid ServiceId { get; set; }
    public decimal Price { get; set; }
    public int Quantity { get; set; }
    public DateTime CreatedAt { get; set; }

    // Navigation properties
    public Appointment Appointment { get; set; } = null!;
    public Service Service { get; set; } = null!;
}
