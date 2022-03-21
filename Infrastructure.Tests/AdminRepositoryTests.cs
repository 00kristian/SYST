using Xunit;
using Core;
using System.Collections.Generic;
using System;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Tests;

public class AdminRepositoryTests
{

    private readonly ISystematicContext _context; 
    private readonly IAdminRepository _repo; 
    Admin admin1 = new Admin{
        Id = 1, 
        Name = "Røde",
        Email = "RødeErKærestekedelig@altid.com",
        Events = new List<Event>{}
    };

    Admin admin2 = new Admin{
        Id = 2, 
        Name = "Lukas",
        Email = "LukasErSyg@godbedring.net",
        Events = new List<Event>(){}
    };

    public AdminRepositoryTests(){
        var connection = new SqliteConnection("Filename=:memory:");
        connection.Open();
        var builder = new DbContextOptionsBuilder<SystematicContext>();
        builder.UseSqlite(connection);
        var context = new SystematicContext(builder.Options);
        context.Database.EnsureCreated();
        
        context.Admins.AddRange(
            admin1,
            admin2
        );

 
        context.SaveChanges();
        _context = context;
        _repo = new AdminRepository(_context);
    }


    [Fact]
    public async void Create_Creates_New_Admin_In_Repository()
    {
        //Arrange
        var newAdmin = new AdminDTO{
            Id = 3, 
            Name = "Maj",
            Email = "Maj@minecraft.net",
            Events = new List<EventDTO>(){}
        };

        //Act
        var actual = await _repo.Create(newAdmin);

        //Assert
        Assert.Equal(Status.Created, actual.Item1);
        Assert.Equal(3, actual.Item2);
    }

    [Fact]
    public async void Create_Returns_Conflict_When_Id_Is_Same()
    {
        //Arrange
        var newAdmin = new AdminDTO{
            Id = 1, 
            Name = "Maj",
            Email = "Maj@minecraft.net",
            Events = new List<EventDTO>(){}
        };

        //Act
        var actual = await _repo.Create(newAdmin);

        //Assert
        Assert.Equal(Status.Conflict, actual.Item1);
        Assert.Equal(1, actual.Item2);
    }

    [Fact]
    public async void Update_Updates_Admin_In_Repository()
    {
        //Arrange
        var newAdmin = new AdminDTO{
            Id = 1, 
            Name = "Maj",
            Email = "Maj@minecraft.net",
            Events = new List<EventDTO>(){}
        };

        //Act
        var actual = await _repo.Update(1, newAdmin);

        //Assert
        Assert.Equal(Status.Updated, actual);
    }

    [Fact]
    public async void Update_returns_notFound_when_looking_for_admin_not_in_database()
    {
        //Arrange
        var newAdmin = new AdminDTO{
            Id = 5, 
            Name = "Maj",
            Email = "Maj@minecraft.net",
            Events = new List<EventDTO>(){}
        };

        //Act
        var actual = await _repo.Update(5, newAdmin);

        //Assert
        Assert.Equal(Status.NotFound, actual);
    }

    [Fact]
    public async void Delete_deletes_admin_with_id_1()
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