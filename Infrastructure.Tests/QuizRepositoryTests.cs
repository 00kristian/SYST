using Xunit;
using Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.Sqlite;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;


namespace Infrastructure.Tests;

public class QuizRepositoryTests {
    private readonly ISystematicContext _context; 
    private readonly IQuizRepository _repo;

    Quiz quiz1 = new Quiz{Id = 1, Date = new DateTime{}, Questions = new List<Question>{}, Events = new List<Event>{}, Candidates = new List<Candidate>{}};
    Quiz quiz2 = new Quiz{Id = 2, Date = new DateTime{}, Questions = new List<Question>{}, Events = new List<Event>{}, Candidates = new List<Candidate>{}};

    public QuizRepositoryTests(){
        var connection = new SqliteConnection("Filename=:memory:");
        connection.Open();
        var builder = new DbContextOptionsBuilder<SystematicContext>();
        builder.UseSqlite(connection);
        var context = new SystematicContext(builder.Options);
        context.Database.EnsureCreated();
        
        context.Quizes.AddRange(
            quiz1,
            quiz2
        );

 
        context.SaveChanges();
        _context = context;
        _repo = new QuizRepository(_context);
    }

    [Fact]
    public async void Create_Creates_Quiz_In_Repository()
    {
        //Arrange
        var quiz3 = new QuizDTO{Id = 3, Date = new DateTime{}, Questions = new List<QuestionDTO>{}, Events = new List<EventDTO>{}, Candidates = new List<CandidateDTO>{}};

        //Act
        var actual = await _repo.Create(quiz3);

        //Assert
        Assert.Equal(Status.Created, actual.Item1);
        Assert.Equal(3, actual.Item2);
    }

    [Fact]
    public async void Create_Returns_Conflict_When_ID_Is_In_the_database()
    {
        //Arrange
        var quiz3 = new QuizDTO{Id = 1, Date = new DateTime{}, Questions = new List<QuestionDTO>{}, Events = new List<EventDTO>{}, Candidates = new List<CandidateDTO>{}};


        //Act
        var actual = await _repo.Create(quiz3);

        //Assert
        Assert.Equal(Status.Conflict, actual.Item1);
        Assert.Equal(1, actual.Item2);
    }

    [Fact]
    public async void Read_returns_quiz1_when_given_ID_1()
    {
        //act
        var  actual = await _repo.Read(1);

        //assert
        
        Assert.Equal(Status.Found, actual.Item1);
        Assert.Equal(quiz1.Id, actual.Item2.Id);
    }

    [Fact]
    public async void Read_returns_status_notFound_when_given_nonexisting_ID()
    {
        //act
        var actual = await _repo.Read(22);
        //assert
        Assert.Equal(Status.NotFound, actual.Item1);
        Assert.Equal(default(QuizDTO), actual.Item2);
    }

    [Fact]
    public async void Read_all_returns_quizes_in_repo()
    {
        // act
        var quizes = await _repo.ReadAll(); 

        //assert
        Assert.Collection(quizes,
            quiz => Assert.Equal(new QuizDTO(1, new DateTime{}, null!, null!, null!), quiz),
            quiz => Assert.Equal(new QuizDTO(2, new DateTime{}, null!, null!, null!), quiz)
        );

    }

    [Fact]
    public async void Update_returns_status_Updated_when_given_existing_id()
    {
        // Arrange
        var newQuiz = new QuizDTO{
            Id = 5
        };
    
        // Act
        var actual = await _repo.Update(1, newQuiz);
    
        // Assert
        Assert.Equal(Status.Updated, actual);
    }

    [Fact]
    public async void Update_returns_not_found_when_given_nonexisiting_idAsync()
    {
         var newQuiz = new QuizDTO{ 
            Id=52
        };
    
        // Act
        var actual = await _repo.Update(42, newQuiz);
    
        // Assert
        Assert.Equal(Status.NotFound, actual);
    }

    [Fact]
    public async void Delete_deletes_quiz2_given_id_2()
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
}