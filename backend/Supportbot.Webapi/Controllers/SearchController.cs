using Bogus;
using Elastic.Clients.Elasticsearch;
using Elastic.Clients.Elasticsearch.QueryDsl;
using Microsoft.AspNetCore.Mvc;
using Supportbot.Application.Dtos;
using Supportbot.Application.Infrastructure;
using Supportbot.Application.Model;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Supportbot.Webapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SearchController : ControllerBase
    {
        private readonly SupportbotContext _db;
        private readonly ElasticsearchClient _client;

        public SearchController(SupportbotContext db, ElasticsearchClient client)
        {
            _db = db;
            _client = client;
        }

        [HttpGet("all")]
        public async Task<ActionResult<List<SearchResultDto>>> GetAllDocuments()
        {
            var searchRequest = new SearchRequestDescriptor<SupportDocument>()
                .Query(q => q.MatchAll());
            var found = await _client.SearchAsync(searchRequest);

            if (!found.IsValidResponse) return BadRequest();
            return Ok(found.Documents.Select(s => new SearchResultDto(s.Title, s.Content)));
        }

        public async Task<ActionResult<List<SearchResultDto>>> Search([FromQuery] string query)
        {
            var searchRequest = new SearchRequestDescriptor<SupportDocument>()
                .Query(q => q
                    .MatchPhrasePrefix(m => m
                        .Field(f => f.Content)
                        .Query(query)
                        .MaxExpansions(10)
                    ));
            var found = await _client.SearchAsync(searchRequest);

            if (!found.IsValidResponse) return BadRequest();
            return Ok(found.Documents.Select(s => new SearchResultDto(s.Title, s.Content)));
        }
    }
}

// .Query(q => q.Match(
//                     new MatchQuery(new Field("content")) { Query = query }));