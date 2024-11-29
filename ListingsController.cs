using Microsoft.AspNetCore.Mvc;
using ShortTermStayAPI.Models;
using Microsoft.AspNetCore.Authorization;

[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("1.0")]
public class ListingsController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public ListingsController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpPost("InsertListing")]
    [Authorize(Roles = "Admin, Host")]
    public async Task<ActionResult> AddListing([FromBody] Listing request)
    {
        if (ModelState.IsValid)
        {   
            var listing = new Listing {
            Title = request.Title,
            Description = request.Description,
            NoOfPeople = request.NoOfPeople,
            Country = request.Country,
            City = request.City,
            Price = request.Price
            };

            _context.Listings.Add(listing);
            await _context.SaveChangesAsync();
            return Ok(new { Status = "Successful", Message = "Listing added successfully" });
        }
        return BadRequest(new { Status = "Error", Message = "Invalid data" });
    }
}

[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("2.0")]
public class Listings2Controller : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public Listings2Controller(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpPost("InsertListing")]
    [Authorize(Roles = "Admin, Host")]
    public async Task<ActionResult> AddListing([FromBody] ListingV2 request)
    {
        if (ModelState.IsValid)
        {   
            var listing = new ListingV2 {
            Title = request.Title,
            Description = request.Description,
            NoOfPeople = request.NoOfPeople,
            Country = request.Country,
            City = request.City,
            Address = request.Address,
            Price = request.Price
            };

            _context.ListingsV2.Add(listing);
            await _context.SaveChangesAsync();
            return Ok(new { Status = "Successful", Message = "Listing added successfully" });
        }
        return BadRequest(new { Status = "Error", Message = "Invalid data" });
    }
}