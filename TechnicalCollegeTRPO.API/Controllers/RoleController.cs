using AspTestStage.Database;

namespace AspTestStage.BaseClasses;

public class RoleController : ControllerBase
{
    public RoleController(AppDbContext db) : base(db)
    {
    }

    public int GetRoleIdByCode(string code)
    {
        var role = _db.Roles.FirstOrDefault(u => u.Code == code);
        if (role is null) throw new ArgumentNullException(nameof(role));

        return role.Id;
    }
}