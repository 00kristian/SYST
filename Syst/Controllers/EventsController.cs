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

    //Return all events stored in the database
    [ProducesResponseType(200)]
    [HttpGet(Name = "GetEvents")]
    public async Task<IEnumerable<EventDTO>> Get()
    {
        return await _repo.ReadAll(); 
    }


    //Return an event given an id
    [ProducesResponseType(404)]
    [ProducesResponseType(typeof(EventDTO), 200)]
    [HttpGet("{id}")]
    public async Task<ActionResult<EventDTO>> Get(int id)
    {
        var res = await _repo.Read(id);
        if (res.Item1 == Status.NotFound) {
            return res.Item1.ToActionResult();
        } else {
            return new ActionResult<EventDTO>(res.Item2);
        }
    }
}