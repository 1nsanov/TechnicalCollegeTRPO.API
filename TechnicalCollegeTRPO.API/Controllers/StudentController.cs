namespace AspTestStage.BaseClasses;

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
        var roleId = GetRoleIdByCode(Role.Student);
        var entity = _db.Users.FirstOrDefault(u => u.Id == id && u.RoleId == roleId);

        if (entity is null) throw new ArgumentNullException(nameof(entity));

        return Ok(entity);
    }

    [Authorize]
    [HttpPost("GetByGroupId")]
    public IActionResult GetByGroupId([FromBody] int groupId)
    {
        var groups = _db.GroupStudents.Where(g => g.GroupId == groupId);
        if (groups is null) throw new ArgumentNullException(nameof(groups));

        var students = new List<Student>();
        var roleStudentId = GetRoleIdByCode(Role.Student);

        foreach(group in groups)
        {
            var student = _db.Users.FirstOrDefault(u => u.Id == group.StudentId && u.RoleId == roleStudentId);
            if (student is not null) students.Add(student);
        }

        return Ok(entity);
    }

    [Authorize]
    [HttpPost("GetByTeacherId")]
    public IActionResult GetByTeacherId([FromBody] int teacherId)
    {

    }

    //TODO: Перенести в RoleController
    private int GetRoleIdByCode(string code)
    {
        var role = _db.Roles.FirstOrDefault(u => u.Code == code);
        if (role is null) throw new ArgumentNullException(nameof(role));

        return role.Id;
    }
}