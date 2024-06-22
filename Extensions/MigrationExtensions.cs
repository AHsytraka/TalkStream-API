using Microsoft.EntityFrameworkCore;
using TalkStream_API.Database;

namespace TalkStream_API.Extensions;

public static class MigrationExtensions
{
    public static void ApplyMigrations(this IApplicationBuilder app)
    {
        using IServiceScope scope = app.ApplicationServices.CreateScope();
        using AppDbContext context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        
        context.Database.Migrate();
    }
}