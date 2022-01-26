using Microsoft.EntityFrameworkCore;
using TestNet6;
using SqlCommenter;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddSqlCommenter();
builder.Services.AddDbContext<TestContext>(
    (p,o) => 
        o.UseNpgsql("Host=localhost;Database=sqlcommenter;Username=postgres")
            .UseSqlCommenter(p));

var app = builder.Build();

app.MapGet("/", () => "Hello World!");
app.MapControllers();
app.Run();