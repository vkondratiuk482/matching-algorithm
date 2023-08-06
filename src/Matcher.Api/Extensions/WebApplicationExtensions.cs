using Matcher.Data;
using Microsoft.EntityFrameworkCore;

namespace Matcher.Api.Extensions;

public static class WebApplicationExtensions
{
    public static WebApplication ApplyMigrations(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();

        var context = scope.ServiceProvider.GetRequiredService<ApplicationContext>();
        
        context.Database.Migrate();

        return app;
    }
}
