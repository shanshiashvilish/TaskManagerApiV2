using TaskManagerApiV2.Application.Extensions;
using TaskManagerApiV2.BackgroundJobs;
using TaskManagerApiV2.Persistence.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler =
            System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
    });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add application services
builder.Services.AddHostedService<TaskReassignmentBackgroundService>();
builder.Services.AddApplicationServices();
builder.Services.AddDatabase();
builder.Services.AddRepositories();
builder.Services.AddSeederServices();

var app = builder.Build();

// DB seeding logic
using (var scope = app.Services.CreateScope())
{
    var seeder = scope.ServiceProvider.GetRequiredService<Seeder>();
    seeder.Seed();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler();
    app.UseHsts();
}

app.UseHttpsRedirection();
app.MapControllers();

app.Run();