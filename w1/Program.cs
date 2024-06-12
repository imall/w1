using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using w1.Common;

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



        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        // builder.Services.Configure<AppsSettingsOptions>
        //     (builder.Configuration.GetSection(AppsSettingsOptions.Appsettings));

        builder.Services.AddOptions<AppsSettingsOptions>()
            .Bind(builder.Configuration.GetSection(AppsSettingsOptions.Appsettings))
            .ValidateDataAnnotations()
            .ValidateOnStart();

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