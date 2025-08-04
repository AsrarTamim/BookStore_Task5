using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Bookstore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksApiController : ControllerBase
    {
        private readonly BookGenerator _generator;

        public BooksApiController()
        {
            _generator = new BookGenerator();
        }

        [HttpGet]
        public IActionResult Get([FromQuery] int seed, [FromQuery] int page, [FromQuery] string locale = "en", 
            [FromQuery] double likes = 0, [FromQuery] double reviews = 0)
        {
            var books = _generator.GenerateBooks(seed, page, locale, likes, reviews);
            return Ok(books);
        }
    }
}
