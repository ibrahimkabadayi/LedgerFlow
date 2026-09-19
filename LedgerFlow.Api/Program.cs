using LedgerFlow.Infrastructure;
using LedgerFlow.Infrastructure.Persistence.Scripts;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddInfrastructure();

var app = builder.Build();

try
{
    using var scope = app.Services.CreateScope();
    var dbSetup = scope.ServiceProvider.GetRequiredService<DatabaseSetup>();
    await dbSetup.EnsureCreated();
}
catch (Exception ex)
{
    app.Logger.LogWarning(ex, "Could not initialize LedgerFlow database automatically on startup.");
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
