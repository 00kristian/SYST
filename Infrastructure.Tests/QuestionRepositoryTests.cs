using Xunit;
using Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.Sqlite;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Infrastructure.Tests;

public class QuestionsRepositoryTests {

    private readonly ISystematicContext _context; 
    private readonly IQuestionRepository _repo;

    Question question1 = new Question{Id = 1,Representation="I am not sure", Answer="Virtual Dispatching", ImageURL="", Options=new List<string>{"Java", "F#", "Virtual Dispatching","Golang"}, Quiz= new Quiz{}}; 

    Question question2 = new Question{Id = 2,Representation="This one is about hockey", Answer="Gretzky", ImageURL="", Options=new List<string>{"Jaromir", "Ovechkin", "Gretzky","Eller"}, Quiz= new Quiz{}}; 

    public QuestionsRepositoryTests(){
        var connection = new SqliteConnection("Filename=:memory:");
        connection.Open();
        var builder = new DbContextOptionsBuilder<SystematicContext>();
        builder.UseSqlite(connection);
        var context = new SystematicContext(builder.Options);
        context.Database.EnsureCreated();
        
        context.Questions.AddRange(
            question1,
            question2
        );

 
        context.SaveChanges();
        _context = context;
        _repo = new QuestionRepository(_context, "");
        
    }



    
    [Fact]
    public async void Create_Creates_Question_In_Repository()
    {
        //Arrange
        var question3 = new CreateQuestionDTO{Representation="What's the difference between ... ", Answer="Not everything"};

        //Act
        var actual = await _repo.Create(question3);

        //Assert
        Assert.Equal(Status.Created, actual.Item1);
        Assert.Equal(3, actual.Item2);
    }

    // [Fact]
    // public async void Create_Returns_Conflict_When_ID_Is_In_the_database()
    // {
    //     //Arrange
    //     var question3 = new CreateQuestionDTO{Representation="What's the difference between ... ", Answer="Not everything"};


    //     //Act
    //     var actual = await _repo.Create(question3);

    //     //Assert
    //     Assert.Equal(Status.Conflict, actual.Item1);
    //     Assert.Equal(1, actual.Item2);
    // }


    
    [Fact]
    public async void Read_returns_question1_when_given_ID_1()
    {
        //act
        var actual = await _repo.Read(1);

        //assert
        Assert.Equal(Status.Found, actual.Item1);
        Assert.Equal(question1.Representation, actual.Item2.Representation);
        Assert.Equal(question1.Answer, actual.Item2.Answer);
        Assert.Equal(question1.Id, actual.Item2.Id);
        Assert.Equal(question1.ImageURL, actual.Item2.ImageURl);
    }

    [Fact]
    public async void Read_returns_status_notFound_when_given_nonexisting_ID()
    {
        //act
        var actual = await _repo.Read(22);
        //assert
        Assert.Equal(Status.NotFound, actual.Item1);
        Assert.Equal(default(QuestionDTO), actual.Item2);
    }


    [Fact]
    public async void Read_all_returns_questions_in_repo()
    {
        // act
        var questions = await _repo.ReadAll(); 

        //assert
        Assert.Collection(questions,
            question => Assert.Equal(new QuestionDTO(1, "I am not sure", "Virtual Dispatching", "", question.Options), question),
            question => Assert.Equal(new QuestionDTO(2, "This one is about hockey", "Gretzky", "", question.Options), question)
        );

    }

    [Fact]
    public async void Update_returns_status_Updated_when_given_existing_id()
    {
        // Arrange
        var newQuestion = new CreateQuestionDTO{
            Representation="New string",
            Answer="This is the new answer"
        };
    
        // Act
        var actual = await _repo.Update(1, newQuestion);
    
        // Assert
        Assert.Equal(Status.Updated, actual);
    }

    
    [Fact]
    public async void Update_returns_not_found_when_given_nonexisiting_idAsync()
    {
         var newQuestion = new CreateQuestionDTO{ 
            Representation="New string",
            Answer="This is the new answer"
        };
    
        // Act
        var actual = await _repo.Update(42, newQuestion);
    
        // Assert
        Assert.Equal(Status.NotFound, actual);
    }

    [Fact]
    public async void Delete_deletes_question2_given_id_2()
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

