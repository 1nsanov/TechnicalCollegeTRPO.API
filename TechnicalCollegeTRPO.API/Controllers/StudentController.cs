using AspTestStage.BaseClasses;
using AspTestStage.Database;
using AspTestStage.Database.Domain;
using AspTestStage.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ControllerBase = TechnicalCollegeTRPO.API.BaseClasses.ControllerBase;

namespace TechnicalCollegeTRPO.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class StudentController : ControllerBase
{
    public StudentController(AppDbContext db) : base(db)
    {
    }

    [Authorize]
    [HttpPost("GetById")]
    public IActionResult GetById([FromBody] int id)
    {
        var dto = UserController.GetUserWithRole(id, Role.Student);

        return Ok(dto);
    }

    [Authorize]
    [HttpPost("GetByGroupId")]
    public IActionResult GetByGroupId([FromBody] int groupId)
    {
        var groups = _db.GroupStudents.Where(g => g.GroupId == groupId);
        if (groups is null) throw new ArgumentNullException(nameof(groups));

        var students = new List<UserDto>();

        foreach(var group in groups)
        {
            var student = UserController.GetUserWithRole(group.StudentId, Role.Student);
            students.Add(student);
        }

        return Ok(students);
    }

    [Authorize(Roles = "teacher")]
    [HttpPost("GetByTeacherId")]
    public IActionResult GetByTeacherId([FromBody] int teacherId)
    {
        var groups = _db.Groups.Where(g => g.TeacherId == teacherId).ToList();
        if (groups is null) throw new ArgumentNullException(nameof(groups));

        var dto = groups.ConvertAll(GroupController.MapToDto);

        return Ok(dto);
    }

    [Authorize(Roles = "teacher")]
    [HttpGet("GetAll")]
    public IActionResult GetAll()
    {
        var roleId = RoleController.GetRoleIdByCode(Role.Student);
        var entity = _db.Users.Where(u => u.RoleId == roleId).ToList();
        var result = entity.ConvertAll(UserController.MapToDto);

        return Ok(result);
    }
}