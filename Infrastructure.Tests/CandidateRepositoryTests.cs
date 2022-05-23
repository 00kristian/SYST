using Xunit;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Core;
using SQLitePCL;

namespace Infrastructure.Tests;

public class CandidateRepositoryTests
{

    private readonly ISystematicContext _context;
    private readonly ICandidateRepository _repo;

    Candidate candidate1 = new Candidate {Id = 1, Name = "Lukas Hjelmstrand", Email = "luhj@itu.dk", CurrentDegree = "BSc", StudyProgram = "Softwareudvikling", University = "IT-University of Copenhagen", GraduationDate = new DateTime { }, IsUpvoted = false, Created = new DateTime(2022, 05, 30)};
    Candidate candidate2 = new Candidate {Id = 2, Name = "Maj Frost Jensen", Email = "mfje@itu.dk", CurrentDegree = "MSc", StudyProgram = "Computer Science", University = "Copenhagen Business School", GraduationDate = new DateTime { }, IsUpvoted = true, Created = new DateTime(2022, 05, 30)};
    Candidate candidate3 = new Candidate {Id = 3, Name = "Frida Pagels", Email = "frip@ku.dk", CurrentDegree = "BSc", StudyProgram = "Law", University = "University of Copenhagen", GraduationDate = new DateTime { }, IsUpvoted = true, Created = new DateTime(2020, 04, 02)};

    public CandidateRepositoryTests()
    {
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
        var candidate1 = new CreateCandidateDTO
        {
            Name = "Oscar Nielsen", Email = "osni@itu.dk", CurrentDegree = "BSc", StudyProgram = "Datalogi",
            University = "ITU", GraduationDate = (new DateTime { }).ToString("yyyy-MM-dd"), IsUpvoted = true
        };
        var candidate2 = new CreateCandidateDTO
        {
            Name = "Maj Frost Jensen", Email = "mfje@itu.dk", CurrentDegree = "MSc", StudyProgram = "Computer Science",
            University = "CBS", GraduationDate = (new DateTime { }).ToString("yyyy-MM-dd"), IsUpvoted = true
        };

        //Act
        var actual1 = await _repo.Create(candidate1);
        var actual2 = await _repo.Create(candidate2);

        //Assert
        Assert.Equal(Status.Created, actual1.Item1);
        Assert.Equal(3, actual1.Item2);

        Assert.Equal(Status.Created, actual2.Item1);
        Assert.Equal(4, actual2.Item2);
    }

    [Fact]
    public void Valid_email_returns_true()
    {
        var email1 = "isabellahamgnusdottir@gmail.com";
        var email2 = "imag@itu.dk";
        var email3 = "swagster@itu.dk";
        var email4 = "gustavmsvensson@ku.dk";
        var email5 = "coco@hotmail.com";
        var email6 = "lilo@sdu.dk";
        var email7 = "sasse@cbs.dk";
        var email8 = "berlin@yahoo.dk";
        var email9 = "julemanden@nordpolen.dk";
        var email10 = "sarahcc@dtu.dk";

        var actual1 = _repo.IsValid(email1);
        var actual2 = _repo.IsValid(email2);
        var actual3 = _repo.IsValid(email3);
        var actual4 = _repo.IsValid(email4);
        var actual5 = _repo.IsValid(email5);
        var actual6 = _repo.IsValid(email6);
        var actual7 = _repo.IsValid(email7);
        var actual8 = _repo.IsValid(email8);
        var actual9 = _repo.IsValid(email9);
        var actual10 = _repo.IsValid(email10);

        Assert.True(actual1);
        Assert.True(actual2);
        Assert.True(actual3);
        Assert.True(actual4);
        Assert.True(actual5);
        Assert.True(actual6);
        Assert.True(actual7);
        Assert.True(actual8);
        Assert.True(actual9);
        Assert.True(actual10);
    }

    [Fact]
    public void Valid_email_returns_false()
    {
        var email1 = "isabellahamgnusdottiratgmail.com";
        var email2 = "@itu.dk";
        var email3 = "coco@@hotmail.com";
        var email4 = "fghjklæ.,mnbvfgsyduiovklgfenws@sdu.dk";
        var email5 = "wertyujhgbfed";

        var actual1 = _repo.IsValid(email1);
        var actual2 = _repo.IsValid(email2);
        var actual3 = _repo.IsValid(email3);
        var actual4 = _repo.IsValid(email4);
        var actual5 = _repo.IsValid(email5);

        Assert.False(actual1);
        Assert.False(actual2);
        Assert.False(actual3);
        Assert.False(actual4);
        Assert.False(actual5);
    }

