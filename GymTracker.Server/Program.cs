using GymTracker.Server.DatabaseConnection;
using GymTracker.Server.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddDbContext<GymTrackerContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IWorkoutManager, WorkoutManager>();
builder.Services.AddScoped<IRoutineManager, RoutineManager>();
builder.Services.AddScoped<IWorkoutDayManager, WorkoutDayManager>();
builder.Services.AddScoped<IExerciseManager, ExerciseManager>();
builder.Services.AddScoped<IBodyPartManager, BodyPartManager>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});

var app = builder.Build();

app.UseDefaultFiles();
app.UseStaticFiles();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowAll");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.MapFallbackToFile("/index.html");

// Explicitly set the URLs
app.Urls.Add("https://localhost:7175");
app.Urls.Add("http://localhost:5175");

app.Run();
