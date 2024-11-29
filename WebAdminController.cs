using Microsoft.AspNetCore.Mvc;
using ShortTermStayAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("1.0")]
[Authorize(Roles="Admin")]
public class WebAdminController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public WebAdminController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet("ReportListingsWithRatings")]
    public async Task<ActionResult> ReportListingsWithRatings(
        [FromQuery] string? country, 
        [FromQuery] string? city, 
        [FromQuery] double? minRating) {
        var query = _context.Listings.AsQueryable(); 
    

        if (!string.IsNullOrEmpty(country))
        {
        query = query.Where(l => l.Country == country);
        }
    

        if (!string.IsNullOrEmpty(city))
        {
           query = query.Where(l => l.City == city);
        }
    

        if (minRating.HasValue)
        {
            query = query.Where(l => l.Rating >= minRating.Value);
        }


        var listings = await query.ToListAsync();

        return Ok(new { Listings = listings });
    }
    
}
