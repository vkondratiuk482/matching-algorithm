using Matcher.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Matcher.Data;

public class ApplicationContext : DbContext
{
    public DbSet<ProfileEntity> Profiles => Set<ProfileEntity>();

    public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
    {
    }
}
