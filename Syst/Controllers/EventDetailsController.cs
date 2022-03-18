using Core;
using Microsoft.AspNetCore.Mvc;

namespace Syst.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EventDetailsController : ControllerBase
{
    private readonly ILogger<EventDetailsController> _logger;
    private IEventRepository _repo;
    
    //We use REST to make sure we have a reliable API
    public EventDetailsController(ILogger<EventDetailsController> logger, IEventRepository repo)
    {
        _logger = logger;
        //First we create our repository so we can access our CRUD operations
        _repo = repo;
    }
    
    //Return an event given the event id
    [ProducesResponseType(404)]
    [ProducesResponseType(typeof(EventDTO), 200)]
    [HttpGet("{id}")]
    public async Task<ActionResult<EventDTO>> Get(int id)
    {   
        //Using our CRUD operation to get the specified candidate by id
        var res = await _repo.Read(id);
        if (res.Item1 == Status.NotFound) {
            return res.Item1.ToActionResult();
        } else {
            return new ActionResult<EventDTO>(res.Item2);
        }
    }
    
    //Update an event
    [ProducesResponseType(404)]
    [ProducesResponseType(204)]
    [HttpPut("{id}")]
    public async Task<IActionResult> Put(int id, [FromBody] EventDTO oldEvent) =>
        (await _repo.Update(id,oldEvent)).ToActionResult();
    
    //Deletes an event
    [ProducesResponseType(404)]
    [ProducesResponseType(204)]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id){
        var deleted = await _repo.Delete(id);
        if (deleted == Status.NotFound) return new NotFoundObjectResult(id);
        return deleted.ToActionResult();
    }
}