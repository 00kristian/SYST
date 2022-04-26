using System.Collections.Generic;
using Core;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;
using Microsoft.AspNetCore.Mvc;
using Syst.Controllers;

namespace Syst.Tests;

public class ImageConrollerTests{

   static readonly CreateQuestionDTO question1 = new CreateQuestionDTO{
        Representation = "What is this?",
        Answer = "A tiger",
        Options = new List<string> {"A lion", "A tiger","A panther", "A leopard"},
        ImageURl = "url/image"
    };

    //TO DO: TEST THIS CONTROLLER
    [Fact]
    public async void Post_adds_image_to_question()
    {

        /* //arrange
        var logger = new Mock<ILogger<ImageController>>();
        var repository = new Mock<IQuestionRepository>();
        var newQuestion = new CreateQuestionDTO("What is this?", "A tiger", new List<string> {"A lion", "A tiger","A panther", "A leopard"}, "url/image");
        var questions = new List<CreateQuestionDTO>{question1};
        repository.Setup(m => m.Create(newQuestion)).Callback(()=>questions.Add(newQuestion));
        var controller = new ImageController(logger.Object, repository.Object);
        var response = await controller.Post(1, newQuestion);

        //assert

        Assert.IsType<CreatedAtActionResult>(response); */
    }
}