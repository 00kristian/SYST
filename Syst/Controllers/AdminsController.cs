using Core;
using Microsoft.AspNetCore.Mvc;

namespace Syst.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AdminsController : ControllerBase
{
    private readonly ILogger<AdminsController> _logger;
    private IAdminRepository _repo;

    public AdminsController(ILogger<AdminsController> logger, IAdminRepository repo)
    {
        _logger = logger;
        _repo = repo;
    }


    [ProducesResponseType(404)]
    [ProducesResponseType(typeof(AdminDTO), 200)]
    [HttpGet("{id}")]
    public async Task<ActionResult<AdminDTO>> Get(int id)
    {
        var res = await _repo.Read(id);
        if (res.Item1 == Status.NotFound) {
            return res.Item1.ToActionResult();
        } else {
            return new ActionResult<AdminDTO>(res.Item2);
        }
    }
}