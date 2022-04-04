using Core;
using Microsoft.AspNetCore.Mvc;

namespace Syst.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EventsController : ControllerBase
{
    private readonly ILogger<EventsController> _logger;
    private IEventRepository _repo;

    //We use REST to make sure we have a reliable API
    public EventsController(ILogger<EventsController> logger, IEventRepository repo)
    {
        _logger = logger;
        //First we create our repository so we can access our CRUD operations
        _repo = repo;
    }

    //Return all events stored in the database
    [ProducesResponseType(200)]
    [HttpGet(Name = "GetEvents")]
    public async Task<IEnumerable<EventDTO>> GetAll()
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

    //Create a new event
    [ProducesResponseType(409)]
    [ProducesResponseType(201)]
    [HttpPost]
    public async Task<IActionResult> Post(CreateEventDTO newEvent) {
        var created = await _repo.Create(newEvent);
        var id = created.Item2;
        if (created.Item1 == Status.Conflict) return new ConflictObjectResult(id);
        return CreatedAtAction(nameof(Get), new { id }, id);
    }

    //Update an event
    [ProducesResponseType(404)]
    [ProducesResponseType(204)]
    [HttpPut("{id}")]
    public async Task<IActionResult> Put(int id, [FromBody] EventDTO newEvent) =>
        (await _repo.Update(id, newEvent)).ToActionResult();

    //Update an event
    [ProducesResponseType(404)]
    [ProducesResponseType(204)]
    [HttpPut("{eventid}/{quizid}")]
    public async Task<IActionResult> Put(int eventid, int quizid) =>
        (await _repo.UpdateQuiz(eventid, quizid)).ToActionResult();
    
    //Delete an event
    [ProducesResponseType(404)]
    [ProducesResponseType(204)]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id){
        var deleted = await _repo.Delete(id);
        if (deleted == Status.NotFound) return new NotFoundObjectResult(id);
        return deleted.ToActionResult();
    }
    
}