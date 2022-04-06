using System.Collections.Generic;
using System.Threading.Tasks;
using Core;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;
using Microsoft.AspNetCore.Mvc;
using Syst.Controllers;

namespace Syst.Tests;

public class QuizControllerTests{

    static readonly QuizDTO quiz1 = new QuizDTO{
        Id = 1, 
        Name = "LINQ query",
        Questions = new List<QuestionDTO> {default(QuestionDTO)},
        Events = new List<EventDTO> {default(EventDTO)},
        Candidates = new List<CandidateDTO>{default(CandidateDTO)} 
    };

    static readonly QuizDTO quiz2 = new QuizDTO{
        Id = 2, 
        Name = "Swagsters",
        Questions = new List<QuestionDTO> {default(QuestionDTO)},
        Events = new List<EventDTO> {default(EventDTO)},
        Candidates = new List<CandidateDTO>{default(CandidateDTO)} 
    };



  
    [Fact]
    public void Get_all_returns_all_quizes()
    {
        var logger = new Mock<ILogger<QuizController>>();
        var repository = new Mock<IQuizRepository>();
        var quizes = new List <QuizDTO> {quiz1, quiz2};
        repository.Setup(m => m.ReadAll()).ReturnsAsync(quizes);
        var controller = new QuizController(logger.Object, repository.Object);

        var respones = controller.GetAll();

        Assert.Equal(quizes, respones.Result);
    }



    [Fact]
    public async void Get_existing_id_return_Quiz()
    {
        // Given
        var logger = new Mock<ILogger<QuizController>>();
        var repository = new Mock<IQuizRepository>();
        var expected = quiz1;
        repository.Setup(m => m.Read(1)).ReturnsAsync((Status.Found, expected));
        var controller = new QuizController(logger.Object, repository.Object);
        // When
        var response = await controller.Get(1);
    
        // Then
        Assert.Equal(expected,response.Value);
    }

    [Fact]
    public async void Get_non_existing_id_return_NotFound()
    {
        //Arrange
         var logger = new Mock<ILogger<QuizController>>();
        var repository = new Mock<IQuizRepository>();
        repository.Setup(m => m.Read(99)).ReturnsAsync((Status.NotFound, default(QuizDTO)));
        var controller = new QuizController(logger.Object, repository.Object);

        // Act
        var response = await controller.Get(99);

        // Assert
        Assert.IsType<NotFoundResult>(response.Result);
    }

    [Fact]
    public async void Post_adds_quiz_to_repository()
    {
        //Arrange
        var logger = new Mock<ILogger<QuizController>>();
        var repository = new Mock<IQuizRepository>();
        var newQuiz = new QuizCreateDTO("Hejsan");
        var quizes = new List <QuizCreateDTO> {newQuiz};
        var createdQuiz = new QuizCreateDTO("Cams spørgsmål");
        repository.Setup(m => m.Create(createdQuiz)).Callback(()=> quizes.Add(createdQuiz));
        var controller = new QuizController(logger.Object, repository.Object);

        //act 
        var response = await controller.Post(createdQuiz);

        //Assert
        Assert.IsType<CreatedAtActionResult>(response);
        Assert.Equal(2, quizes.Count);
    }

    //TODO: Test if DTO is null
    [Fact]
    public async void Post_returns_Conflict_when_DTO_is_Null()
    {
        //arrange 
        var logger = new Mock<ILogger<QuizController>>();
        var repository = new Mock<IQuizRepository>();
        var newQuiz = default(QuizCreateDTO);
        repository.Setup(m => m.Create(newQuiz)).ReturnsAsync((Status.Conflict,0 ));
        var controller = new QuizController(logger.Object, repository.Object);

        //act
        var response = await controller.Post(newQuiz);

        //Assert
        Assert.IsType<ConflictObjectResult>(response);


    }

    [Fact]
    public async void Put_returns_Status_updated_when_given_existing_id()
    {

        //arrange
        var logger = new Mock<ILogger<QuizController>>();
        var repository = new Mock<IQuizRepository>();
        var newQuiz = new QuizCreateDTO("Swagsters");
        repository.Setup(m => m.Update(2, newQuiz)).ReturnsAsync(Status.Updated);
        var controller = new QuizController(logger.Object, repository.Object);

        //act 
        var response = await controller.Put(2, newQuiz);

        //assert

        Assert.IsType<NoContentResult>(response);
    }

    [Fact]
    public async Task Put_returns_status_notFound_when_given_nonexisting_idAsync()
    {
        //arrange
        var logger = new Mock<ILogger<QuizController>>();
        var repository = new Mock<IQuizRepository>();
        var newQuiz = new QuizCreateDTO("Swagsters");
        repository.Setup(m => m.Update(99, newQuiz)).ReturnsAsync(Status.NotFound);
        var controller = new QuizController(logger.Object, repository.Object);

        //act 
        var response = await controller.Put(99, newQuiz);

        //assert

        Assert.IsType<NotFoundResult>(response);
    }

    [Fact]
    public async void Delete_non_existing_id_return_NotFound()
    {
        //Arrange
        var logger = new Mock<ILogger<QuizController>>();
        var repository = new Mock<IQuizRepository>();
        repository.Setup(m => m.Delete(90)).ReturnsAsync(Status.NotFound);
        var controller = new QuizController(logger.Object, repository.Object);

        //Act
        var response = await controller.Delete(90);

        //Assert
        Assert.IsType<NotFoundObjectResult>(response);
    }

     [Fact]
    public async void Delete_existing_id_return_NoContent()
    {
        //Arrange
        var logger = new Mock<ILogger<QuizController>>();
        var repository = new Mock<IQuizRepository>();
        repository.Setup(m => m.Delete(1)).ReturnsAsync(Status.Deleted);
        var controller = new QuizController(logger.Object, repository.Object);

        //Act
        var response = await controller.Delete(1);

        //Assert
        Assert.IsType<NoContentResult>(response);
    }




    
}