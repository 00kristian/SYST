using Core;
using Microsoft.AspNetCore.Mvc;

namespace Syst.Controllers;

[ApiController]
[Route("api/[controller]")]

//TODO: test this!
public class QuizQuestionController : ControllerBase
{
    private readonly ILogger<QuizQuestionController> _logger;
    private IQuizRepository _repo;

    public QuizQuestionController(ILogger<QuizQuestionController> logger, IQuizRepository repo)
    {
        _logger = logger;
        _repo = repo;
    }

    //Updates the question
    [ProducesResponseType(404)]
    [ProducesResponseType(204)]
    [HttpPut("{id}")]
    public async Task<IActionResult> Put(int id, [FromBody] CreateQuestionDTO newQuestion) {
        var created = await _repo.AddQuestion(id, newQuestion);
        var index = created.Item2;
        if (created.Item1 == Status.Conflict) return new ConflictObjectResult(index);
        return CreatedAtAction(nameof(Put), new { index }, index);
    }

    // [ProducesResponseType(404)]
    // [ProducesResponseType(204)]
    // [HttpDelete("{quizId}/{questionId}")]
    // public async Task<IActionResult> Delete(int quizId, int questionId){
    //     var deleted = await _repo.RemoveQuestion(quizId, questionId);
    //     if (deleted == Status.NotFound) return new NotFoundObjectResult(questionId);
    //     return deleted.ToActionResult();
    // }  
}