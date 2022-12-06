using AspTestStage.Database;
using ControllerBase = TechnicalCollegeTRPO.API.BaseClasses.ControllerBase;

namespace TechnicalCollegeTRPO.API.Controllers;

public class RoleController : ControllerBase
{
    public RoleController(AppDbContext db = null) : base(db)
    {
    }

    public static int GetRoleIdByCode(string code)
    {
        var role = _db.Roles.FirstOrDefault(u => u.Code == code);
        if (role is null) throw new ArgumentNullException(nameof(role));

        return role.Id;
    }
}