namespace CarCareSystem.Application.DTOs;

public class CreateAppointmentDto
{
    public Guid ClientId { get; set; }
    public DateTime ScheduledDate { get; set; }
    public string? Notes { get; set; }
    public List<AppointmentServiceDto> Services { get; set; } = new();
}

public class UpdateAppointmentDto
{
    public DateTime ScheduledDate { get; set; }
    public AppointmentStatusDto Status { get; set; }
    public string? Notes { get; set; }
}

public class AppointmentDto
{
    public Guid Id { get; set; }
    public Guid ClientId { get; set; }
    public DateTime ScheduledDate { get; set; }
    public DateTime? CompletedDate { get; set; }
    public AppointmentStatusDto Status { get; set; }
    public string? Notes { get; set; }
    public decimal TotalPrice { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public List<AppointmentServiceDto> Services { get; set; } = new();
}

public class AppointmentServiceDto
{
    public Guid ServiceId { get; set; }
    public string ServiceName { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public int Quantity { get; set; }
}

public enum AppointmentStatusDto
{
    Scheduled = 1,
    InProgress = 2,
    Completed = 3,
    Cancelled = 4
}
