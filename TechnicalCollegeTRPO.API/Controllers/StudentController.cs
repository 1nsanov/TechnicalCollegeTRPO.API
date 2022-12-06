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
        var entity = UserController.GetUserWithRole(id, Role.Student);

        return Ok(entity);
    }

    [Authorize]
    [HttpPost("GetByGroupId")]
    public IActionResult GetByGroupId([FromBody] int groupId)
    {
        var groups = _db.GroupStudents.Where(g => g.GroupId == groupId);
        if (groups is null) throw new ArgumentNullException(nameof(groups));

        var students = new List<User>();

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

        return Ok(groups);
    }

    [Authorize(Roles = "teacher")]
    [HttpPost("GetAll")]
    public IActionResult GetAll()
    {
        var roleId = RoleController.GetRoleIdByCode(Role.Student);
        var entity = _db.Users.Where(u => u.RoleId == roleId).ToList();
        var result = entity.ConvertAll(e => new UserDto()
        {
            Id = e.Id,
            Username = e.Username,
            RoleId = e.RoleId,
            Birthdate = e.Birthdate,
            Email = e.Email,
            FullName = e.FullName,
            Password = e.Password,
            Phone = e.Phone,
        });

        return Ok(result);
    }
}