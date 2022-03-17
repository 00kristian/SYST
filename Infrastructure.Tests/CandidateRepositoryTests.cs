using System.Collections.Generic;
using Core;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Infrastructure.Tests;

public class CandidateRepositoryTests{

    private readonly ISystematicContext _context; 
    private readonly ICandidateRepository _repo;

     Candidate candidate1 = new Candidate {Id=1, Name = "Lukas Hjelmstrand", Email = "luhj@itu.dk", StudyProgram = "Bsc i Softwareudvikling", University = UniversityEnum.ITU};
     Candidate candidate2 = new Candidate {Id=2, Name = "Rene Dif", Email = "rene@dif.dk", StudyProgram = "Msc i Vand", University = UniversityEnum.CBS};

     public CandidateRepositoryTests(){
        var connection = new SqliteConnection("Filename=:memory:");
        connection.Open();
        var builder = new DbContextOptionsBuilder<SystematicContext>();
        builder.UseSqlite(connection);
        var context = new SystematicContext(builder.Options);
        context.Database.EnsureCreated();
        
        context.Candidates.AddRange(
            candidate1,
            candidate2
        );

 
        context.SaveChanges();
        _context = context;
        _repo = new CandidateRepository(_context);
    }

    [Fact]
    public async void Create_Creates_New_Candidate_In_Repository()
    {
        //Arrange
        var candidate1 = new CandidateDTO {Name = "Oscar", Email = "Eng@itu.dk", StudyProgram = "Bsc i League of Legends", University = UniversityEnum.ITU};

        //Act
        var actual = await _repo.Create(candidate1);

        //Assert
        Assert.Equal(Status.Created, actual.Item1);
        Assert.Equal(3, actual.Item2);
    }

  /*   [Fact]
    public async void Create_Returns_Conflict_When_Name_Is_In_the_database()
    {
        //Arrange
        

        //Act
        var actual = await _repo.Create(newAdmin);

        //Assert
        Assert.Equal(Status.Conflict, actual.Item1);
        Assert.Equal(1, actual.Item2);
    }
 */
    [Fact]
    public async void ReadNameFromID_returns_Rene_Dif_given_id_2()
    {

        //Act
        var actual = await _repo.ReadNameFromId(2);

        //Assert
        Assert.Equal(Status.Found, actual.Item1);
        Assert.Equal("Rene Dif", actual.Item2);
    }

    [Fact]
    public async void ReadNameFromID_returns_Status_notFound_when_given_non_existing_id()
    {

        //Act
        var actual = await _repo.ReadNameFromId(42);

        //Assert
        Assert.Equal(Status.NotFound, actual.Item1);
        Assert.Equal(null, actual.Item2);
    }

    [Fact]
    public async void Read_returns_Candidate1_when_giving_id_1()
    {
        //act 

        var actual = await _repo.Read(1);
        //assert

        Assert.Equal(Status.Found, actual.Item1);
        Assert.Equal(candidate1.Name, actual.Item2.Name);
        Assert.Equal(candidate1.Email, actual.Item2.Email);
        Assert.Equal(candidate1.StudyProgram, actual.Item2.StudyProgram);
        Assert.Equal(candidate1.University, actual.Item2.University);
    }

     [Fact]
    public async void Read_returns_Status_notfound_when_giving_non_existing_id()
    {
        //act 

        var actual = await _repo.Read(42);
        //assert

        Assert.Equal(Status.NotFound, actual.Item1);
        Assert.Equal(default(CandidateDTO), actual.Item2);
      
    }

    
    [Fact]
    public async void ReadAll_returns_all_candidates()
    {
        //act
        var candidates = await _repo.ReadAll();

        //assert
        Assert.Collection(candidates,
            candidate => Assert.Equal(new CandidateDTO(1,"Lukas Hjelmstrand", "luhj@itu.dk", "Bsc i Softwareudvikling", UniversityEnum.ITU,  null!, new QuizDTO { }), candidate),
            candidate => Assert.Equal(new CandidateDTO(2, "Rene Dif", "rene@dif.dk", "Msc i Vand", UniversityEnum.CBS, null!, new QuizDTO { }), candidate)
        );
    }


    [Fact]
    public async void Update_returns_notFound_when_looking_for_admin_not_in_database()
    {
        //Arrange
        var newCandidate = new CandidateDTO{
            Id = 5, 
            Name = "Maj",
            Email = "Maj@minecraft.net",
            Events = new List<EventDTO>(){}
        };

        //Act
        var actual = await _repo.Update(5, newCandidate);

        //Assert
        Assert.Equal(Status.NotFound, actual);
    }

    [Fact]
    public async void Update_Updates_Candidate1_In_Repository()
    {
        //Arrange
        var newCandidate = new CandidateDTO{
            Id = 1, 
            Name = "Gardal",
            Email = "G@hejsa.net",
            Events = new List<EventDTO>(){}
        };

        //Act
        var actual = await _repo.Update(1, newCandidate);

        //Assert
        Assert.Equal(Status.Updated, actual);
    }


    [Fact]
    public async void Delete_deletes_candidates_with_id_1()
    {

        //Act
        var actual = await _repo.Delete(1);

        //Assert
        Assert.Equal(Status.Deleted, actual);
    }

    [Fact]
    public async void Delete_returns_notFound_when_trying_to_delete_nonexisting_id()
    {

        //Act
        var actual = await _repo.Delete(42);

        //Assert
        Assert.Equal(Status.NotFound, actual);
    }


}