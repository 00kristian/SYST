using Xunit;
using Syst.Controllers;
using Moq;
using Core;
using Infrastructure;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace Syst.Tests;

public class EventControllerTests
{
    //Create some objects to test on
    static readonly QuizDTO quiz1 = new QuizDTO{
        Id = 90,
        Name = "Swag",
        Questions = null!,
        Events = null!,
        Candidates = null!
    };

    static readonly CandidateDTO candidate1 = new CandidateDTO {
		Id = 1,
		Name = "Bob Jensen",
		Email = "bobj@dtu.dk",
		StudyProgram = "Datalogi",
		University = "DTU",
		GraduationDate = "20-06-2024",
		Events = null!,
		Quiz = quiz1,
		IsUpvoted = false
	};

    static readonly Candidate candidate2 = new Candidate{
        Id = 2,
        Name = "Michael Jackson",
        Email = "jackson@dtu.dk",
        CurrentDegree ="BSC",
        StudyProgram = "Software",
        University = "DTU",
        GraduationDate = System.DateTime.Today,
        PercentageOfCorrectAnswers = 0.0,
        EventsParticipatedIn = null,
        Quiz = null,
        IsUpvoted = false,
        Answer = null,
        Created = System.DateTime.Today
    };

    static readonly EventDTO event1 = new EventDTO{
        Id = 1,
        Name = "TechBBQ",
        Date = "03-21-2022",
        Location = "Copenhagen",
        Candidates = new List<CandidateDTO>{candidate1},
        Quiz = quiz1,
        Rating = 3.5,
    };

    static readonly EventDTO event2 = new EventDTO{
        Id = 2,
        Name = "We Love Tech!",
        Date = "05-30-2022",
        Location = "London",
        Candidates = null!,
        Quiz = quiz1,
        Rating = 4.8,
        WinnersId = new List<int>{1}
    };

    static readonly EventDTO event3 = new EventDTO{
        Id = 1,
        Name = "IT Fest",
        Date = "12-24-2024",
        Location = "Copenhagen",
        Candidates = null!,
        Quiz = quiz1,
        Rating = 1.2,
    };





    //Testing starts here
    [Fact]
    public async void Get_existing_id_return_Event()
    {
        // Arrange
        var logger = new Mock<ILogger<EventsController>>();
        var repository = new Mock<IEventRepository>();
        var expected = event1;
        repository.Setup(m => m.Read(1)).ReturnsAsync((Status.Found, expected));
        var controller = new EventsController(logger.Object, repository.Object);

        // Act
        var response = await controller.Get(1);

        // Assert
        Assert.Equal(expected, response.Value);
    }

    [Fact]
    public async void Get_non_existing_id_return_NotFound()
    {
        //Arrange
        var logger = new Mock<ILogger<EventsController>>();
        var repository = new Mock<IEventRepository>();
        repository.Setup(m => m.Read(99)).ReturnsAsync((Status.NotFound, default(EventDTO)));
        var controller = new EventsController(logger.Object, repository.Object);

        // Act
        var response = await controller.Get(99);

        // Assert
        Assert.IsType<NotFoundResult>(response.Result);
    }

    [Fact]
    public async void Put_updates_Event()
    {
        //Arrange
        var logger = new Mock<ILogger<EventsController>>();
        var updater = new CreateEventDTO() {
            Name = event2.Name,
            Date = event2.Date,
            Location = event2.Location
        };
        var oldEvent = event1;
        var repository = new Mock<IEventRepository>();

        repository.Setup(m => m.Update(1, updater)).Callback(() => {
                oldEvent.Name = updater.Name; 
                oldEvent.Location = updater.Location;
            }).ReturnsAsync(Status.Updated);
        
        var controller = new EventsController(logger.Object, repository.Object);

        //Act
        var response = await controller.Put(1, updater);

        //Assert
        Assert.IsType<NoContentResult>(response);
        Assert.Equal(updater.Name, oldEvent.Name);
        Assert.Equal(updater.Location, oldEvent.Location);

    }

    [Fact]
    public async void Put_unknown_id_return_NotFound()
    {
        //Arrange
        var logger = new Mock<ILogger<EventsController>>();
        var repository = new Mock<IEventRepository>();
        var updater = new CreateEventDTO() {
            Name = event2.Name,
            Date = event2.Date,
            Location = event2.Location
        };
        repository.Setup(m => m.Update(99, updater)).ReturnsAsync((Status.NotFound));
        var controller = new EventsController(logger.Object, repository.Object);

        //Act
        var response = await controller.Put(99, updater);

        //Assert
        Assert.IsType<NotFoundResult>(response);
    }

    [Fact]
    public async void Delete_non_existing_id_return_NotFound()
    {
        //Arrange
        var logger = new Mock<ILogger<EventsController>>();
        var repository = new Mock<IEventRepository>();
        repository.Setup(m => m.Delete(90)).ReturnsAsync(Status.NotFound);
        var controller = new EventsController(logger.Object, repository.Object);

        //Act
        var response = await controller.Delete(90);

        //Assert
        Assert.IsType<NotFoundObjectResult>(response);
    }

    [Fact]
    public async void Delete_existing_id_return_NoContent()
    {
        //Arrange
        var logger = new Mock<ILogger<EventsController>>();
        var repository = new Mock<IEventRepository>();
        repository.Setup(m => m.Delete(1)).ReturnsAsync(Status.Deleted);
        var controller = new EventsController(logger.Object, repository.Object);

        //Act
        var response = await controller.Delete(1);

        //Assert
        Assert.IsType<NoContentResult>(response);
    }

