using Xunit;
using Syst.Controllers;
using Moq;
using Core;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace Syst.Tests;

public class CandidateControllerTests
{
	//Objects to use for testing
	static readonly QuizDTO quiz1 = new QuizDTO
	{
		Id = 20,
		Name = "Systematic Festival Quiz",
		Questions = null!,
		Events = null!,
		Candidates = null!
	};
	static readonly CandidateDTO candidate1 = new CandidateDTO {
		Id = 1,
		Name = "Bob Jensen",
		Email = "bobj@dtu.dk",
		StudyProgram = "Datalogi",
		University = "DTU",
		GraduationDate = "20-06-2024",
		Events = null!,
		Quiz = quiz1,
		IsUpvoted = false
	};
	static readonly CandidateDTO candidate2 = new CandidateDTO
	{
		Id = 2,
		Name = "Ib Hansen",
		Email = "ibha@ruc.dk",
		StudyProgram = "Informatik",
		University = "RUC",
		GraduationDate = "27-05-2023",
		Events = null!,
		Quiz = quiz1,
		IsUpvoted = false
	};



	//Testing starts here
	[Fact]
	public void Get_all_returns_all_candidates() {
		//Arrange
		var logger = new Mock<ILogger<CandidatesController>>();
		var repository = new Mock<ICandidateRepository>();
		var candidates = new List<CandidateDTO> { candidate1, candidate2 };
		repository.Setup(m => m.ReadAll()).ReturnsAsync(candidates);
		var controller = new CandidatesController(logger.Object, repository.Object);

		//Act
		var response = controller.Get();

		//Assert
		Assert.Equal(candidates, response.Result);
	}

	[Fact]
	public async void Get_existing_id_return_Candidate() {
		//Arrange
		var logger = new Mock<ILogger<CandidatesController>>();
		var repository = new Mock<ICandidateRepository>();
		var expected = candidate1;
		repository.Setup(m => m.Read(1)).ReturnsAsync((Status.Found, expected));
		var controller = new CandidatesController(logger.Object, repository.Object);

		//Act
		var response = await controller.Get(1);

		//Assert
		Assert.Equal(expected, response.Value);
	}

	[Fact]
	public async void Get_non_existing_id_return_NotFound() {
		//Arrange
		var logger = new Mock<ILogger<CandidatesController>>();
		var repository = new Mock<ICandidateRepository>();
		repository.Setup(m => m.Read(99)).ReturnsAsync((Status.NotFound, default(CandidateDTO)));
		var controller = new CandidatesController(logger.Object, repository.Object);

		//Act
		var response = await controller.Get(99);

		//Assert
		Assert.IsType<NotFoundResult>(response.Result);
	}

	[Fact]
	public async void Post_adds_candidate_to_repository() {
		//Arrange
		var logger = new Mock<ILogger<CandidatesController>>();
		var repository = new Mock<ICandidateRepository>();
		var candidate3 = new CreateCandidateDTO(3, "Hanne Nielsen", "hani@itu.dk","BSc" , "SWU", "ITU", "25-01-2023", true);
		var candidates = new List<CreateCandidateDTO> { candidate3 };
		var createdCandidate = new CreateCandidateDTO(4, "Sanne Pedersen", "sape@itu.dk","BSc",  "GBI", "ITU", "25-01-2023", false);
		repository.Setup(m => m.Create(createdCandidate)).Callback(() => candidates.Add(createdCandidate));
		var controller = new CandidatesController(logger.Object, repository.Object);

		//Act
		var response = await controller.Post(createdCandidate);

		//Assert
		Assert.IsType<CreatedAtActionResult>(response);
		Assert.Equal(2, candidates.Count);
	}

	[Fact]
	public async void Post_existing_id_returns_StatusConflict() {
		//Arrange
		var logger = new Mock<ILogger<CandidatesController>>();
		var repository = new Mock<ICandidateRepository>();
		var candidates = new List<CandidateDTO> {candidate1};
		var createdCandidate = new CreateCandidateDTO(1, "Sanne Pedersen", "sape@itu.dk", "BSc", "GBI", "ITU", "25-01-2023", false);
		repository.Setup(m => m.Create(createdCandidate)).ReturnsAsync(() => (Status.Conflict, 1));
		var controller = new CandidatesController(logger.Object, repository.Object);

		//Act
		var response = await controller.Post(createdCandidate);

		//Assert
		Assert.IsType<ConflictObjectResult>(response);
		Assert.Equal(1, candidates.Count);
	}

	[Fact]
	public async void Post_null_DTO_returns_StatusConflict()
	{
		//Arrange
		var logger = new Mock<ILogger<CandidatesController>>();
		var repository = new Mock<ICandidateRepository>();
		var newCandidate = default(CreateCandidateDTO);
		repository.Setup(m => m.Create(newCandidate)).ReturnsAsync(() => (Status.Conflict, 0));
		var controller = new CandidatesController(logger.Object, repository.Object);

		//Act
		var response = await controller.Post(newCandidate);

		//Assert
		Assert.IsType<ConflictObjectResult>(response);
	}

	[Fact]
	public async void Delete_non_existing_id_return_NotFound() {
		//Arrange
		var logger = new Mock<ILogger<CandidatesController>>();
		var repository = new Mock<ICandidateRepository>();
		repository.Setup(m => m.Delete(90)).ReturnsAsync(Status.NotFound);
		var controller = new CandidatesController(logger.Object, repository.Object);

		//Act
		var response = await controller.Delete(90);

		//Assert
		Assert.IsType<NotFoundObjectResult>(response);
	}

	[Fact]
	public async void Delete_existing_id_return_NoContent() {
		//Arrange
		var logger = new Mock<ILogger<CandidatesController>>();
		var repository = new Mock<ICandidateRepository>();
		repository.Setup(m => m.Delete(1)).ReturnsAsync(Status.Deleted);
		var controller = new CandidatesController(logger.Object, repository.Object);

		//Act
		var response = await controller.Delete(1);

		//Assert
		Assert.IsType<NoContentResult>(response);
	}

}