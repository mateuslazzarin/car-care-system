namespace CarCareSystem.Api.Controllers;

using Microsoft.AspNetCore.Mvc;
using Application.DTOs;
using Application.Interfaces;

[ApiController]
[Route("api/[controller}")]
public class ServicesController : ControllerBase
{
    private readonly IServiceService _serviceService;
    private readonly ILogger<ServicesController> _logger;

    public ServicesController(IServiceService serviceService, ILogger<ServicesController> logger)
    {
        _serviceService = serviceService ?? throw new ArgumentNullException(nameof(serviceService));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>
    /// Obtém todos os serviços
    /// </summary>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<ServiceDto>>> GetAll()
    {
        try
        {
            var services = await _serviceService.GetAllAsync();
            return Ok(services);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting all services");
            return StatusCode(StatusCodes.Status500InternalServerError, new { message = "An error occurred while fetching services" });
        }
    }

    /// <summary>
    /// Obtém um serviço por ID
    /// </summary>
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ServiceDto>> GetById(Guid id)
    {
        try
        {
            var service = await _serviceService.GetByIdAsync(id);
            if (service == null)
                return NotFound(new { message = "Service not found" });

            return Ok(service);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting service with id {ServiceId}", id);
            return StatusCode(StatusCodes.Status500InternalServerError, new { message = "An error occurred while fetching the service" });
        }
    }

    /// <summary>
    /// Obtém serviços de um cliente
    /// </summary>
    [HttpGet("client/{clientId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<ServiceDto>>> GetByClientId(Guid clientId)
    {
        try
        {
            var services = await _serviceService.GetByClientIdAsync(clientId);
            return Ok(services);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting services for client {ClientId}", clientId);
            return StatusCode(StatusCodes.Status500InternalServerError, new { message = "An error occurred while fetching services" });
        }
    }

    /// <summary>
    /// Obtém serviços por categoria
    /// </summary>
    [HttpGet("category/{category}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<ServiceDto>>> GetByCategory(ServiceCategoryDto category)
    {
        try
        {
            var services = await _serviceService.GetByCategoryAsync(category);
            return Ok(services);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting services by category {Category}", category);
            return StatusCode(StatusCodes.Status500InternalServerError, new { message = "An error occurred while fetching services" });
        }
    }

    /// <summary>
    /// Obtém serviços ativos
    /// </summary>
    [HttpGet("active")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<ServiceDto>>> GetActiveServices()
    {
        try
        {
            var services = await _serviceService.GetActiveServicesAsync();
            return Ok(services);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting active services");
            return StatusCode(StatusCodes.Status500InternalServerError, new { message = "An error occurred while fetching active services" });
        }
    }

    /// <summary>
    /// Cria um novo serviço
    /// </summary>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ServiceDto>> Create([FromBody] CreateServiceDto dto)
    {
        try
        {
            var service = await _serviceService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = service.Id }, service);
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
            _logger.LogError(ex, "Error creating service");
            return StatusCode(StatusCodes.Status500InternalServerError, new { message = "An error occurred while creating the service" });
        }
    }

    /// <summary>
    /// Atualiza um serviço
    /// </summary>
    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ServiceDto>> Update(Guid id, [FromBody] UpdateServiceDto dto)
    {
        try
        {
            var service = await _serviceService.UpdateAsync(id, dto);
            return Ok(service);
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
            _logger.LogError(ex, "Error updating service with id {ServiceId}", id);
            return StatusCode(StatusCodes.Status500InternalServerError, new { message = "An error occurred while updating the service" });
        }
    }

    /// <summary>
    /// Deleta um serviço
    /// </summary>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(Guid id)
    {
        try
        {
            await _serviceService.DeleteAsync(id);
            return NoContent();
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting service with id {ServiceId}", id);
            return StatusCode(StatusCodes.Status500InternalServerError, new { message = "An error occurred while deleting the service" });
        }
    }
}
