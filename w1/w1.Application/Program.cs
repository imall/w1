using Microsoft.EntityFrameworkCore;
using w1.Common;
using w1.Models;

namespace w1;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);


        // Console.WriteLine(student.GetStudent());

        // Add services to the container.

        builder.Services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

        builder.Services.AddDbContext<ContosoUniversityContext>(options =>
        {
            options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            options.EnableSensitiveDataLogging();
        });

        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        // builder.Services.Configure<AppsSettingsOptions>
        //     (builder.Configuration.GetSection(AppsSettingsOptions.Appsettings));

        builder.Services.AddOptions<AppsSettingsOptions>()
               .Bind(builder.Configuration.GetSection(AppsSettingsOptions.Appsettings))
               .ValidateDataAnnotations() // 取值時驗證
               .ValidateOnStart(); // 啟動時驗證

        var app = builder.Build();

        // Configure the HTTP request pipeline.

        app.UseSwagger();
        app.UseSwaggerUI();

        app.UseHttpsRedirection();

        app.UseAuthorization();


        app.MapControllers();

        app.Run();
    }
}