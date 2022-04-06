using Xunit;
using Syst.Controllers;
using Moq;
using Core;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace Syst.Tests;

public class AdminsControllerTests {

    //Objects to use for testing
    static readonly AdminDTO admin1 = new AdminDTO { 
        Id = 1,
        Name = "Iulia",
        Email = "Iulia@systematic.dk",
        Events = null!
    };


    [Fact]
    public async void Get_existing_id_return_Admin() {
        //Arrange
        var logger = new Mock<ILogger<AdminsController>>();
        var repository = new Mock<IAdminRepository>();
        var expected = admin1;
        repository.Setup(m => m.Read(1)).ReturnsAsync((Status.Found, expected));
        var controller = new AdminsController(logger.Object, repository.Object);

        //Act
        var response = await controller.Get(1);

        //Assert
        Assert.Equal(expected, response.Value);
    }

    [Fact]
    public async void Get_non_existing_id_return_NotFound()
    {
        //Arrange
        var logger = new Mock<ILogger<AdminsController>>();
        var repository = new Mock<IAdminRepository>();
        repository.Setup(m => m.Read(99)).ReturnsAsync((Status.NotFound, default(AdminDTO)));
        var controller = new AdminsController(logger.Object, repository.Object);

        //Act
        var response = await controller.Get(99);

        //Assert
        Assert.IsType<NotFoundResult>(response.Result);
    }
}