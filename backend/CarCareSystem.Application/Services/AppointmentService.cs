namespace CarCareSystem.Application.Services;

using Domain.Entities;
using Domain.Repositories;
using DTOs;
using Interfaces;

public class AppointmentService : IAppointmentService
{
    private readonly IAppointmentRepository _appointmentRepository;
    private readonly IClientRepository _clientRepository;

    public AppointmentService(IAppointmentRepository appointmentRepository, IClientRepository clientRepository)
    {
        _appointmentRepository = appointmentRepository ?? throw new ArgumentNullException(nameof(appointmentRepository));
        _clientRepository = clientRepository ?? throw new ArgumentNullException(nameof(clientRepository));
    }

    public async Task<AppointmentDto?> GetByIdAsync(Guid id)
    {
        if (id == Guid.Empty)
            throw new ArgumentException("Appointment ID cannot be empty", nameof(id));

        var appointment = await _appointmentRepository.GetByIdAsync(id);
        return appointment != null ? MapToDto(appointment) : null;
    }

    public async Task<IEnumerable<AppointmentDto>> GetAllAsync()
    {
        var appointments = await _appointmentRepository.GetAllAsync();
        return appointments.Select(MapToDto);
    }

    public async Task<IEnumerable<AppointmentDto>> GetByClientIdAsync(Guid clientId)
    {
        if (clientId == Guid.Empty)
            throw new ArgumentException("Client ID cannot be empty", nameof(clientId));

        var appointments = await _appointmentRepository.GetByClientIdAsync(clientId);
        return appointments.Select(MapToDto);
    }

    public async Task<IEnumerable<AppointmentDto>> GetByStatusAsync(AppointmentStatusDto status)
    {
        var domainStatus = (AppointmentStatus)(int)status;
        var appointments = await _appointmentRepository.GetByStatusAsync(domainStatus);
        return appointments.Select(MapToDto);
    }

    public async Task<IEnumerable<AppointmentDto>> GetByDateRangeAsync(DateTime startDate, DateTime endDate)
    {
        if (startDate > endDate)
            throw new ArgumentException("Start date cannot be greater than end date");

        var appointments = await _appointmentRepository.GetByDateRangeAsync(startDate, endDate);
        return appointments.Select(MapToDto);
    }

    public async Task<AppointmentDto> CreateAsync(CreateAppointmentDto dto)
    {
        ValidateCreateAppointmentDto(dto);

        var client = await _clientRepository.GetByIdAsync(dto.ClientId);
        if (client == null)
            throw new KeyNotFoundException($"Client with ID '{dto.ClientId}' not found.");

        if (dto.ScheduledDate <= DateTime.UtcNow)
            throw new ArgumentException("Scheduled date must be in the future.");

        var appointment = new Appointment
        {
            Id = Guid.NewGuid(),
            ClientId = dto.ClientId,
            ScheduledDate = dto.ScheduledDate,
            Status = AppointmentStatus.Scheduled,
            Notes = dto.Notes,
            TotalPrice = 0,
            CreatedAt = DateTime.UtcNow
        };

        await _appointmentRepository.AddAsync(appointment);
        await _appointmentRepository.SaveChangesAsync();

        return MapToDto(appointment);
    }

    public async Task<AppointmentDto> UpdateAsync(Guid id, UpdateAppointmentDto dto)
    {
        if (id == Guid.Empty)
            throw new ArgumentException("Appointment ID cannot be empty", nameof(id));

        var appointment = await _appointmentRepository.GetByIdAsync(id);
        if (appointment == null)
            throw new KeyNotFoundException($"Appointment with ID '{id}' not found.");

        appointment.ScheduledDate = dto.ScheduledDate;
        appointment.Status = (AppointmentStatus)(int)dto.Status;
        appointment.Notes = dto.Notes;
        appointment.UpdatedAt = DateTime.UtcNow;

        if (dto.Status == AppointmentStatusDto.Completed)
            appointment.CompletedDate = DateTime.UtcNow;

        _appointmentRepository.Update(appointment);
        await _appointmentRepository.SaveChangesAsync();

        return MapToDto(appointment);
    }

    public async Task DeleteAsync(Guid id)
    {
        if (id == Guid.Empty)
            throw new ArgumentException("Appointment ID cannot be empty", nameof(id));

        var appointment = await _appointmentRepository.GetByIdAsync(id);
        if (appointment == null)
            throw new KeyNotFoundException($"Appointment with ID '{id}' not found.");

        _appointmentRepository.Delete(appointment);
        await _appointmentRepository.SaveChangesAsync();
    }

    private void ValidateCreateAppointmentDto(CreateAppointmentDto dto)
    {
        if (dto.ClientId == Guid.Empty)
            throw new ArgumentException("Client ID is required.", nameof(dto.ClientId));

        if (dto.ScheduledDate == default)
            throw new ArgumentException("Scheduled date is required.", nameof(dto.ScheduledDate));
    }

    private static AppointmentDto MapToDto(Appointment appointment)
    {
        return new AppointmentDto
        {
            Id = appointment.Id,
            ClientId = appointment.ClientId,
            ScheduledDate = appointment.ScheduledDate,
            CompletedDate = appointment.CompletedDate,
            Status = (AppointmentStatusDto)(int)appointment.Status,
            Notes = appointment.Notes,
            TotalPrice = appointment.TotalPrice,
            CreatedAt = appointment.CreatedAt,
            UpdatedAt = appointment.UpdatedAt
        };
    }
}
