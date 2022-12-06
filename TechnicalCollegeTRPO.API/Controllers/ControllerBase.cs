using AspTestStage.Database;
using Microsoft.AspNetCore.Mvc;

namespace TechnicalCollegeTRPO.API.BaseClasses;

public class ControllerBase : Controller
{
    internal static AppDbContext _db;

    public ControllerBase(AppDbContext db)
    {
        _db = db;
    }
}