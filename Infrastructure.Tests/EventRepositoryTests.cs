using Xunit;
using System;
using System.Collections.Generic;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Core;

namespace Infrastructure.Tests;

public class EventRepositoryTests {

     private readonly ISystematicContext _context; 
    private readonly IEventRepository _repo;
    Event event1 = new Event{ Id = 1, Name="Workshop event", Date = new DateTime{}, Location="ITU", Candidates= new List<Candidate>{ }, Quiz = new Quiz{}, Rating=4.0};
    Event event2 = new Event{ Id = 2, Name="Swagger event", Date = new DateTime{}, Location="Scrollbar", Candidates= new List<Candidate>{ }, Quiz = new Quiz{}, Rating=7.0};

    public EventRepositoryTests(){
        var connection = new SqliteConnection("Filename=:memory:");
        connection.Open();
        var builder = new DbContextOptionsBuilder<SystematicContext>();
        builder.UseSqlite(connection);
        var context = new SystematicContext(builder.Options);
        context.Database.EnsureCreated();
        
        context.Events.AddRange(
            event1,
            event2
        );

 
        context.SaveChanges();
        _context = context;
        _repo = new EventRepository(_context);
    }



    [Fact]
    public async void Create_Creates_Event_In_Repository()
    {
        //Arrange
        var event3 = new EventDTO{Name="Swagger event", Date = new DateTime{}, Location="Scrollbar", Rating=7.0};

        //Act
        var actual = await _repo.Create(event3);

        //Assert
        Assert.Equal(Status.Created, actual.Item1);
        Assert.Equal(3, actual.Item2);
    }

     [Fact]
    public async void Create_Returns_Conflict_When_ID_Is_In_the_database()
    {
        //Arrange
        var event3 = new EventDTO{Id=2, Name="Swagger event", Date = new DateTime{}, Location="Scrollbar", Rating=7.0};


        //Act
        var actual = await _repo.Create(event3);

        //Assert
        Assert.Equal(Status.Conflict, actual.Item1);
        Assert.Equal(2, actual.Item2);
    }

    [Fact]
    public async void Read_returns_event1_when_given_ID_1()
    {
        //act
        var  actual =await  _repo.Read(1);

        //assert
        
        Assert.Equal(Status.Found, actual.Item1);
        Assert.Equal(event1.Name, actual.Item2.Name);
        Assert.Equal(event1.Date, actual.Item2.Date);
        Assert.Equal(event1.Location, actual.Item2.Location);
        Assert.Equal(event1.Rating, actual.Item2.Rating);
    }

    [Fact]
    public async void Read_returns_status_notFound_when_given_nonexisting_ID()
    {
        //act
        var actual = await _repo.Read(22);
        //assert
        Assert.Equal(Status.NotFound, actual.Item1);
        Assert.Equal(default(EventDTO), actual.Item2);
    }


    //virker ikke endnu
    [Fact]
    public async void Read_all_returns_all_events_in_repo()
    {
         //act
        var events = await _repo.ReadAll();

        //assert
        Assert.Collection(events,
            firstEvent => Assert.Equal(new EventDTO(1,"Lukas Hjelmstrand", new DateTime{}, "Bsc i Softwareudvikling", new List<CandidateDTO>{}, new QuizDTO {}, 4.0, new List<AdminDTO> {}), firstEvent),
            SecondEvent => Assert.Equal(new EventDTO(2, "Rene Dif", new DateTime { }, "Msc i Vand", new List<CandidateDTO> { }, new QuizDTO { }, 7.0, new List<AdminDTO> { }), SecondEvent)
        );
    }


    [Fact]
    public async void ReadNameFromId_returns_Rene_Dif_when_given_id_2()
    {
         //act
        var actual = await _repo.ReadNameFromId(2);
         //assert

        Assert.Equal(Status.Found, actual.Item1);
        Assert.Equal(event2.Name, actual.Item2);
    }

    [Fact]
    public async void ReadNameFromId_returns_status_not_found_when_given_nonexisiting_Id()
    {
        // act
        var actual = await _repo.ReadNameFromId(14);
        //assert
        Assert.Equal(Status.NotFound, actual.Item1);
        Assert.Equal(null, actual.Item2);
    }

    [Fact]
    public async void Update_returns_status_Updated_when_given_existing_id()
    {
        // Arrange
        var newEvent = new EventDTO{
            Id = 1, 
            Name = "HackIt",
            Location = "KU",
            Rating = 9.0,
        };
    
        // Act
        var actual = await _repo.Update(1, newEvent);
    
        // Assert
        Assert.Equal(Status.Updated, actual);
    }

     [Fact]
    public async void ReadNameFromId_returns_correct_name_after_updating_Event_with_id_1()
    {
        // Arrange
        var newEvent = new EventDTO{
            Id = 1, 
            Name = "HackIt",
            Location = "KU",
            Rating = 9.0,
        };
    
        // Act
        await _repo.Update(1, newEvent);
        var actual = await _repo.ReadNameFromId(1);
    
        // Assert
        Assert.Equal(Status.Found, actual.Item1);
        Assert.Equal(newEvent.Name, actual.Item2);
    }

    [Fact]
    public async void Update_returns_not_found_when_given_nonexisiting_id()
    {
        // Arrange
        var newEvent = new EventDTO{
            Name = "HackIt",
            Location = "KU",
            Rating = 9.0,
        };
    
        // Act
        var actual = await _repo.Update(13, newEvent);
    
        // Assert
        Assert.Equal(Status.NotFound, actual);
    }

    [Fact]
    public async void Delete_deletes_event2_given_id_2()
    {
        //act 
        var actual = await _repo.Delete(2);

        //assert
        Assert.Equal(Status.Deleted, actual);
    }

    [Fact]
    public async void Delete_returns_status_not_found_when_given_nonexisiting_id()
    {
        //act 
        var actual = await _repo.Delete(98);

        //assert
        Assert.Equal(Status.NotFound, actual);

    }

    [Fact]
    public async void Read_returns_notFound_when_deleting_event_with_id_1()
    {
        // Act
        await _repo.Delete(1);
        var actual = await _repo.Read(1);

        //Assert
        Assert.Equal(Status.NotFound, actual.Item1);
        Assert.Equal(default(EventDTO), actual.Item2);
    }
}