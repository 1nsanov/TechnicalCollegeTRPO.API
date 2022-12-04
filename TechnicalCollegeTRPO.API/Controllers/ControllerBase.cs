using AspTestStage.Database;
using Microsoft.AspNetCore.Mvc;

namespace AspTestStage.BaseClasses;

public class ControllerBase : Controller
{
    internal readonly AppDbContext _db;

    public ControllerBase(AppDbContext db)
    {
        _db = db;
    }

    protected async void SaveDb() => await _db.SaveChangesAsync();
}