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

    Quiz quiz1 = new Quiz{Id = 1, Name="Lukki", Questions = new List<Question>{}, Events = new List<Event>{}, Candidates = new List<Candidate>{}};
    Quiz quiz2 = new Quiz{Id = 2, Name = "Sals", Questions = new List<Question>{}, Events = new List<Event>{}, Candidates = new List<Candidate>{}};

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
        _repo = new QuizRepository(_context, "");
    }

    [Fact]
    public async void Create_Creates_Quiz_In_Repository()
    {
        //Arrange
        var quiz3 = new QuizCreateDTO{Name="Gandalf"};

        //Act
        var actual = await _repo.Create(quiz3);

        //Assert
        Assert.Equal(Status.Created, actual.Item1);
        Assert.Equal(3, actual.Item2);
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
            quiz => Assert.Equal(new QuizDTO(1, "Lukki", null!, null!, null!), quiz),
            quiz => Assert.Equal(new QuizDTO(2, "Sals", null!, null!, null!), quiz)
        );

    }

    [Fact]
    public async void Update_returns_status_Updated_when_given_existing_id()
    {
        // Arrange
        var newQuiz = new QuizCreateDTO{
            Name = "Soulja Boy"
        };
    
        // Act
        var actual = await _repo.Update(1, newQuiz);
    
        // Assert
        Assert.Equal(Status.Updated, actual);
    }

    [Fact]
    public async void Update_returns_not_found_when_given_nonexisiting_idAsync()
    {
         var newQuiz = new QuizCreateDTO{ 
            Name = "52"
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

    [Fact]
    public async void Clone_clones_quiz_from_original() {
        //arrange
        var question1 = new Question{Representation="I am not sure", Answer="Virtual Dispatching", ImageURL="555", Options=new List<string>{"Java", "F#", "Virtual Dispatching","Golang"}};
        var question2 = new Question{Representation="This one is about hockey", Answer="Gretzky", ImageURL="", Options=new List<string>{"Jaromir", "Ovechkin", "Gretzky","Eller"}};
        _context.Questions.Add(question1);
        _context.Questions.Add(question2);
        var quiz_1 = new Quiz(){Name="OG", Questions = new List<Question>{
            question1, question2
        }, Events = new List<Event>{}, Candidates = new List<Candidate>{}};
        var quiz_2 = new Quiz() {Name="Clone", Questions = new List<Question>{}, Events = new List<Event>{}, Candidates = new List<Candidate>{}};
        _context.Quizes.Add(quiz_1);
        _context.Quizes.Add(quiz_2);
        await _context.SaveChangesAsync();

        //act
        var status = await _repo.Clone(quiz_2.Id, quiz_1.Id);

        //assert
        Assert.Equal(Status.Updated, status);
        Assert.Equal("OG", quiz_2.Name);
        Assert.Collection(quiz_2.Questions,
            question => {
                Assert.Equal("I am not sure", question.Representation);
                Assert.Equal("Virtual Dispatching", question.Answer);
                Assert.Equal("555", question.ImageURL);
                Assert.Collection(question.Options, 
                    option => Assert.Equal("Java", option),
                    option => Assert.Equal("F#", option),
                    option => Assert.Equal("Virtual Dispatching", option),
                    option => Assert.Equal("Golang", option)
                );
            },
            question => {
                Assert.Equal("This one is about hockey", question.Representation);
                Assert.Equal("Gretzky", question.Answer);
                Assert.Equal("", question.ImageURL);
                Assert.Collection(question.Options, 
                    option => Assert.Equal("Jaromir", option),
                    option => Assert.Equal("Ovechkin", option),
                    option => Assert.Equal("Gretzky", option),
                    option => Assert.Equal("Eller", option)
                );
            }
        );
    }
}