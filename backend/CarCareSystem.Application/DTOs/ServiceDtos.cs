namespace CarCareSystem.Application.DTOs;

public class CreateServiceDto
{
    public Guid ClientId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public ServiceCategoryDto Category { get; set; }
    public decimal Price { get; set; }
    public int DurationMinutes { get; set; }
}

public class UpdateServiceDto
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public ServiceCategoryDto Category { get; set; }
    public decimal Price { get; set; }
    public int DurationMinutes { get; set; }
}

public class ServiceDto
{
    public Guid Id { get; set; }
    public Guid ClientId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public ServiceCategoryDto Category { get; set; }
    public decimal Price { get; set; }
    public int DurationMinutes { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public bool IsActive { get; set; }
}

public enum ServiceCategoryDto
{
    Aesthetics = 1,
    Audio = 2,
    Accessories = 3,
    Film = 4
}
