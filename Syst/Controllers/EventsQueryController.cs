using Core;
using Microsoft.AspNetCore.Mvc;

namespace Syst.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EventsQueryController : ControllerBase
{
    private readonly ILogger<EventsController> _logger;
    private IEventRepository _repo;

    //We use REST to make sure we have a reliable API
    public EventsQueryController(ILogger<EventsController> logger, IEventRepository repo)
    {
        _logger = logger;
        //First we create our repository so we can access our CRUD operations
        _repo = repo;
    }
    
    //Returns the recent events in the database
    [ProducesResponseType(404)]
    [ProducesResponseType(200)]
    [HttpGet(Name = "GetQuery")]
    [HttpGet("{query}")]
    public async Task<IEnumerable<EventDTO>> Get(string query)
    {
        if (query == "recent") {
            return await _repo.ReadRecent();
        } else if (query == "upcoming") {
            return await _repo.ReadUpcoming();
        }
        else return await _repo.ReadAll();
    }
}