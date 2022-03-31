using Xunit;
using Syst.Controllers;
using Moq;
using Core;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace Syst.Tests;

public class QuestionsControllerTests
{
    //Objects to use for testing
    static readonly QuizDTO quiz1 = new QuizDTO{
        Id = 99,
        Date = new System.DateTime(2022,11,21),
        Questions = new List<QuestionDTO>{},
        Events = null!,
        Candidates = null!
    };
    static readonly QuestionDTO Q1 = new QuestionDTO{
        Id = 1,
        Representation = "What will the program print?",
        Answer = "A",
        ImageURl = null!,
        Options = new List<string>{"Hello World!", "World Hello!", "Hello! World", "World! Hello"},
        Quiz = quiz1
    };

        static readonly QuestionDTO Q2 = new QuestionDTO{
        Id = 2,
        Representation = "What will printet lastly?",
        Answer = "C",
        ImageURl = null!,
        Options = new List<string>{"Cat", "Dog", "Mouse", "Bunny"},
        Quiz = quiz1
    };

        static readonly QuestionDTO Q3 = new QuestionDTO{
        Id = 3,
        Representation = "What will printet as the second option?",
        Answer = "B",
        ImageURl = null!,
        Options = new List<string>{"25", "69", "345", "9"},
        Quiz = quiz1
    };

    //Testing starts from here

    [Fact]

    public async void Get_existing_id_returns_Question(){
        
        //Arrange
        var logger = new Mock<ILogger<QuestionsController>>();
        var repository = new Mock<IQuestionRepository>();
        var expected = Q1;
        repository.Setup(q => q.Read(1)).ReturnsAsync((Status.Found, expected));
        var controller = new QuestionsController(logger.Object, repository.Object);

        //Act
        var actual = await controller.Get(1);

        //Assert
        Assert.Equal(expected, actual.Value);
    }

    [Fact]

    public async void Get_non_existing_id_return_NotFound()
    {
        //Arrange
        var logger = new Mock<ILogger<QuestionsController>>();
        var repository = new Mock<IQuestionRepository>();
        repository.Setup(q => q.Read(69)).ReturnsAsync((Status.NotFound, default(QuestionDTO)));
        var controller = new QuestionsController(logger.Object, repository.Object);

        //Act
        var actual = await controller.Get(69);

        //Assert
        Assert.IsType<NotFoundResult>(actual.Result);
    }

    [Fact]

    public async void Put_existing_question_updates_question()
    {
        //Arrange
        var logger = new Mock<ILogger<QuestionsController>>();
        var repository = new Mock<IQuestionRepository>();
        var oldQuestion = Q2;
        var newQuestion = new QuestionDTO(2, "What will printet lastly?", "B", null, new List<string>{"Cat", "Dog", "Mouse", "Bunny"}, quiz1);
        repository.Setup(q => q.Update(2, newQuestion)).Callback(() => {
            oldQuestion.Answer = newQuestion.Answer;
        }).ReturnsAsync(Status.Updated);
        var controller = new QuestionsController(logger.Object, repository.Object);

        //Act
        var actual = await controller.Put(2, newQuestion);

        //Assert
        Assert.IsType<NoContentResult>(actual);
        Assert.Equal(newQuestion.Answer, oldQuestion.Answer);
    }

    [Fact]

    public async void Put_non_existing_question_returns_NotFound()
    {
        //Arrange
        var logger = new Mock<ILogger<QuestionsController>>();
        var repository = new Mock<IQuestionRepository>();
        repository.Setup(q => q.Update(420, Q2)).ReturnsAsync((Status.NotFound));
        var controller = new QuestionsController(logger.Object, repository.Object);

        //Act
        var actual = await controller.Put(420, Q2);

        //Assert
        Assert.IsType<NotFoundResult>(actual);
    }

}