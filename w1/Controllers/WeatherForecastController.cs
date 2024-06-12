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
    private const int someNumber = 1;

    private static readonly string[] Summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    private readonly ILogger<WeatherForecastController> _logger;
    private readonly IOptions<AppsSettingsOptions> _options;
    private readonly ContosoUniversityContext context;

    public WeatherForecastController(
        ILogger<WeatherForecastController> logger,
        IOptions<AppsSettingsOptions> options,
        ContosoUniversityContext context)
    {
        _logger = logger;
        _options = options;
        this.context = context;
    }


    [HttpGet(Name = "GetWeatherForecast")]
    public IActionResult Get(string? q)
    {
        //var data = context.Courses.Where(x => x.CourseId > someNumber && x.Title.Contains("J"));
        var data = from c in context.Courses
                   join d in context.Departments on c.DepartmentId equals d.DepartmentId
                   where c.CourseId > someNumber && d.StartDate.Date == DateTime.Parse("2015-03-21")
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
        var data = from c in context.MyDeptCourses
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
        var data = await context.GetProcedures().GetMyDeptCoursesAsync(q);

        return Ok(data);
    }


    [HttpGet("GetCourseWithPerson")]
    public ActionResult<IEnumerable<MyPersion>> GetCourseWithPerson(string? q)
    {
        var data = context.Courses.Include(c => c.Instructors)
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

        var data = await context.MyDeptCourses.FromSql($""" 
                       SELECT [c].[CourseID] AS [CourseId], [c].[Title], [c].[Credits], [d].[Name] AS [DepartmentName], [d].[StartDate] AS [DepartmentDate]
                       FROM [Course] AS [c]
                       INNER JOIN [Department] AS [d] ON [c].[DepartmentID] = [d].[DepartmentID]
                       """).ToListAsync();

        return Ok(data);
    }
}