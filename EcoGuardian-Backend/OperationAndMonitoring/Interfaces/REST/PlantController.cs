
using EcoGuardian_Backend.OperationAndMonitoring.Domain.Model.Queries;
using EcoGuardian_Backend.OperationAndMonitoring.Domain.Services;
using EcoGuardian_Backend.OperationAndMonitoring.Interfaces.REST.Resources;
using EcoGuardian_Backend.OperationAndMonitoring.Interfaces.REST.Transform;
using EcoGuardian_Backend.Shared.Interfaces.Filters;
using Microsoft.AspNetCore.Mvc;

namespace EcoGuardian_Backend.OperationAndMonitoring.Interfaces.REST;


[ApiController]
[ProducesResponseType(500)]
[Route("api/v1/plants")]

public class PlantController(IPlantCommandService plantCommandService, IPlantQueryService plantQueryService) : ControllerBase
{
    [HttpPost]
    [ProducesResponseType(201)]
    [AuthorizeFilter("Admin", "Domestic", "Business")]
    public async Task<IActionResult> CreatePlant([FromForm] CreatePlantResource resource)
    {
        var command = CreatePlantCommandFromResourceAssembler.ToCommandFromResource(resource);
        var plant = await plantCommandService.Handle(command);

        return StatusCode(201, new
        {
            message = "Planta creada exitosamente",
            id = plant.Id
        });
    }

    [HttpPut("{id:int}")]
    [ProducesResponseType(200)]
    [AuthorizeFilter("Admin", "Domestic", "Business")]
    public async Task<IActionResult> UpdatePlant([FromBody] UpdatePlantResource resource, [FromRoute] int id)
    {
        var command = UpdatePlantCommandFromResourceAssembler.ToCommandFromResource(id, resource);
        await plantCommandService.Handle(command);
        return Ok(true);
    }

    [HttpDelete("{id:int}")]
    [ProducesResponseType(200)]
    [AuthorizeFilter("Admin", "Domestic", "Business")]
    public async Task<IActionResult> DeletePlant([FromRoute] int id)
    {
        var command = DeletePlantCommandFromResourceAssembler.ToCommandFromResource(id);
        await plantCommandService.Handle(command);
        return Ok(true);
    }

    [HttpGet]
    [ProducesResponseType(200)]
    [AuthorizeFilter("Admin", "Domestic", "Business", "Specialist")]
    public async Task<IActionResult> GetPlantsByUserId([FromQuery] string userId)
    {
        var query = new GetPlantsByUserIdQuery(userId);
        var plants = await plantQueryService.Handle(query);
        var resources = plants.Select(PlantResourceFromEntityAssembler.ToResourceFromEntity).ToList();
        return Ok(resources);
    }
    [HttpGet("{id:int}")]
    [ProducesResponseType(200)]
    [ProducesResponseType(404)]
    [AuthorizeFilter("Admin", "Domestic", "Business", "Specialist")]
    public async Task<IActionResult> GetPlantById([FromRoute] int id)
    {
        var query = new GetPlantByIdQuery(id);
        var plant = await plantQueryService.Handle(query);
        if (plant == null)
        {
            return NotFound();
        }
        var resource = PlantResourceFromEntityAssembler.ToResourceFromEntity(plant);
        return Ok(resource);
    }
}