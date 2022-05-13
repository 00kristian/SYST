using Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Syst.Controllers;

[ApiController]
[Route("api/[controller]")]
public class QuestionsController : ControllerBase
{
    private readonly ILogger<QuestionsController> _logger;
    private IQuestionRepository _repo;

    //We use REST to make sure we have a reliable API
    public QuestionsController(ILogger<QuestionsController> logger, IQuestionRepository repo)
    {
        _logger = logger;
        //First we create our repository so we can access our CRUD operations
        _repo = repo;
    }

    //Return all questions stored in the database
    [Authorize]
    [ProducesResponseType(200)]
    [HttpGet(Name = "GetQuestions")]
    public async Task<IEnumerable<QuestionDTO>> GetAll()
    {
        return await _repo.ReadAll(); 
    }


    //Return an question given an id
    [Authorize]
    [ProducesResponseType(404)]
    [ProducesResponseType(typeof(QuestionDTO), 200)]
    [HttpGet("{id}")]
    public async Task<ActionResult<QuestionDTO>> Get(int id)
    {
        var res = await _repo.Read(id);
        if (res.Item1 == Status.NotFound) {
            return res.Item1.ToActionResult();
        } else {
            return new ActionResult<QuestionDTO>(res.Item2);
        }
    }

    //Create a new question
    [Authorize]
    [ProducesResponseType(409)]
    [ProducesResponseType(201)]
    [HttpPost]
    public async Task<IActionResult> Post(CreateQuestionDTO newQuestion) {
        var created = await _repo.Create(newQuestion);
        var id = created.Item2;
        if (created.Item1 == Status.Conflict) return new ConflictObjectResult(id);
        return CreatedAtAction(nameof(Get), new { id }, id);
    }

    //Update an question
    [Authorize]
    [ProducesResponseType(404)]
    [ProducesResponseType(204)]
    [HttpPut("{id}")]
    public async Task<IActionResult> Put(int id, [FromBody] CreateQuestionDTO oldQuestion) =>
        (await _repo.Update(id,oldQuestion)).ToActionResult();
    
    //Delete an question
    [Authorize]
    [ProducesResponseType(404)]
    [ProducesResponseType(204)]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id){
        var deleted = await _repo.Delete(id);
        if (deleted == Status.NotFound) return new NotFoundObjectResult(id);
        return deleted.ToActionResult();
    }
    
}