var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((context, loggerConfig) => loggerConfig.ReadFrom.Configuration(context.Configuration));

builder.Services.AddApplication()
                .AddPresentation()
                .AddInfrastructure();

builder.Services.AddEndpoints();

var app = builder.Build();

app.UseHttpsRedirection();

app.MapEndpoints();

app.UseMiddlewares();

app.UseSerilogRequestLogging();

app.UseExceptionHandler();

//app.UseAuthentication();
//app.UseAuthorization();

await app.RunAsync();

public partial class Program;