    [Fact]
    public async void ReadNameFromID_returns_Maj__Frost_Jensen_given_id_2()
    {

        //Act
        var actual = await _repo.ReadNameFromId(2);

        //Assert
        Assert.Equal(Status.Found, actual.Item1);
        Assert.Equal("Maj Frost Jensen", actual.Item2);
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
        var actual = await _repo.Read(1);

        Assert.Equal(Status.Found, actual.Item1);
        Assert.Equal(candidate1.Name, actual.Item2.Name);
        Assert.Equal(candidate1.Email, actual.Item2.Email);
        Assert.Equal(candidate1.CurrentDegree, actual.Item2.CurrentDegree);
        Assert.Equal(candidate1.StudyProgram, actual.Item2.StudyProgram);

        Assert.Equal(candidate1.University, actual.Item2.University);
        Assert.Equal(candidate1.GraduationDate.ToString("yyyy-MM"), actual.Item2.GraduationDate);
        Assert.Equal(candidate1.IsUpvoted, actual.Item2.IsUpvoted);

    }

    [Fact]
    public async void Read_returns_Status_notfound_when_giving_non_existing_id()
    {
        var actual = await _repo.Read(42);

        Assert.Equal(Status.NotFound, actual.Item1);
        Assert.Equal(default(CandidateDTO), actual.Item2);

    }

    [Fact]
    public async void Deletes_old_candidates_successfully()
    {
        var oldCandidate = new Candidate
        {
            Name = "Filippa", Email = "flip@ku.dk", CurrentDegree = "BSc", StudyProgram = "Veterinary Medicine",
            University = "KU", GraduationDate = new DateTime { }, IsUpvoted = false,
            Created = new DateTime(2020, 05, 22)
        };

        _context.Candidates.Add(oldCandidate);

        _context.SaveChanges();

        Assert.Contains(oldCandidate, _context.Candidates);

        await _repo.DeleteOldCandidates();

        Assert.DoesNotContain(oldCandidate, _context.Candidates);
    }

    [Fact]
    public async void ReadAll_returns_all_candidates()
    {
        //act
        var candidates = await _repo.ReadAll();

        //assert
        Assert.Collection(candidates,

            candidate =>
                Assert.Equal(
                    new CandidateDTO(1, "Lukas Hjelmstrand", "luhj@itu.dk", "BSc", "Softwareudvikling", "IT-University of Copenhagen",
                        (new DateTime { }).ToString("yyyy-MM"), 0.0, null!, new QuizDTO { }, false,
                        new DateTime(2022, 05, 30)), candidate),
            candidate => Assert.Equal(new CandidateDTO(2, "Maj Frost Jensen", "mfje@itu.dk", "MSc", "Computer Science",
                "Copenhagen Business School", (new DateTime { }).ToString("yyyy-MM"), 0.0, null!, new QuizDTO { }, true,
                new DateTime(2022, 05, 30)), candidate)

        );
    }


