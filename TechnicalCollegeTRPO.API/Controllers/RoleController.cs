using AspTestStage.Database;
using AspTestStage.Database.Domain;
using Microsoft.AspNetCore.Mvc;

namespace AspTestStage.BaseClasses;

[ApiController]
[Route("api/[controller]")]
public class RoleController : ControllerBase
{
    public RoleController(AppDbContext db) : base(db)
    { }

    [HttpPost("Add")]
    public async Task<IActionResult> Add([FromBody] Role? newRole)
    {
        if (newRole is null) BadRequest();

        var entity = new Role()
        {
            Code = newRole.Code,
            Name = newRole.Name
        };

        await _db.Roles.AddAsync(entity);
        await _db.SaveChangesAsync();

        return Ok(entity);
    }

    [HttpGet("GetList")]
    public async Task<IActionResult> GetList()
    {
        var entity = _db.Roles.ToList();

        return Ok(entity);
    }
}