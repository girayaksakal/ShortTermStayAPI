using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShortTermStayAPI.Models;
using ShortTermStayAPI.Models.Queries;
using Microsoft.AspNetCore.Authorization;

[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("1.0")]

public class BookingsController: ControllerBase {
    private readonly ApplicationDbContext _context;

    public BookingsController(ApplicationDbContext context) {
        _context = context;
    }

    [HttpGet("QueryListings")]
    public async Task<ActionResult> QueryListings(
        [FromQuery] DateTime from, 
        [FromQuery] DateTime to, 
        [FromQuery] int noOfPeople, 
        [FromQuery] string? country, 
        [FromQuery] string? city,
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10) {
        var query = _context.Listings
            .Where(l => l.NoOfPeople >= noOfPeople &&
                        (l.Country == country || string.IsNullOrEmpty(country)) &&
                        (l.City == city || string.IsNullOrEmpty(city)) &&
                        !_context.Bookings.Any(b => b.ListingId == l.Id &&
                                                    ((from >= b.From && from <= b.To) ||
                                                    (to >= b.From && to <= b.To) || 
                                                    (from <= b.From && to >= b.To))));
        var totalRecords = await query.CountAsync();
        var listings = await query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
        
        var result = new {
            TotalRecords = totalRecords,
            pageNumber = pageNumber,
            pageSize = pageSize,
            Listings = listings
        };

        return Ok(result);
    }

    [HttpPost("BookAStay")]
    [Authorize(Roles = "Admin, Guest")]
    public async Task<ActionResult> BookAStay([FromBody] BookingRequest request) {

        var isAvailable = !await _context.Bookings.AnyAsync(b => b.ListingId == request.ListingId &&
                                                                ((request.From >= b.From && request.From <= b.To) ||
                                                                (request.To >= b.From && request.To <= b.To) || 
                                                                (request.From <= b.From && request.To >= b.To)));
        
        if (!isAvailable) {
            return BadRequest(new { Status = "Error", Message = "Listing is not available for the selected dates" });
        }

        if (User.Identity == null || !User.Identity.IsAuthenticated) {
            return Unauthorized(new { Status = "Error", Message = "User is not authenticated" });
        }

        var username = User.Identity.Name;

        if (string.IsNullOrEmpty(username)) {
            return Unauthorized(new { Status = "Error", Message = "User name is not available" });
        }

        var booking = new Booking {
            ListingId = request.ListingId,
            From = request.From,
            To = request.To,
            Username = username,
            GuestNames = request.GuestNames
        };

        _context.Bookings.Add(booking);
        await _context.SaveChangesAsync();

        return Ok(new { Status = "Successful", Message = "Booking successful" });
    }

    [HttpPost("ReviewAStay")]
    [Authorize]
    public async Task<ActionResult> ReviewAStay([FromBody] BookingReview request) {
        if (User.Identity == null || !User.Identity.IsAuthenticated) {
            return Unauthorized(new { Status = "Error", Message = "User is not authenticated" });
        }

        var username = User.Identity.Name;

        if (string.IsNullOrEmpty(username)) {
            return Unauthorized(new { Status = "Error", Message = "User name is not available" });
        }

        var booking = await _context.Bookings
            .Where(b => b.Username == username && b.Id == request.BookingId)
            .FirstOrDefaultAsync();

        if (booking == null) {
            return Unauthorized(new { Status = "Error", Message = "You can only review stays you have booked." });
        }

        var newReview = new BookingReview {
            BookingId = request.BookingId,
            Rating = request.Rating,
            Comment = request.Comment
        };

        _context.BookingReviews.Add(newReview);
        await _context.SaveChangesAsync();

        return Ok(new { Status = "Successful", Message = "Review added successfully" });
    }
    private async Task UpdateListingRating(int bookingId)
    {
        var listingId = await _context.Bookings
            .Where(b => b.Id == bookingId)
            .Select(b => b.ListingId)
            .FirstOrDefaultAsync();

        if (listingId == 0) return;

        var averageRating = await _context.BookingReviews
            .Where(br => br.BookingId == bookingId)
            .AverageAsync(br => br.Rating);

        var listing = await _context.Listings
            .FirstOrDefaultAsync(l => l.Id == listingId);

        if (listing != null)
        {
            listing.Rating = averageRating;

            _context.Listings.Update(listing);
            await _context.SaveChangesAsync();
        }
    }
}