    [Fact]
    public async void Update_returns_notFound_when_looking_for_admin_not_in_database()
    {
        //Arrange
        var newCandidate = new CandidateDTO
        {
            Id = 5,
            Name = "Camille Gonnsen",
            Email = "cgon@itu.dk",
            Events = new List<EventDTO>() { },
            GraduationDate = new DateTime { }.ToString("yyyy-MM-dd"),
            IsUpvoted = true
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
        var newCandidate = new CandidateDTO
        {
            Id = 1,
            Name = "Sarah Christiansen",
            Email = "sacc@itu.dk",
            Events = new List<EventDTO>() { },
            University = "ITU",
            GraduationDate = new DateTime { }.ToString("yyyy-MM-dd"),
            IsUpvoted = true
        };

        //Act
        var actual = await _repo.Update(1, newCandidate);

        //Assert
        Assert.Equal(Status.Updated, actual);
    }


    [Fact]
    public void Update_upvote_correctly()
    {
        var actual = _repo.UpdateUpVote(candidate1.Id);

        Assert.True(candidate1.IsUpvoted);
        Assert.Equal(Status.Updated, actual.Result);
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


    [Fact]
    public async void Deletes_correct_candidates_when_expired()
    {
        _context.Candidates.AddRange(
            candidate3
        );
        await _context.SaveChangesAsync();

        await _repo.DeleteOldCandidates();

        Assert.DoesNotContain(_context.Candidates, c => c.Id == candidate3.Id);
    }

    [Fact]
    public async void Deletes_no_candidates_as_non_expired()
    {
        await _repo.DeleteOldCandidates();

        Assert.Contains(_context.Candidates, c => c.Id == candidate1.Id);
        Assert.Contains(_context.Candidates, c => c.Id == candidate2.Id);
    }

    [Fact]
    public async void add_answer_cannot_find_quiz()
    {
        var answers = new AnswersDTO(9, 1, 1, new[] {"Yes", "No", "Above 5"});

        var actual = await _repo.AddAnswer(candidate1.Id, answers);

        Assert.Equal(Status.NotFound, actual);
    }

    [Fact]
    public async void add_answers_cannot_find_candidate()
    {
        var actual = await _repo.AddAnswer(100, new AnswersDTO());

        Assert.Equal(Status.NotFound, actual);
    }

    [Fact]
    public async void add_answers_successfully_updates_candidate_and_correct_calc_score()
    {
        var event1 = new Event
        {
            Name = "ITU Match Making", Date = new DateTime { }, Location = "Scrollbar",
            Candidates = new List<Candidate> { }, Quiz = new Quiz(), Rating = 7.0, Winners = new List<Candidate>()
        };
        var quiz1 = new Quiz
        {
            Id = 1, Name = "Difficult quiz", Questions = new List<Question> { }, Events = new List<Event> { },
            Candidates = new List<Candidate> { }
        };
        event1.Quiz = quiz1;
        _context.Events.Add(event1);
        _context.Quizes.Add(quiz1);
        var question1 = new Question
        {
            Id = 1, Representation = "I am not sure", Answer = "Virtual Dispatching", ImageURL = "",
            Options = new List<string> {"Java", "F#", "Virtual Dispatching", "Golang"}, Quiz = quiz1
        };
        var question2 = new Question
        {
            Id = 2, Representation = "This one is about hockey", Answer = "Gretzky", ImageURL = "",
            Options = new List<string> {"Jaromir", "Ovechkin", "Gretzky", "Eller"}, Quiz = quiz1
        };
        _context.Questions.AddRange(question1, question2);
        _context.SaveChanges();

        var answer = new AnswersDTO(1, quiz1.Id, event1.Id, new[] {"Java", "Gretzky"});

        var actual = await _repo.AddAnswer(candidate1.Id, answer);

        Assert.Equal(Status.Updated, actual);
        Assert.Equal(50, candidate1.PercentageOfCorrectAnswers);
    }

    [Fact]
    public async void returns_graph_data_correctly()
    {
        var candidate4 = new Candidate()
        {
            Id = 6, Name = "Isa", Email = "isa@itu.dk", CurrentDegree = "PhD",
            StudyProgram = "Computer Science", University = "IT-University of Copenhagen",
            GraduationDate = new DateTime { }, IsUpvoted = true,
            Created = new DateTime(2022, 05, 30)
        };

        _context.Candidates.AddRange(candidate3, candidate4);
        _context.SaveChanges();

        var uniList = new List<string>
        {
            "Aalborg University",
            "Aarhus University",
            "Copenhagen Business School",
            "IT-University of Copenhagen",
            "Roskilde University",
            "Technical University of Denmark",
            "University of Copenhagen",
            "University of Southern Denmark"
        };

        var actual = await _repo.GraphData(uniList);

        Assert.Equal(0, actual[0]);
        Assert.Equal(0, actual[1]);
        Assert.Equal(1, actual[2]);
        Assert.Equal(2, actual[3]);
        Assert.Equal(0, actual[4]);
        Assert.Equal(0, actual[5]);
        Assert.Equal(1, actual[6]);
        Assert.Equal(0, actual[7]);
        Assert.Equal(0, actual[8]);
    }

    [Fact]
    public async void returns_graph_data_correctly_tests_others()
    {
        var candidate5 = new Candidate()
        {
            Id = 6, Name = "Isa", Email = "isa@itu.dk", CurrentDegree = "PhD",
            StudyProgram = "Computer Science", University = "Other",
            GraduationDate = new DateTime { }, IsUpvoted = true,
            Created = new DateTime(2022, 05, 30)
        };
        var candidate6 = new Candidate()
        {
            Id = 5, Name = "Isa", Email = "isa@itu.dk", CurrentDegree = "PhD",
            StudyProgram = "Computer Science", University = "Other",
            GraduationDate = new DateTime { }, IsUpvoted = true,
            Created = new DateTime(2022, 05, 30)
        };

        _context.Candidates.AddRange(candidate5, candidate6);
        _context.SaveChanges();

        var uniList = new List<string>
        {
            "Aalborg University",
            "Aarhus University",
            "Copenhagen Business School",
            "IT-University of Copenhagen",
            "Roskilde University",
            "Technical University of Denmark",
            "University of Copenhagen",
            "University of Southern Denmark"
        };

        var actual = await _repo.GraphData(uniList);

        Assert.Equal(0, actual[0]);
        Assert.Equal(0, actual[1]);
        Assert.Equal(1, actual[2]);
        Assert.Equal(1, actual[3]);
        Assert.Equal(0, actual[4]);
        Assert.Equal(0, actual[5]);
        Assert.Equal(0, actual[6]);
        Assert.Equal(0, actual[7]);
        Assert.Equal(2, actual[8]);
    }

    [Fact]
    public async void candidate_distribution_returns_correct_data()
    {
        candidate1.PercentageOfCorrectAnswers = 25;
        candidate2.University = "IT-University of Copenhagen";
        candidate3.University = "IT-University of Copenhagen";
        candidate2.PercentageOfCorrectAnswers = 25;
        candidate3.PercentageOfCorrectAnswers = 50;
        _context.Candidates.Add(candidate3);
        _context.SaveChanges();

        var actual = await _repo.CandidateDistribution("IT-University of Copenhagen");
        
        Assert.Equal("IT-University of Copenhagen", actual.Name);
        Assert.True(actual.answerRates.Length == 2);
        Assert.True(actual.distribution.Length == 2);
        
        Assert.Equal(25, actual.answerRates[0]);
        Assert.Equal(2, actual.distribution[0]);
        
        Assert.Equal(50, actual.answerRates[1]);
        Assert.Equal(1, actual.distribution[1]);
    }

}