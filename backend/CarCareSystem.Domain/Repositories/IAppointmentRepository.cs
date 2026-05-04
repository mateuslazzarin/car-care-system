namespace CarCareSystem.Domain.Repositories;

using Entities;

public interface IAppointmentRepository : IRepository<Appointment>
{
    Task<IEnumerable<Appointment>> GetByClientIdAsync(Guid clientId);
    Task<IEnumerable<Appointment>> GetByStatusAsync(AppointmentStatus status);
    Task<IEnumerable<Appointment>> GetByDateRangeAsync(DateTime startDate, DateTime endDate);
}
