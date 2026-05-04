namespace CarCareSystem.Application.Interfaces;

using DTOs;

public interface IAppointmentService
{
    Task<AppointmentDto?> GetByIdAsync(Guid id);
    Task<IEnumerable<AppointmentDto>> GetAllAsync();
    Task<IEnumerable<AppointmentDto>> GetByClientIdAsync(Guid clientId);
    Task<IEnumerable<AppointmentDto>> GetByStatusAsync(AppointmentStatusDto status);
    Task<IEnumerable<AppointmentDto>> GetByDateRangeAsync(DateTime startDate, DateTime endDate);
    Task<AppointmentDto> CreateAsync(CreateAppointmentDto dto);
    Task<AppointmentDto> UpdateAsync(Guid id, UpdateAppointmentDto dto);
    Task DeleteAsync(Guid id);
}
