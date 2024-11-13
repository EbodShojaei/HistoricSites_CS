using System.Security.Claims;
using ExoticHistoricSites.Server.Repositories;
using ExoticHistoricSites.Shared.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ExoticHistoricSites.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class HistoricSitesController : ControllerBase
{
    private readonly IHistoricSiteRepository _repository;

    public HistoricSitesController(IHistoricSiteRepository repository)
    {
        _repository = repository;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<HistoricSite>>> GetAll()
    {
        var sites = await _repository.GetAllAsync();
        return Ok(sites);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<HistoricSite>> GetById(int id)
    {
        var site = await _repository.GetByIdAsync(id);
        if (site == null)
            return NotFound();
        return Ok(site);
    }

    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<ActionResult<HistoricSite>> Create(HistoricSite site)
    {
        var createdSite = await _repository.CreateAsync(site);
        return CreatedAtAction(nameof(GetById), new { id = createdSite.Id }, createdSite);
    }

    [Authorize(Roles = "Admin")]
    [HttpPut("{id}")]
    public async Task<ActionResult<HistoricSite>> Update(int id, HistoricSite site)
    {
        var updatedSite = await _repository.UpdateAsync(id, site);
        if (updatedSite == null)
            return NotFound();
        return Ok(updatedSite);
    }

    [Authorize(Roles = "Admin")]
    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(int id)
    {
        var result = await _repository.DeleteAsync(id);
        if (!result)
            return NotFound();
        return NoContent();
    }

    [Authorize]
    [HttpPost("{siteId}/ratings")]
    public async Task<ActionResult> AddRating(int siteId, [FromBody] int rating)
    {
        var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value!);
        var result = await _repository.AddRatingAsync(siteId, userId, rating);
        if (!result)
            return NotFound();
        return Ok();
    }

    [Authorize(Roles = "Admin")]
    [HttpPost("{siteId}/images")]
    public async Task<ActionResult> AddImage(int siteId, [FromBody] string imageBase64)
    {
        var result = await _repository.AddImageAsync(siteId, imageBase64);
        if (!result)
            return NotFound();
        return Ok();
    }

    [Authorize(Roles = "Admin")]
    [HttpDelete("{siteId}/images/{imageId}")]
    public async Task<ActionResult> RemoveImage(int siteId, int imageId)
    {
        var result = await _repository.RemoveImageAsync(siteId, imageId);
        if (!result)
            return NotFound();
        return NoContent();
    }
}
