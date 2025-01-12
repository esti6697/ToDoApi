using Microsoft.EntityFrameworkCore;
using ToDoApi;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

//הגדרת DB Context
builder.Services.AddDbContext<ToDoDbContext>(options =>
    options.UseMySql(builder.Configuration.GetConnectionString("ToDoDB"),
    ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("ToDoDB"))));

// builder.Services.AddAuthentication(options =>
// {
//     options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
//     options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
// }).AddJwtBearer(options =>
// {
//     options.TokenValidationParameters = new TokenValidationParameters
//     {
//         ValidateIssuer = true,
//         ValidateAudience = true,
//         ValidateLifetime = true,
//         ValidateIssuerSigningKey = true,
//         ValidIssuer = builder.Configuration["JwtSettings:Issuer"],
//         ValidAudience = builder.Configuration["JwtSettings:Audience"],
//         IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JwtSettings:SecretKey"]))
//     };
// });

// הוספת CORS
builder.Services.AddCors(options =>
{
        options.AddPolicy("http://localhost:3000/", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

// הוספת Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// הגדרת Swagger Middleware
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "ToDo API v1");
        c.RoutePrefix = string.Empty; // Swagger ייפתח בכתובת הבסיס
    });
}

// app.UseAuthentication();
// app.UseAuthorization();
app.UseCors("http://localhost:3000/");

// [Authorize]
app.MapGet("/tasks", async (ToDoDbContext dbContext) =>
{
    var tasks = await dbContext.Items.ToListAsync();
    return Results.Ok(tasks);
});

// [Authorize]
app.MapPost("/tasks", async (Item newItem, ToDoDbContext dbContext) =>
{
    await dbContext.Items.AddAsync(newItem);
    await dbContext.SaveChangesAsync();
    return Results.Created($"/tasks/{newItem.Id}", newItem);
});

// [Authorize]
app.MapPut("/tasks/{id}", async (int id, [FromBody] bool IsComplete ,ToDoDbContext dbContext) =>
{

    var existingItem = await dbContext.Items.FindAsync(id);
    if (existingItem == null)
    {
        return Results.NotFound($"Task with ID {id} not found.");
    }

    existingItem.IsComplete = IsComplete;
    dbContext.Items.Update(existingItem);
    await dbContext.SaveChangesAsync();
    return Results.Ok(existingItem);
});

// [Authorize]
app.MapDelete("/tasks/{id}", async (int id, ToDoDbContext dbContext) =>
{
    var existingItem = await dbContext.Items.FindAsync(id);
    if (existingItem == null)
    {
        return Results.NotFound($"Task with ID {id} not found.");
    }
    dbContext.Items.Remove(existingItem);
    await dbContext.SaveChangesAsync();
    return Results.Ok($"Task with ID {id} deleted successfully.");
});

app.Run();
