using AspTestStage.BaseClasses;
using AspTestStage.Database;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using TechnicalCollegeTRPO.API.Models.Dto;

namespace TechnicalCollegeTRPO.API.BaseClasses;

public abstract class ControllerBase : Controller 
{
    internal static AppDbContext _db;

    protected ControllerBase(AppDbContext db)
    {
        _db = db;
    }
}