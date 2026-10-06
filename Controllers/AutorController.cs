using Microsoft.AspNetCore.Mvc;
using Microsoft.CSharp.RuntimeBinder;
using Luis_P1_P4.Services;
using Luis_P1_P4.Models;
using SQLitePCL;

namespace Luis_P1_P4.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AutorController : ControllerBase
{
    private readonly PersonaServices _personaServices;

    public AutorController(PersonaServices personaServices)
    {
        _personaServices = personaServices;
    }

    [HttpPost("{nombre},{nacionalidad},{FechaNacimiento},{Sueldo}")]
    public async Task<IActionResult> CreatePersona( [FromRoute] string nombre,
    [FromRoute] string nacionalidad,
    [FromRoute] string FechaNacimiento,
    [FromRoute] int Sueldo)
    {
        try
        {
            var record = new PersonaRecordSet(nombre, nacionalidad, FechaNacimiento, Sueldo);
            bool success = await _personaServices.SaveAsync(record);
            if (!success)
            {
                return StatusCode(500, new { message = "Error al guardar en la base de datos." });
            }

            return Ok(new { message = "Persona creada exitosamente." });
        }
        catch (Exception ex)
        {
            // Captura cualquier falla real (conexión BD, tabla inexistente, error de Dapper, etc.)
            return StatusCode(500, new { message = "Error interno del servidor.", detalle = ex.Message });
        }
    }

    [HttpPut("update/, {id},{nombre},{nacionalidad},{FechaNacimiento},{Sueldo}")]
    public async Task<IActionResult> UpdatePersona([FromRoute] int id,  [FromRoute] string nombre,
    [FromRoute] string nacionalidad,
    [FromRoute] string FechaNacimiento,
    [FromRoute] int Sueldo )
    {
        try
        {
            var record = new PersonaRecordSet(nombre, nacionalidad, FechaNacimiento, Sueldo);
            var actualizado = await _personaServices.UpdateAsync(id, record);

            if (actualizado == null)
            {
                return NotFound(new { message = $"No se encontró el registro con ID {id}." });
            }

            return Ok(actualizado);
        }
        catch (Exception ex)
        {
            // Captura cualquier falla real (conexión BD, tabla inexistente, error de Dapper, etc.)
            return StatusCode(500, new { message = "Error interno del servidor.", detalle = ex.Message });
        }
    }
    [HttpDelete("Delete,{id}")]
    public async Task<IActionResult> DeletePersona([FromRoute] int id)
    {
        try
        {
            var eliminado = await _personaServices.DeleteAsync(id);

            if (eliminado == null)
            {
                return NotFound(new { message = $"No se encontró el registro con ID {id}." });
            }

            return Ok(new { message = "Persona eliminada exitosamente.", persona = eliminado });
        }
        catch (Exception ex)
        {
            // Captura cualquier falla real (conexión BD, tabla inexistente, error de Dapper, etc.)
            return StatusCode(500, new { message = "Error interno del servidor.", detalle = ex.Message });
        }
    }

    [HttpGet("BuscarId/{id}")]
    public async Task<IActionResult> GetPersona([FromRoute] int id)
    {
        try
        {
            var persona = await _personaServices.GetByIdAsync(id);

            if (persona == null)
            {
                return NotFound(new { message = $"No se encontró el registro con ID {id}." });
            }

            return Ok(persona);
        }
        catch (Exception ex)
        {
            // Captura cualquier falla real (conexión BD, tabla inexistente, error de Dapper, etc.)
            return StatusCode(500, new { message = "Error interno del servidor.", detalle = ex.Message });
        }
    }

    [HttpGet("list/get")]
    public async Task<IActionResult> GetListAsync()
    {
        try
        {
            var personas = await _personaServices.GetListAsync();
            return Ok(personas);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Error interno del servidor.", detalle = ex.Message });
        }
    }

}