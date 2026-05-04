namespace CarCareSystem.Domain.Entities;

public class Service
{
    public Guid Id { get; set; }
    public Guid ClientId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public ServiceCategory Category { get; set; }
    public decimal Price { get; set; }
    public int DurationMinutes { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public bool IsActive { get; set; } = true;

    // Navigation properties
    public Client Client { get; set; } = null!;
    public ICollection<AppointmentService> AppointmentServices { get; set; } = new List<AppointmentService>();
}

public enum ServiceCategory
{
    Aesthetics = 1,        // Estética (polimento, enceramento, etc)
    Audio = 2,             // Som
    Accessories = 3,       // Acessórios
    Film = 4               // Películas
}
