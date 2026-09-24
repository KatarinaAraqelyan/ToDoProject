using ToDoProject.Data;
using ToDoProject.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IToDoService, ToDoService>();
builder.Services.AddSingleton<IToDoService, InMemoryToDoService>();


bool isInMemory = builder.Configuration.GetValue<bool>("UseInMemoryToDo");

    if (isInMemory)
    {
        builder.Services.AddSingleton<IToDoService, InMemoryToDoService>();
    } else
    {
        builder.Services.AddDbContext<ToDoContext>();
        builder.Services.AddScoped<IToDoService, ToDoService>();
    }

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ToDoContext>();
    dbContext.Database.EnsureDeleted();
    dbContext.Database.EnsureCreated();
}

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.MapControllers();

app.Run();