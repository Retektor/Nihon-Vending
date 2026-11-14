using Microsoft.AspNetCore.Mvc;
using NHibernate.Exceptions;
using NihonVending.Domain.DTOs;
using NihonVending.Domain.Entities;
using NihonVending.Services;
using Npgsql;

namespace NihonVending.Controllers;

[ApiController]
[Route("api/vending_machines")]
public class VendingMachineController(
    IVendingMachineService service) : ControllerBase
{
    [HttpGet]
    public ActionResult<IReadOnlyList<VendingMachineDto>> GetAll()
    {
        return Ok(service.GetAll());
    }

    [HttpGet("{id:int}")]
    public ActionResult<VendingMachineDto> GetById(int id)
    {
        var machine = service.GetById(id);
        if (machine is null)
        {
            return NotFound();
        }

        return Ok(machine);
    }

    [HttpPost]
    public ActionResult<VendingMachineDto> Create([FromBody] VendingMachineCreateRequest dto)
    {
        try
        {
            var created = service.Create(dto);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }
        catch (GenericADOException ex) when (
            ex.InnerException is PostgresException { SqlState: PostgresErrorCodes.CheckViolation } pgEx &&
            pgEx.ConstraintName == "chk_vending_machines_category")
        {
            var available = string.Join(", ", VendingMachineCategories.Allowed);
            return BadRequest(new
            {
                message = $"Unknown category type. Available categories: {available}"
            });
        }
    }

    [HttpPut("{id:int}")]
    public IActionResult Update(int id, [FromBody] VendingMachineUpdateRequest dto)
    {
        try
        {
            var updated = service.Update(dto);
            if (updated == null)
            {
                return NotFound();
            }
            return Ok(updated);
        }
        catch (GenericADOException ex) when (
            ex.InnerException is PostgresException { SqlState: PostgresErrorCodes.CheckViolation } pgEx &&
            pgEx.ConstraintName == "chk_vending_machines_category")
        {
            var available = string.Join(", ", VendingMachineCategories.Allowed);
            return BadRequest(new
            {
                message = $"Unknown category type. Available categories: {available}"
            });
        }
    }

    [HttpDelete("{id:int}")]
    public IActionResult Delete(int id)
    {
        service.Delete(id);

        return NoContent();
    }
}
