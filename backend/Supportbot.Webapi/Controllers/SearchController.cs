using Bogus;
using Microsoft.AspNetCore.Mvc;
using Supportbot.Application.Dtos;
using Supportbot.Application.Infrastructure;
using System.Collections.Generic;
using System.Linq;

namespace Supportbot.Webapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SearchController : ControllerBase
    {
        private readonly SupportbotContext _db;

        public SearchController(SupportbotContext db)
        {
            _db = db;
        }

        public ActionResult<List<SearchResultDto>> Search([FromQuery] string query)
        {
            //Randomizer.Seed = new Random(1658);
            var faker = new Faker("de");

            var results = new Faker<SearchResultDto>("de")
                .CustomInstantiator(f => new SearchResultDto(f.Lorem.Word(), f.Lorem.Paragraph()))
                .Generate(faker.Random.Int(3, 6));
            return Ok(results.ToList());
        }
    }
}
