namespace strategy.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using strategy.Models;
    using strategy.Services;

    [ApiController]
    [Route("api/[controller]")]
    public sealed class RecommendationsController(RecommendationService service) : ControllerBase
    {
        [HttpGet]
        public IActionResult Get([FromQuery] string? type, [FromQuery] int limit = 5)
        {
            // Demo user 
            var user = new UserContext(
                UserId: Guid.NewGuid(),
                Interests: ["Clothing"]
            );
            try
            {
                var items = service.Recommend(type, limit, user);

                return Ok(new
                {
                    type = string.IsNullOrWhiteSpace(type) ? "popular" : type,
                    availableTypes = service.AvailableTypes,
                    items
                });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new
                {
                    error = ex.Message
                });
            }
        }
    }
}