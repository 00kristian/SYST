using Xunit;
using Syst.Controllers;
using Moq;
using Core;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace Syst.Tests;

public class EventsQueryControllerTests
{
    //Create objects to test on
    static readonly QuizDTO quiz1 = new QuizDTO
    {
        Id = 90,
        Name = "Swag",
        Questions = null!,
        Events = null!,
        Candidates = null!
    };

    static readonly EventDTO event1 = new EventDTO
    {
        Id = 1,
        Name = "TechBBQ",
        Date = "03-21-2020",
        Location = "Copenhagen",
        Candidates = null!,
        Quiz = quiz1,
        Rating = 3.5,
    };

    static readonly EventDTO event2 = new EventDTO
    {
        Id = 2,
        Name = "We Love Tech!",
        Date = "05-30-2026",
        Location = "London",
        Candidates = null!,
        Quiz = quiz1,
        Rating = 4.8,
    };

    static readonly EventDTO event3 = new EventDTO
    {
        Id = 1,
        Name = "IT Fest",
        Date = "12-24-2030",
        Location = "Copenhagen",
        Candidates = null!,
        Quiz = quiz1,
        Rating = 1.2,
    };





    //Testing starts here
    [Fact]
    public  void Get_recent_returns_Recent()
    {
        //Assert
        var logger = new Mock<ILogger<EventsController>>();
        var repository = new Mock<IEventRepository>();
        var recentEvents = new List<EventDTO> { event1 };
        repository.Setup(m => m.ReadRecent()).ReturnsAsync(recentEvents);
        var controller = new EventsQueryController(logger.Object, repository.Object);

        //Act
        var response = controller.Get("recent");

        //Assert
        Assert.Equal(recentEvents, response.Result);
    }


    [Fact]
    public void Get_upcoming_returns_Upcoming()
    {
        //Assert
        var logger = new Mock<ILogger<EventsController>>();
        var repository = new Mock<IEventRepository>();
        var upcomingEvents = new List<EventDTO> { event2, event3 };
        repository.Setup(m => m.ReadUpcoming()).ReturnsAsync(upcomingEvents);
        var controller = new EventsQueryController(logger.Object, repository.Object);

        //Act
        var response =  controller.Get("upcoming");

        //Assert
        Assert.Equal(upcomingEvents, response.Result);
    }


    [Fact]
    public  void Get_neither_recent_nor_upcoming_returns_All()
    {
        //Assert
        var logger = new Mock<ILogger<EventsController>>();
        var repository = new Mock<IEventRepository>();
        var events = new List<EventDTO> { event1, event2, event3 };
        repository.Setup(m => m.ReadAll()).ReturnsAsync(events);
        var controller = new EventsQueryController(logger.Object, repository.Object);

        //Act
        var response =  controller.Get("");

        //Assert
        Assert.Equal(events, response.Result);
    }

}