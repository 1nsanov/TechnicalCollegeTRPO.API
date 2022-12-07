using AspTestStage.Database;
using AspTestStage.Database.Domain;
using AspTestStage.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TechnicalCollegeTRPO.API.Models.Dto.Group;
using ControllerBase = TechnicalCollegeTRPO.API.BaseClasses.ControllerBase;

namespace TechnicalCollegeTRPO.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class GroupController : ControllerBase
{
    public GroupController(AppDbContext db) : base(db)
    {
    }

    [Authorize(Roles = "teacher")]
    [HttpPost("Add")]
    public async Task<IActionResult> Add([FromBody] GroupDto dto)
    {
        if (dto is null) throw new ArgumentNullException(nameof(dto));

        var entity = new Group()
        {
            Speciality = dto.Speciality,
            Number = dto.Number,
            TeacherId = dto.TeacherId,
        };

        await _db.Groups.AddAsync(entity);
        await _db.SaveChangesAsync();

        return Ok("Group is add");
    }

    [Authorize(Roles = "teacher")]
    [HttpPost("Update")]
    public async Task<IActionResult> Update([FromBody] GroupDto? dto)
    {
        var entity = GetGroup(dto);

        entity.Speciality = dto.Speciality;
        entity.Number = dto.Number;
        _db.Groups.Update(entity);
        await _db.SaveChangesAsync();

        return Ok("Group is update");
    }

    [Authorize(Roles = "teacher")]
    [HttpPost("Remove")]
    public async Task<IActionResult> Remove([FromBody] int groupId)
    {
        var entity = GetGroup(groupId);

        _db.Groups.Remove(entity);
        await _db.SaveChangesAsync();

        return Ok("Group is delete");
    }

    [Authorize]
    [HttpPost("GetByGroupId")]
    public IActionResult GetByGroupId([FromBody] int groupId)
    {
        var entity = GetGroup(groupId);
        var dto = MapToDto(entity);

        return Ok(dto);
    }

    [Authorize(Roles = "teacher")]
    [HttpPost("GetByTeacherId")]
    public IActionResult GetByTeacherId([FromBody] int teacherId)
    {
        var entity = _db.Groups.Where(g => g.TeacherId == teacherId).ToList();

        var dto = entity.ConvertAll(MapToDto);

        return Ok(dto);
    }

    [Authorize(Roles = "teacher")]
    [HttpPost("AddStudent")]
    public async Task<IActionResult> AddStudent([FromBody] GroupStudentDto dto)
    {
        var entity = GetGroupStudent(dto);

        await _db.GroupStudents.AddAsync(entity);
        await _db.SaveChangesAsync();

        return Ok("Student add to group");
    }

    [Authorize(Roles = "teacher")]
    [HttpPost("RemoveStudent")]
    public async Task<IActionResult> RemoveStudent([FromBody] GroupStudentDto dto)
    {
        var entity = GetGroupStudent(dto);

        _db.GroupStudents.Remove(entity);
        await _db.SaveChangesAsync();

        return Ok("Student remove from group");
    }


    private GroupStudent GetGroupStudent(GroupStudentDto dto)
    {
        var student = UserController.GetUserWithRole(dto.StudentId, Role.Student);
        if (student is null) throw new ArgumentException(nameof(student));

        var group = _db.Groups.FirstOrDefault(g => g.Id == dto.GroupId);
        if (group is null) throw new ArgumentException(nameof(group));

        return new GroupStudent()
        {
            GroupId = dto.GroupId,
            StudentId = dto.StudentId,
        };
    }

    private Group GetGroup(GroupDto dto)
    {
        if (dto is null) throw new ArgumentNullException(nameof(dto));

        var entity = _db.Groups.FirstOrDefault(g => g.Id == dto.Id && g.TeacherId == dto.TeacherId);

        if (entity is null) throw new ArgumentNullException(nameof(entity));

        return entity;
    }

    private Group GetGroup(int id)
    {
        var entity = _db.Groups.FirstOrDefault(g => g.Id == id);

        if (entity is null) throw new ArgumentNullException(nameof(entity));

        return entity;
    }

    public static GroupDto MapToDto(Group e)
    {
        return new GroupDto()
        {
            Id = e.Id,
            TeacherId = e.TeacherId,
            Number = e.Number,
            Speciality = e.Speciality,
        };
    }
}