namespace CarCareSystem.Api.Controllers;

using Microsoft.AspNetCore.Mvc;
using Application.DTOs;
using Application.Interfaces;

[ApiController]
[Route("api/[controller]")]
public class AppointmentsController : ControllerBase
{
    private readonly IAppointmentService _appointmentService;
    private readonly ILogger<AppointmentsController> _logger;

    public AppointmentsController(IAppointmentService appointmentService, ILogger<AppointmentsController> logger)
    {
        _appointmentService = appointmentService ?? throw new ArgumentNullException(nameof(appointmentService));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>
    /// Obtém todos os agendamentos
    /// </summary>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<AppointmentDto>>> GetAll()
    {
        try
        {
            var appointments = await _appointmentService.GetAllAsync();
            return Ok(appointments);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting all appointments");
            return StatusCode(StatusCodes.Status500InternalServerError, new { message = "An error occurred while fetching appointments" });
        }
    }

    /// <summary>
    /// Obtém um agendamento por ID
    /// </summary>
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<AppointmentDto>> GetById(Guid id)
    {
        try
        {
            var appointment = await _appointmentService.GetByIdAsync(id);
            if (appointment == null)
                return NotFound(new { message = "Appointment not found" });

            return Ok(appointment);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting appointment with id {AppointmentId}", id);
            return StatusCode(StatusCodes.Status500InternalServerError, new { message = "An error occurred while fetching the appointment" });
        }
    }

    /// <summary>
    /// Obtém agendamentos por cliente
    /// </summary>
    [HttpGet("client/{clientId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<AppointmentDto>>> GetByClientId(Guid clientId)
    {
        try
        {
            var appointments = await _appointmentService.GetByClientIdAsync(clientId);
            return Ok(appointments);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting appointments for client {ClientId}", clientId);
            return StatusCode(StatusCodes.Status500InternalServerError, new { message = "An error occurred while fetching appointments" });
        }
    }

    /// <summary>
    /// Obtém agendamentos por status
    /// </summary>
    [HttpGet("status/{status}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<AppointmentDto>>> GetByStatus(AppointmentStatusDto status)
    {
        try
        {
            var appointments = await _appointmentService.GetByStatusAsync(status);
            return Ok(appointments);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting appointments by status {Status}", status);
            return StatusCode(StatusCodes.Status500InternalServerError, new { message = "An error occurred while fetching appointments" });
        }
    }

    /// <summary>
    /// Obtém agendamentos por intervalo de datas
    /// </summary>
    [HttpGet("range")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<AppointmentDto>>> GetByDateRange([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
    {
        try
        {
            var appointments = await _appointmentService.GetByDateRangeAsync(startDate, endDate);
            return Ok(appointments);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting appointments by date range");
            return StatusCode(StatusCodes.Status500InternalServerError, new { message = "An error occurred while fetching appointments" });
        }
    }

    /// <summary>
    /// Cria um novo agendamento
    /// </summary>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<AppointmentDto>> Create([FromBody] CreateAppointmentDto dto)
    {
        try
        {
            var appointment = await _appointmentService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = appointment.Id }, appointment);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating appointment");
            return StatusCode(StatusCodes.Status500InternalServerError, new { message = "An error occurred while creating the appointment" });
        }
    }

    /// <summary>
    /// Atualiza um agendamento
    /// </summary>
    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<AppointmentDto>> Update(Guid id, [FromBody] UpdateAppointmentDto dto)
    {
        try
        {
            var appointment = await _appointmentService.UpdateAsync(id, dto);
            return Ok(appointment);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating appointment with id {AppointmentId}", id);
            return StatusCode(StatusCodes.Status500InternalServerError, new { message = "An error occurred while updating the appointment" });
        }
    }

    /// <summary>
    /// Deleta um agendamento
    /// </summary>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(Guid id)
    {
        try
        {
            await _appointmentService.DeleteAsync(id);
            return NoContent();
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting appointment with id {AppointmentId}", id);
            return StatusCode(StatusCodes.Status500InternalServerError, new { message = "An error occurred while deleting the appointment" });
        }
    }
}
