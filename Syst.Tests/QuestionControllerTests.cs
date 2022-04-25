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
        Name = "Quiz 1",
        Questions = new List<QuestionDTO>{},
        Events = null!,
        Candidates = null!
    };

    static readonly QuizDTO quiz2 = new QuizDTO{
        Id = 33,
        Name = "Quiz 2",
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
    };

        static readonly QuestionDTO Q2 = new QuestionDTO{
        Id = 2,
        Representation = "What will printet lastly?",
        Answer = "C",
        ImageURl = null!,
        Options = new List<string>{"Cat", "Dog", "Mouse", "Bunny"},
    };

        static readonly QuestionDTO Q3 = new QuestionDTO{
        Id = 3,
        Representation = "What will printet as the second option?",
        Answer = "B",
        ImageURl = null!,
        Options = new List<string>{"25", "69", "345", "9"},
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

    public void Get_all_returns_all_questions()
    {
        var logger = new Mock<ILogger<QuestionsController>>();
        var repository = new Mock<IQuestionRepository>();
        var expected = new List <QuestionDTO> {Q1, Q2, Q3};
        repository.Setup(q => q.ReadAll()).ReturnsAsync(expected);
        var controller = new QuestionsController(logger.Object, repository.Object);

        var actual = controller.GetAll();

        Assert.Equal(expected, actual.Result);
    }

    
    [Fact]

    public async void Post_adds_question_to_repository()
    {
        //Arrange
        var logger = new Mock<ILogger<QuestionsController>>();
        var repository = new Mock<IQuestionRepository>();
        var Q4 = new CreateQuestionDTO("What will be printed next?", "D", new List<string>{"Skateboard", "Boat", "Car", "Bicycle"}, "img");
        var questions = new List<CreateQuestionDTO>{Q4};
        var postQ5 = new CreateQuestionDTO("What will the outcome be?", "A", new List<string>{"Denmark", "Sweden", "Norway", "Finlan"}, "img");
        repository.Setup(q => q.Create(postQ5)).Callback(() => questions.Add(postQ5));
        var controller = new QuestionsController(logger.Object, repository.Object);

        //Act
        var actual = await controller.Post(postQ5);

        //Assert
        Assert.Equal(2,questions.Count);
        Assert.IsType<CreatedAtActionResult>(actual);

    }

    [Fact]

    public async void Post_existing_question_returns_StatusConflict()
    {
        //Arrange
        var logger = new Mock<ILogger<QuestionsController>>();
        var repository = new Mock<IQuestionRepository>();
        var questions = new List<QuestionDTO>{Q1};
        var postQ1 = new CreateQuestionDTO("What will the program print?", "A", new List<string>{"Hello World!", "World Hello!", "Hello! World", "World! Hello"}, "$");
        repository.Setup(q => q.Create(postQ1)).ReturnsAsync(() => (Status.Conflict, 1));
        var controller = new QuestionsController(logger.Object, repository.Object);

        //Act
        var actual = await controller.Post(postQ1);

        //Assert
        Assert.Equal(1,questions.Count);
        Assert.IsType<ConflictObjectResult>(actual);

    }

    [Fact]

    public async void Post_null_DTO_returns_StatusConflict()
    {
        //Arrange
        var logger = new Mock<ILogger<QuestionsController>>();
        var repository = new Mock<IQuestionRepository>();
        var newQuestion = default(CreateQuestionDTO);
        repository.Setup(q => q.Create(newQuestion)).ReturnsAsync(() => (Status.Conflict, 0));
        var controller = new QuestionsController(logger.Object, repository.Object);

        //Act
        var actual = await controller.Post(newQuestion);

        //Assert
        Assert.IsType<ConflictObjectResult>(actual);

    }

    [Fact]

    public async void Put_existing_question_updates_question()
    {
        //Arrange
        var logger = new Mock<ILogger<QuestionsController>>();
        var repository = new Mock<IQuestionRepository>();
        var oldQuestion = Q2;
        var newQuestion = new CreateQuestionDTO("What will printet lastly?", "B", new List<string>{"Cat", "Dog", "Mouse", "Bunny"}, "img");
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
        var Q2b = new CreateQuestionDTO() {
            Representation = Q2.Representation,
            Answer = Q2.Answer,
            Options = Q2.Options,
            ImageURl = Q2.ImageURl
        };
        repository.Setup(q => q.Update(420, Q2b)).ReturnsAsync((Status.NotFound));
        var controller = new QuestionsController(logger.Object, repository.Object);

        //Act
        var actual = await controller.Put(420, Q2b);

        //Assert
        Assert.IsType<NotFoundResult>(actual);
    }

    [Fact]

    public async void Delete_non_existing_question_returns_NotFound()
    {
        //Arrange
        var logger = new Mock<ILogger<QuestionsController>>();
        var repository = new Mock<IQuestionRepository>();
        repository.Setup(q => q.Delete(13)).ReturnsAsync((Status.NotFound));
        var controller = new QuestionsController(logger.Object, repository.Object);

        //Act
        var actual = await controller.Delete(13);

        //Assert
        Assert.IsType<NotFoundObjectResult>(actual);
    }

    [Fact]

    public async void Delete_existing_question_returns_NoContent()
    {
        //Arrange
        var logger = new Mock<ILogger<QuestionsController>>();
        var repository = new Mock<IQuestionRepository>();
        repository.Setup(q => q.Delete(3)).ReturnsAsync((Status.Deleted));
        var controller = new QuestionsController(logger.Object, repository.Object);

        //Act
        var actual = await controller.Delete(3);

        //Assert
        Assert.IsType<NoContentResult>(actual);
    }
}