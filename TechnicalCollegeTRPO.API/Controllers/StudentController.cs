using AspTestStage.Database;
using AspTestStage.Database.Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AspTestStage.BaseClasses;

[ApiController]
[Route("api/[controller]")]
public class StudentController : ControllerBase
{
    private UserController UserController { get; set; }
    public StudentController(AppDbContext db, UserController userController) : base(db)
    {
        UserController = userController;
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

    [Authorize]
    [HttpPost("GetByTeacherId")]
    public IActionResult GetByTeacherId([FromBody] int teacherId)
    {
        var groups = _db.Groups.Where(g => g.TeacherId == teacherId).ToList();
        if (groups is null) throw new ArgumentNullException(nameof(groups));

        return Ok(groups);
    }
}