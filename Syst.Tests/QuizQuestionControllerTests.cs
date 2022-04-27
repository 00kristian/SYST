using System.Collections.Generic;
using Core;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;
using Microsoft.AspNetCore.Mvc;
using Syst.Controllers;

namespace Syst.Tests;

public class QuizQuestionControllerTests{

   static readonly CreateQuestionDTO question1 = new CreateQuestionDTO{
        Representation = "What is this?",
        Answer = "A tiger",
        Options = new List<string> {"A lion", "A tiger","A panther", "A leopard"},
        ImageURl = "url/image"
    };


    [Fact]
    public async void Put_returns_Status_updated_when_given_existing_id()
    {

        //arrange
        var logger = new Mock<ILogger<QuizQuestionController>>();
        var repository = new Mock<IQuizRepository>();
        var newQuestion = new CreateQuestionDTO("What is this?", "A tiger", new List<string> {"A lion", "A tiger","A panther", "A leopard"}, "url/image");
        repository.Setup(m => m.AddQuestion(1, newQuestion)).ReturnsAsync((Status.Updated,  1));
        var controller = new QuizQuestionController(logger.Object, repository.Object);

        //act 
        var response = await controller.Put(1, newQuestion);

        //assert

        Assert.IsType<CreatedAtActionResult>(response);
    }
}