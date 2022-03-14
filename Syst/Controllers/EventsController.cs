using Core;
using Microsoft.AspNetCore.Mvc;

namespace Syst.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EventsController : ControllerBase
{
    private readonly ILogger<EventsController> _logger;
    private IEventRepository _repo;

    public EventsController(ILogger<EventsController> logger, IEventRepository repo)
    {
        _logger = logger;
        _repo = repo;
    }

    [ProducesResponseType(200)]
    [HttpGet(Name = "GetEvents")]
    public async Task<IEnumerable<EventDTO>> Get()
    {
        return await _repo.ReadAll(); 
    }
}