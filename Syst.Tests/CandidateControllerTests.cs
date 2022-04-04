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


	[Fact]
	public void Get_all_returns_all_candidates() {
		throw new NotImplementedException()
	}

	[Fact]
	public async void Get_existing_id_return_Candidate() {
		throw new NotImplementedException()
	}

	[Fact]
	public async void Get_non_existing_id_return_NotFound() {
		throw new NotImplementedException()
	}

	[Fact]
	public async void Post_adds_candidate_to_repository() {
		throw new NotImplementedException()
	}

	[Fact]
	public async void Post_existing_id_returns_StatusConflict() {
		throw new NotImplementedException()
	}

	[Fact]
	public async void Delete_non_existing_id_return_NotFound() {
		throw new NotImplementedException()
	}

	[Fact]
	public async void Delete_existing_id_return_NoContent() {
		throw new NotImplementedException()
	}

}