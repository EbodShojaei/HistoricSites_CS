using System.Text.Json;
using ExoticHistoricSites.API.Data;
using ExoticHistoricSites.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;

namespace ExoticHistoricSites.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
    public class HistoricSitesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<HistoricSitesController> _logger;

        public HistoricSitesController(
            ApplicationDbContext context,
            ILogger<HistoricSitesController> logger
        )
        {
            _context = context;
            _logger = logger;
        }

        // GET: api/HistoricSites
        [HttpGet]
        public async Task<ActionResult<IEnumerable<HistoricSite>>> GetHistoricSites()
        {
            try
            {
                var sites = await _context.HistoricSites.ToListAsync();
                _logger.LogInformation($"Retrieved {sites.Count} historic sites");
                return Ok(
                    new
                    {
                        success = true,
                        count = sites.Count,
                        data = sites,
                    }
                );
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error retrieving historic sites: {ex.Message}");
                return StatusCode(
                    500,
                    new { success = false, message = "Error retrieving historic sites" }
                );
            }
        }

        // GET: api/HistoricSites/5
        [HttpGet("{id:int}")]
        public async Task<ActionResult<HistoricSite>> GetById(int id)
        {
            try
            {
                _logger.LogInformation($"Attempting to retrieve historic site with ID: {id}");

                var site = await _context
                    .HistoricSites.AsNoTracking()
                    .SingleOrDefaultAsync(s => s.Id == id);

                if (site == null)
                {
                    _logger.LogWarning($"Historic site with ID {id} not found");
                    return NotFound(
                        new { success = false, message = $"Historic site with ID {id} not found" }
                    );
                }

                return Ok(new { success = true, data = site });
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error retrieving historic site {id}: {ex.Message}");
                return StatusCode(
                    500,
                    new
                    {
                        success = false,
                        message = $"Error retrieving historic site with ID {id}",
                    }
                );
            }
        }

        // GET: api/HistoricSites/search
        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<HistoricSite>>> SearchHistoricSites(
            [FromQuery] string? country = null,
            [FromQuery] string? name = null
        )
        {
            try
            {
                var query = _context.HistoricSites.AsQueryable();

                if (!string.IsNullOrWhiteSpace(country))
                {
                    query = query.Where(s => s.Countries.ToLower().Contains(country.ToLower()));
                }

                if (!string.IsNullOrWhiteSpace(name))
                {
                    query = query.Where(s => s.Name.ToLower().Contains(name.ToLower()));
                }

                var sites = await query.ToListAsync();
                return Ok(
                    new
                    {
                        success = true,
                        count = sites.Count,
                        data = sites,
                    }
                );
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error searching historic sites: {ex.Message}");
                return StatusCode(
                    500,
                    new { success = false, message = "Error searching historic sites" }
                );
            }
        }

        // POST: api/HistoricSites/upload-image/{id}
        [HttpPost("upload-image/{id:int}")]
        public async Task<IActionResult> UploadImage(int id, IFormFile image)
        {
            try
            {
                var site = await _context.HistoricSites.FindAsync(id);
                if (site == null)
                {
                    return NotFound(
                        new { success = false, message = $"Historic site with ID {id} not found" }
                    );
                }

                if (image == null || image.Length == 0)
                {
                    return BadRequest(new { success = false, message = "No image file provided" });
                }

                using var memoryStream = new MemoryStream();
                await image.CopyToAsync(memoryStream);
                memoryStream.Position = 0;

                // Resize image to 300px width
                using var imageSharp = await Image.LoadAsync(memoryStream);
                var aspectRatio = (float)imageSharp.Height / imageSharp.Width;
                var newWidth = 300;
                var newHeight = (int)(newWidth * aspectRatio);

                imageSharp.Mutate(x => x.Resize(newWidth, newHeight));

                using var outputStream = new MemoryStream();
                await imageSharp.SaveAsJpegAsync(outputStream);
                var base64String = Convert.ToBase64String(outputStream.ToArray());

                site.ImageBase64 = $"data:image/jpeg;base64,{base64String}";
                await _context.SaveChangesAsync();

                return Ok(
                    new
                    {
                        success = true,
                        message = "Image uploaded successfully",
                        data = new { id = site.Id, imageBase64 = site.ImageBase64 },
                    }
                );
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error uploading image: {ex.Message}");
                return StatusCode(500, new { success = false, message = "Error uploading image" });
            }
        }

        // POST: api/HistoricSites
        [HttpPost]
        public async Task<ActionResult<HistoricSite>> CreateHistoricSite(HistoricSite historicSite)
        {
            try
            {
                _context.HistoricSites.Add(historicSite);
                await _context.SaveChangesAsync();

                return CreatedAtAction(
                    nameof(GetById),
                    new { id = historicSite.Id },
                    new { success = true, data = historicSite }
                );
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error creating historic site: {ex.Message}");
                return StatusCode(
                    500,
                    new { success = false, message = "Error creating historic site" }
                );
            }
        }

        // PUT: api/HistoricSites/5
        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateHistoricSite(int id, HistoricSite historicSite)
        {
            try
            {
                if (id != historicSite.Id)
                {
                    return BadRequest(new { success = false, message = "ID mismatch" });
                }

                var existingSite = await _context.HistoricSites.FindAsync(id);
                if (existingSite == null)
                {
                    return NotFound(
                        new { success = false, message = $"Historic site with ID {id} not found" }
                    );
                }

                _context.Entry(existingSite).CurrentValues.SetValues(historicSite);
                await _context.SaveChangesAsync();

                return Ok(new { success = true, data = historicSite });
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error updating historic site {id}: {ex.Message}");
                return StatusCode(
                    500,
                    new { success = false, message = $"Error updating historic site with ID {id}" }
                );
            }
        }

        // DELETE: api/HistoricSites/5
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteHistoricSite(int id)
        {
            try
            {
                var historicSite = await _context.HistoricSites.FindAsync(id);
                if (historicSite == null)
                {
                    return NotFound(
                        new { success = false, message = $"Historic site with ID {id} not found" }
                    );
                }

                _context.HistoricSites.Remove(historicSite);
                await _context.SaveChangesAsync();

                return Ok(
                    new
                    {
                        success = true,
                        message = $"Historic site with ID {id} deleted successfully",
                    }
                );
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error deleting historic site {id}: {ex.Message}");
                return StatusCode(
                    500,
                    new { success = false, message = $"Error deleting historic site with ID {id}" }
                );
            }
        }

        private bool HistoricSiteExists(int id)
        {
            return _context.HistoricSites.Any(e => e.Id == id);
        }
    }
}
