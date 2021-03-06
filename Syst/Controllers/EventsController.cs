using Core;
using Microsoft.AspNetCore.Authorization;
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
    [Authorize]
    [ProducesResponseType(200)]
    [HttpGet(Name = "GetEvents")]
    public async Task<IEnumerable<EventDTO>> GetAll()
    {
        return await _repo.ReadAll(); 
    }


    //Return an event given an id
    [Authorize]
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
    [Authorize]
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
    [Authorize]
    [ProducesResponseType(404)]
    [ProducesResponseType(204)]
    [HttpPut("{id}")]
    public async Task<IActionResult> Put(int id, [FromBody] CreateEventDTO newEvent) =>
        (await _repo.Update(id, newEvent)).ToActionResult();

    //Update an event
    [Authorize]
    [ProducesResponseType(404)]
    [ProducesResponseType(204)]
    [HttpPut("{eventid}/{quizid}")]
    public async Task<IActionResult> Put(int eventid, int quizid) =>
        (await _repo.UpdateQuiz(eventid, quizid)).ToActionResult();
    
    //Update an event
    [Authorize]
    [ProducesResponseType(404)]
    [ProducesResponseType(204)]
    [HttpPut("rating/{id}")]
    public async Task<IActionResult> Put(int id, [FromBody] double rating) =>
        (await _repo.UpdateRating(id, rating)).ToActionResult();
    
    //Delete an event
    [Authorize]
    [ProducesResponseType(404)]
    [ProducesResponseType(204)]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id){
        var deleted = await _repo.Delete(id);
        if (deleted == Status.NotFound) return new NotFoundObjectResult(id);
        return deleted.ToActionResult();
    }
    
    //Picks winners
    [Authorize]
    [ProducesResponseType(404)]
    [ProducesResponseType(typeof(IEnumerable<CandidateDTO>), 200)]
    [HttpGet("winners/{eventId}/{numOfWinners}")]
    public async Task<ActionResult<IEnumerable<CandidateDTO>>> GetWinners(int eventId, int numOfWinners)
    {
        var result = await _repo.PickMultipleWinners(eventId, numOfWinners);

        if (result.Item1 == Status.NotFound)
        {
            return new NotFoundObjectResult(eventId);
        }
        return new ActionResult<IEnumerable<CandidateDTO>>(result.Item2);
        
    }

}