using Core;
using Microsoft.AspNetCore.Mvc;

namespace Syst.Controllers;

[ApiController]
[Route("api/[controller]")]
public class QuizController : ControllerBase

{

    private readonly ILogger<QuizController> _logger;
    private IQuizRepository _repo;

    //We use REST to make sure we have a reliable API
    public QuizController(ILogger<QuizController> logger, IQuizRepository repo)
    {
        _logger = logger;
        //First we create our repository so we can access our CRUD operations
        _repo = repo;
    }


    //Return all quiz stored in the database related to the event
    [ProducesResponseType(200)]
    [HttpGet(Name = "GetQuizes")]
    public async Task<IEnumerable<QuizDTO>> GetAll()
    {
        return await _repo.ReadAll(); 
    }

    //Create a new quiz
    [ProducesResponseType(409)]
    [ProducesResponseType(201)]
    [HttpPost]
    public async Task<IActionResult> Post(QuizCreateDTO newQuiz) {
        var created = await _repo.Create(newQuiz);
        var id = created.Item2;
        if (created.Item1 == Status.Conflict) return new ConflictObjectResult(id);
        return CreatedAtAction(nameof(Get), new { id }, id);
    }

    
    //Return a quiz given an id
    [ProducesResponseType(404)]
    [ProducesResponseType(typeof(QuizDTO), 200)]
    [HttpGet("{id}")]
    public async Task<ActionResult<QuizDTO>> Get(int id)
    {
        var res = await _repo.Read(id);
        if (res.Item1 == Status.NotFound) {
            return res.Item1.ToActionResult();
        } else {
            return new ActionResult<QuizDTO>(res.Item2);
        }
    }
    
    //Update a quiz
    [ProducesResponseType(404)]
    [ProducesResponseType(204)]
    [HttpPut("{id}")]
    public async Task<IActionResult> Put(int id, [FromBody] QuizCreateDTO newQuiz) =>
        (await _repo.Update(id,newQuiz)).ToActionResult();


    //Delete a quiz
    [ProducesResponseType(404)]
    [ProducesResponseType(204)]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id){
        var deleted = await _repo.Delete(id);
        if (deleted == Status.NotFound) return new NotFoundObjectResult(id);
        return deleted.ToActionResult();
    } 

    //Clone a quiz
    [ProducesResponseType(404)]
    [ProducesResponseType(204)]
    [HttpPut("{quizid}/clone/{cloneid}")]
    public async Task<IActionResult> Clone([FromRoute] int quizid, [FromRoute] int cloneid) =>
        (await _repo.Clone(quizid, cloneid)).ToActionResult();
}