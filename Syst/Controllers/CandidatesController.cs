using Core;
using Microsoft.AspNetCore.Mvc;

namespace Syst.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CandidatesController : ControllerBase
{
    private readonly ILogger<CandidatesController> _logger;
    private ICandidateRepository _repo;

    //We use REST to make sure we have a reliable API
    public CandidatesController(ILogger<CandidatesController> logger, ICandidateRepository repo)
    {
        _logger = logger;
        //First we create our repository so we can access our CRUD operations
        _repo = repo;
    }

    //Return all candidates in the system
    [ProducesResponseType(200)]
    [HttpGet(Name = "GetCandidates")]
    public async Task<IEnumerable<CandidateDTO>> Get() {
        return await _repo.ReadAll();
    }
    
    //Return a candidate given the candidate id
    [ProducesResponseType(404)]
    [ProducesResponseType(typeof(CandidateDTO), 200)]
    [HttpGet("{id}")]
    public async Task<ActionResult<CandidateDTO>> Get(int id)
    {   
        //Using our CRUD operation to get the specified candidate by id
        var res = await _repo.Read(id);
        if (res.Item1 == Status.NotFound) {
            return res.Item1.ToActionResult();
        } else {
            return new ActionResult<CandidateDTO>(res.Item2);
        }
    }

    //Create a new candidate
    [ProducesResponseType(409)]
    [HttpPost]
    public async Task<IActionResult> Post(CandidateDTO candidate) {
        var created = await _repo.Create(candidate);
        var id = created.Item2;
        if (created.Item1 == Status.Conflict) return new ConflictObjectResult(id);
        return CreatedAtAction(nameof(Get), new { id }, id);
    }
    
    //Deletes a candidate
    [ProducesResponseType(404)]
    [ProducesResponseType(204)]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id){
        var deleted = await _repo.Delete(id);
        if (deleted == Status.NotFound) return new NotFoundObjectResult(id);
        return deleted.ToActionResult();
    }
}