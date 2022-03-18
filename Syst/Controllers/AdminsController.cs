using Core;
using Microsoft.AspNetCore.Mvc;

namespace Syst.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AdminsController : ControllerBase
{
    private readonly ILogger<AdminsController> _logger;
    private IAdminRepository _repo;

    //We use REST to make sure we have a reliable API
    public AdminsController(ILogger<AdminsController> logger, IAdminRepository repo)
    {
        _logger = logger;
        //First we create our repository so we can access our CRUD operations
        _repo = repo;
    }

    
    //Return an admin given the admin id
    [ProducesResponseType(404)]
    [ProducesResponseType(typeof(AdminDTO), 200)]
    [HttpGet("{id}")]
    public async Task<ActionResult<AdminDTO>> Get(int id)
    {   
        //Using our CRUD operation to get the specified admin by id
        var res = await _repo.Read(id);
        if (res.Item1 == Status.NotFound) {
            return res.Item1.ToActionResult();
        } else {
            return new ActionResult<AdminDTO>(res.Item2);
        }
    }
}