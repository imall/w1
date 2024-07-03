using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using w1.Common;
using w1.Models;
using w1.Models.dto;

namespace w1.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{
    private const int SomeNumber = 1;

    private static readonly string[] Summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };
    
    private readonly ContosoUniversityContext _context;
    private readonly IConfiguration _config;

    public WeatherForecastController(
        ContosoUniversityContext context,
        IConfiguration config)
    {
        _context = context;
        _config = config;
    }

    [HttpGet("root",Name = "GetWeathe")]
    public ActionResult<IEnumerable<WeatherForecast>> Get()
    {
        return Enumerable.Range(1, 5).Select(index => new WeatherForecast
                         {
                             Date = DateTime.Now.AddDays(index),
                             TemperatureC = Random.Shared.Next(-20, 55),
                             Summary = Summaries[Random.Shared.Next(Summaries.Length)]
                         })
                         .ToArray();
    }

    [HttpGet("GetSomeKey")]
    public string GetSomekey()
    {
        return _config.GetValue<string>("Somekey")!;
    }



    [HttpGet(Name = "GetWeatherForecast")]
    public IActionResult Get(string? q)
    {
        //var data = context.Courses.Where(x => x.CourseId > someNumber && x.Title.Contains("J"));
        var data = from c in _context.Courses
                   join d in _context.Departments on c.DepartmentId equals d.DepartmentId
                   where c.CourseId > SomeNumber && d.StartDate.Date == DateTime.Parse("2015-03-21")
                   select new CourseVeiwModel()
                   {
                       CourseId = c.CourseId,
                       Title = c.Title,
                       Credits = c.Credits,
                       DepartmentName = d.Name,
                       DepartmentDate = d.StartDate
                   };

        // 如果 q 有值，判斷 title 起始或結束於Q
        if (!string.IsNullOrEmpty(q))
        {
            data = data.Where(x => x.Title.StartsWith(q) || x.Title.EndsWith(q));
        }

        return Ok(data);
    }


    [HttpGet("somekey")]
    public IActionResult GetStudent(string? q)
    {
        var data = from c in _context.MyDeptCourses
                   select c;
        if (!string.IsNullOrEmpty(q))
        {
            data = data.Where(x => x.Title.StartsWith(q) || x.Title.EndsWith(q));
        }

        return Ok(data);
    }

    [HttpGet("GetStudentUseStoreProcedure")]
    public async Task<IActionResult> GetStudentUseStoreProcedure(string? q)
    {
        var data = await _context.GetProcedures().GetMyDeptCoursesAsync(q);

        return Ok(data);
    }


    [HttpGet("GetCourseWithPerson")]
    public ActionResult<IEnumerable<MyPersion>> GetCourseWithPerson(string? q)
    {
        var data = _context.Courses.Include(c => c.Instructors)
            .SelectMany(c => c.Instructors, (c, i) => new MyPersion
            {
                Id = i.Id,
                LastName = i.LastName,
                FirstName = i.FirstName,
                Discriminator = i.Discriminator
            });

        return Ok(data);
    }

    [HttpGet("GetStudentUseRawSql")]
    public async Task<IActionResult> GetStudentUseRawSql(string? q)
    {
        // 用 rawsql 的方法實現 GetStudentUseStoreProcedure
        var query = $"""
            SELECT [c].[CourseID] AS [CourseId], [c].[Title], [c].[Credits], [d].[Name] AS [DepartmentName], [d].[StartDate] AS [DepartmentDate]
            FROM [Course] AS [c]
            INNER JOIN [Department] AS [d] ON [c].[DepartmentID] = [d].[DepartmentID]
            """;

        var data = await _context.MyDeptCourses.FromSql($""" 
                       SELECT [c].[CourseID] AS [CourseId], [c].[Title], [c].[Credits], [d].[Name] AS [DepartmentName], [d].[StartDate] AS [DepartmentDate]
                       FROM [Course] AS [c]
                       INNER JOIN [Department] AS [d] ON [c].[DepartmentID] = [d].[DepartmentID]
                       """).ToListAsync();

        return Ok(data);
    }
}