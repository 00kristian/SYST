using Xunit;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Core;
using NuGet.Frameworks;

namespace Infrastructure.Tests;

public class EventRepositoryTests {

    private readonly ISystematicContext _context; 
    private readonly IEventRepository _repo;

    private Candidate candidate1;
    private Candidate candidate2;
    private Candidate candidate3;

    private Event event1;
    private Event event2;
    
    public EventRepositoryTests(){
        var connection = new SqliteConnection("Filename=:memory:");
        connection.Open();
        var builder = new DbContextOptionsBuilder<SystematicContext>();
        builder.UseSqlite(connection);
        var context = new SystematicContext(builder.Options);
        context.Database.EnsureCreated();
        
        /*context.Candidates.AddRange(
            candidate1, 
            candidate2, 
            candidate3
            );*/
        
        candidate1 = new Candidate {Name = "Lukas Hjelmstrand", Email = "luhj@itu.dk", CurrentDegree = "BSc", StudyProgram = "Softwareudvikling", University = "ITU", GraduationDate = new DateTime{}, IsUpvoted = false};
        candidate2 = new Candidate {Name = "Maj Frost Jensen", Email = "mfje@itu.dk", CurrentDegree = "MSc", StudyProgram = "Computer Science", University = "CBS", GraduationDate = new DateTime{}, IsUpvoted = true};
        candidate3 = new Candidate {Name = "Gustav Svensson", Email = "gs@ku.dk", CurrentDegree = "BSc", StudyProgram = "Law", University = "KU", GraduationDate = new DateTime{}, IsUpvoted = true};

        event1 = new Event{Name="Workshop event", Date = new DateTime{}, Location="ITU", Candidates = new List<Candidate>{candidate1, candidate2, candidate3}, Quiz = new Quiz{}, Rating=4.0, Winners=new List<Candidate>()};
        event2 = new Event{Name="Swagger event", Date = new DateTime{}, Location="Scrollbar", Candidates = new List<Candidate>{}, Quiz = new Quiz{}, Rating=7.0, Winners=new List<Candidate>()};

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
        var event3 = new CreateEventDTO{Name="Swagger event", Date = "2022-03-21", Location="Scrollbar"};

        //Act
        var actual = await _repo.Create(event3);

        //Assert
        Assert.Equal(Status.Created, actual.Item1);
        Assert.Equal(3, actual.Item2);
    }

    // [Fact]
    // public async void Create_Returns_Conflict_When_ID_Is_In_the_database()
    // {
    //     //Arrange
    //     var event3 = new CreateEventDTO{Name="Swagger event", Date = "2022-03-21", Location="Scrollbar"};


    //     //Act
    //     var actual = await _repo.Create(event3);

    //     //Assert
    //     Assert.Equal(Status.Conflict, actual.Item1);
    //     Assert.Equal(2, actual.Item2);
    // }

    [Fact]
    public async void Read_returns_event1_when_given_ID_1()
    {
        //act
        var  actual =await  _repo.Read(1);

        //assert
        
        Assert.Equal(Status.Found, actual.Item1);
        Assert.Equal(event1.Name, actual.Item2.Name);
        Assert.Equal(event1.Date.ToString("yyyy-MM-dd"), actual.Item2.Date);
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


    
    [Fact]
    public async void Read_all_returns_all_events_in_repo()
    {
         //act
        var events = await _repo.ReadAll();

        //assert
        Assert.Collection(events,
            firstEvent => Assert.Equal(new EventDTO(1,"Workshop event", (new DateTime{}).ToString("yyyy-MM-dd"), "ITU", null!, new QuizDTO {Id = 1}, 4.0,  null!), firstEvent),
            SecondEvent => Assert.Equal(new EventDTO( 2, "Swagger event", (new DateTime{}).ToString("yyyy-MM-dd"), "Scrollbar", null!, new QuizDTO{ Id = 2}, 7.0,  null!), SecondEvent )
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
        var newEvent = new CreateEventDTO{
            Name = "HackIt",
            Location = "KU",
            Date = "2022-10-10"
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
        var newEvent = new CreateEventDTO{
            Name = "HackIt",
            Location = "KU",
            Date = "2022-10-10"
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
        var newEvent = new CreateEventDTO{
            Name = "HackIt",
            Location = "KU",
            Date = "2022-10-10"
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

    [Fact]
    public async void pick_one_winner_from_candidates()
    {
        var winners = await _repo.PickMultipleWinners(event1.Id, 1);

        Assert.Equal(Status.Found, winners.Item1);
        Assert.True(winners.Item2.Count() == 1);
        Assert.Contains(event1.Candidates.ToList(), candidate => candidate.Id == winners.Item2.First().Id);
    }

    [Fact]
    public async void pick_multiple_winners_correctly()
    {
        var winners = await _repo.PickMultipleWinners(event1.Id, 3);
        
        Assert.Equal(Status.Found, winners.Item1);
        Assert.True(winners.Item2.ToList().Count() == 3);
        Assert.Contains(event1.Candidates, candidate => candidate.Id == winners.Item2.ElementAt(0).Id);
        
        Assert.True(winners.Item2.ElementAt(0).Id != winners.Item2.ElementAt(1).Id && winners.Item2.ElementAt(0).Id != winners.Item2.ElementAt(2).Id && winners.Item2.ElementAt(1).Id != winners.Item2.ElementAt(2).Id);
    }

    [Fact]
    public async void winners_already_picked()
    {
        var winners1 = await _repo.PickMultipleWinners(event1.Id, 2);
        var winners2 = await _repo.PickMultipleWinners(event1.Id, 2);
        
        Assert.Equal(Status.Found, winners1.Item1);
        Assert.Equal(Status.Found, winners2.Item1);
        
        Assert.Collection(winners1.Item2,
            firstWinner => Assert.Equal(winners2.Item2.ElementAt(0), firstWinner),
            SecondWinner => Assert.Equal(winners2.Item2.ElementAt(1), SecondWinner)
        );
    }

    [Fact]
    public async void pick_winners_no_candidates_available()
    {
        var winners = await _repo.PickMultipleWinners(event2.Id, 1);
        
        Assert.Equal(Status.NotFound, winners.Item1);
        Assert.True(winners.Item2.ToList().Count() == 0);
    }

    /*[Fact]
    public async void pick_winner_returns_correct_winner()
    {
        // Act
        var winner = await _repo.pickAWinner(event1.Id);
        
        //Assert
        Assert.Equal(Status.Found, winner.Item1);
    }

    [Fact]
    public async void pick_winner_twice_returns_the_same()
    {
        //Act
        var firstWinner = await _repo.pickAWinner(event1.Id);
        var secondWinner = await _repo.pickAWinner(event1.Id);

        //Assert
        Assert.Equal(Status.Found, firstWinner.Item1);
        Assert.Equal(Status.Found, secondWinner.Item1);
        Assert.Equal(0, secondWinner.Item2.Id);
    }

    [Fact]
    public async void pick_winner_returns_notfound_no_candidates()
    {
        //Act
        var winner = await _repo.pickAWinner(event2.Id);
        
        //Assert
        Assert.Equal(Status.NotFound, winner.Item1);
    }*/

}