    [Fact]
    public async void Post_adds_event_to_repository()
    {
        //Arrange
        var logger = new Mock<ILogger<EventsController>>();
        var repository = new Mock<IEventRepository>();
        var event1 = new CreateEventDTO("TechTalk", "02-02-2022", "Rotterdam");
        var events = new List<CreateEventDTO> {event1};
        var createdEvent = new CreateEventDTO("IT Fest", "01-01-2025", "Copenhagen");
        repository.Setup(m => m.Create(createdEvent)).Callback(() => events.Add(createdEvent));
        var controller = new EventsController(logger.Object, repository.Object);

        //Act
        var response = await controller.Post(createdEvent);

        //Assert
        Assert.IsType<CreatedAtActionResult>(response);
        Assert.Equal(2, events.Count);
    }
    [Fact]
	public async void Post_null_DTO_returns_StatusConflict()
	{
		//Arrange
		var logger = new Mock<ILogger<EventsController>>();
		var repository = new Mock<IEventRepository>();
		var newEvent = default(CreateEventDTO);
		repository.Setup(m => m.Create(newEvent)).ReturnsAsync(() => (Status.Conflict, 0));
		var controller = new EventsController(logger.Object, repository.Object);

		//Act
		var response = await controller.Post(newEvent);

		//Assert
		Assert.IsType<ConflictObjectResult>(response);
	}

    [Fact]
    public async void Post_existing_id_returns_StatusConflict() 
    { 
        //Arrange
        var logger = new Mock<ILogger<EventsController>>();
        var repository = new Mock<IEventRepository>();
        var events = new List<EventDTO> {event1};
        var createdEvent = new CreateEventDTO("IT Fest", "01-01-2025", "Copenhagen");
        repository.Setup(m => m.Create(createdEvent)).ReturnsAsync(() => (Status.Conflict, 1));
        var controller = new EventsController(logger.Object, repository.Object);

        //Act
        var response = await controller.Post(createdEvent);

        //Assert
        Assert.IsType<ConflictObjectResult>(response);
        Assert.Equal(1, events.Count);
        
    }

    [Fact]
    public void Get_all_returns_all_events()
    {
        var logger = new Mock<ILogger<EventsController>>();
        var repository = new Mock<IEventRepository>();
        var events = new List<EventDTO> {event1,event2};
        repository.Setup(m => m.ReadAll()).ReturnsAsync(events);
        var controller = new EventsController(logger.Object, repository.Object);

        var respones = controller.GetAll();

        Assert.Equal(events, respones.Result);
    }

    [Fact]
    public async void Picks_the_multiple_winners(){
        var logger = new Mock<ILogger<EventsController>>();
        var repository = new Mock<IEventRepository>();
        var winners = new List<CandidateDTO>{candidate1};
        repository.Setup(m => m.PickMultipleWinners(1, 1)).ReturnsAsync(() => (Status.Found,  winners));
        var controller = new EventsController(logger.Object, repository.Object);

        var respones = await controller.GetWinners(1, 1);

        Assert.Collection(winners,
            candiate => {
                Assert.Equal(1, candiate.Id);
                Assert.Equal("Bob Jensen", candiate.Name);
                Assert.Equal("bobj@dtu.dk", candiate.Email);
                Assert.Equal("Datalogi", candiate.StudyProgram);
                Assert.Equal("DTU", candiate.University);
                Assert.Equal("20-06-2024", candiate.GraduationDate);
                Assert.Equal(null!, candiate.Events);
                Assert.Equal(quiz1, candiate.Quiz);
                Assert.Equal(false, candiate.IsUpvoted);
            }
        );
    }

    [Fact]
    public async void Returns_NotFoundObject_when_list_is_empty(){
        var logger = new Mock<ILogger<EventsController>>();
        var repository = new Mock<IEventRepository>();
        var winners = new List<CandidateDTO>{};
        repository.Setup(m => m.PickMultipleWinners(1, 10)).ReturnsAsync(() => (Status.NotFound,  winners));
        var controller = new EventsController(logger.Object, repository.Object);

        var respones = await controller.GetWinners(1, 10);

        Assert.IsType<NotFoundObjectResult>(respones.Result);
    }

    [Fact]
    public async void Returns_NotFoundObject_when_no_event_exists(){
        var logger = new Mock<ILogger<EventsController>>();
        var repository = new Mock<IEventRepository>();
        var winners = new List<CandidateDTO>{candidate1};
        repository.Setup(m => m.PickMultipleWinners(10, 1)).ReturnsAsync(() => (Status.NotFound,  winners));
        var controller = new EventsController(logger.Object, repository.Object);

        var respones = await controller.GetWinners(10, 1);

        Assert.IsType<NotFoundObjectResult>(respones.Result);
    }

    [Fact]
        public async void Returns_Found_when_winners_already_exists(){
        var logger = new Mock<ILogger<EventsController>>();
        var repository = new Mock<IEventRepository>();
        var winners = new List<CandidateDTO>{candidate1};
        repository.Setup(m => m.PickMultipleWinners(2, 1)).ReturnsAsync(() => (Status.Found,  winners));
        var controller = new EventsController(logger.Object, repository.Object);

        var respones = await controller.GetWinners(2, 1);

        Assert.Collection(winners,
            candiate => {
                Assert.Equal(1, candiate.Id);
                Assert.Equal("Bob Jensen", candiate.Name);
                Assert.Equal("bobj@dtu.dk", candiate.Email);
                Assert.Equal("Datalogi", candiate.StudyProgram);
                Assert.Equal("DTU", candiate.University);
                Assert.Equal("20-06-2024", candiate.GraduationDate);
                Assert.Equal(null!, candiate.Events);
                Assert.Equal(quiz1, candiate.Quiz);
                Assert.Equal(false, candiate.IsUpvoted);
            }
        );
    }


}
