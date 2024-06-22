using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;
using TalkStream_API.Database;
using TalkStream_API.Entities;
using TalkStream_API.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAuthorization();
builder.Services.AddAuthentication(IdentityConstants.ApplicationScheme)
    .AddCookie(IdentityConstants.ApplicationScheme)
    .AddBearerToken(IdentityConstants.BearerScheme);


builder.Services.AddIdentityCore<User>()
    .AddEntityFrameworkStores<AppDbContext>()
    .AddApiEndpoints();

builder.Services.AddDbContext<AppDbContext>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("Database");
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString), optionsBuilder => 
            optionsBuilder.SchemaBehavior(MySqlSchemaBehavior.Ignore) );
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.ApplyMigrations();
}

app.UseAuthentication();
app.UseAuthorization();


app.MapGet("users/me", async(ClaimsPrincipal claims, AppDbContext dbContext) =>
{
    var userId = claims.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value;
    return await dbContext.Users.FindAsync(userId); 
}).RequireAuthorization();

app.UseHttpsRedirection();

app.MapIdentityApi<User>();

app.MapControllers();

app.Run(); 