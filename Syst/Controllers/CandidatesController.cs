using System.Net;
using Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
    [Authorize]
    [ProducesResponseType(200)]
    [HttpGet(Name = "GetCandidates")]
    public async Task<IEnumerable<CandidateDTO>> Get() 
    {
        return await _repo.ReadAll();
    }
    
    //Return a candidate given the candidate id
    [Authorize]
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
    [Authorize]
    [ProducesResponseType(409)]
    [HttpPost]
    
    public async Task<IActionResult> Post(CreateCandidateDTO candidate) 
    {
        var created = await _repo.Create(candidate);
        var id = created.Item2;
        if (created.Item1 == Status.Conflict) return new ConflictObjectResult(id);
        return CreatedAtAction(nameof(Get), new { id }, id);
    }
    
    //Deletes a candidate
    [Authorize]
    [ProducesResponseType(404)]
    [ProducesResponseType(204)]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _repo.Delete(id);
        if (deleted == Status.NotFound) return new NotFoundObjectResult(id);
        return deleted.ToActionResult();
    }
    
    //Updates a candidate
    [Authorize]
    [ProducesResponseType(404)]
    [ProducesResponseType(202)]
    [HttpPut("{id}")]
    public async Task<IActionResult> Put(int id, CandidateDTO candidate)
    {
        var updated = await _repo.Update(id, candidate);
        if (updated == Status.NotFound) return new NotFoundObjectResult(id);
        return updated.ToActionResult();
    } 

    
    //Updates a upvote for candidate
    [Authorize]
    [ProducesResponseType(404)]
    [ProducesResponseType(202)]
    [HttpPut("upvote/{id}")]
    public async Task<IActionResult> PutUpVote(int id)
    {
        var updated = await _repo.UpdateUpVote(id);
        if (updated == Status.NotFound) return new NotFoundObjectResult(id);
        return updated.ToActionResult();
    }
    
    //Create a new answer to candidate 
    [Authorize]
    [ProducesResponseType(409)]
    [HttpPost("Answer/{candidateId}")]
    public async Task<IActionResult> PostAnswer([FromRoute] int candidateId, [FromBody] AnswerDTO answer) {
        var res = await _repo.AddAnswer(candidateId, answer);
        if (res == Status.NotFound) return new NotFoundObjectResult(candidateId);
        return res.ToActionResult();
    }

    //This is maybe more like a get request
    [Authorize]
    [ProducesResponseType(404)]
    [ProducesResponseType(typeof(CandidateDTO), 200)]
    [HttpPut("GraphData")]
    public async Task<ActionResult<int[]>> GraphData(string[] universities)
    {   
        //Using our CRUD operation to get the specified candidate by id
        var res = await _repo.GraphData(universities);
        return new ActionResult<int[]>(res);
    }
}