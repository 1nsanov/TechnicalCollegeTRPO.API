using AspTestStage.Database;
using AspTestStage.Database.Domain;
using Microsoft.AspNetCore.Mvc;

namespace AspTestStage.BaseClasses;

public class ControllerBase : Controller
{
    internal readonly AppDbContext _db;

    public ControllerBase(AppDbContext db)
    {
        _db = db;
    }
}