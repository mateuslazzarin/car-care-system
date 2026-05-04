namespace CarCareSystem.Infrastructure.Repositories;

using Microsoft.EntityFrameworkCore;
using Domain.Entities;
using Domain.Repositories;
using Persistence;

public class AppointmentRepository : IAppointmentRepository
{
    private readonly CarCareSystemDbContext _context;

    public AppointmentRepository(CarCareSystemDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task<Appointment?> GetByIdAsync(Guid id)
    {
        return await _context.Appointments
            .Include(a => a.AppointmentServices)
            .FirstOrDefaultAsync(a => a.Id == id);
    }

    public async Task<IEnumerable<Appointment>> GetAllAsync()
    {
        return await _context.Appointments
            .Include(a => a.AppointmentServices)
            .ToListAsync();
    }

    public async Task<IEnumerable<Appointment>> GetByClientIdAsync(Guid clientId)
    {
        return await _context.Appointments
            .Where(a => a.ClientId == clientId)
            .Include(a => a.AppointmentServices)
            .ToListAsync();
    }

    public async Task<IEnumerable<Appointment>> GetByStatusAsync(AppointmentStatus status)
    {
        return await _context.Appointments
            .Where(a => a.Status == status)
            .Include(a => a.AppointmentServices)
            .ToListAsync();
    }

    public async Task<IEnumerable<Appointment>> GetByDateRangeAsync(DateTime startDate, DateTime endDate)
    {
        return await _context.Appointments
            .Where(a => a.ScheduledDate >= startDate && a.ScheduledDate <= endDate)
            .Include(a => a.AppointmentServices)
            .ToListAsync();
    }

    public async Task AddAsync(Appointment entity)
    {
        await _context.Appointments.AddAsync(entity);
    }

    public void Update(Appointment entity)
    {
        _context.Appointments.Update(entity);
    }

    public void Delete(Appointment entity)
    {
        _context.Appointments.Remove(entity);
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}
