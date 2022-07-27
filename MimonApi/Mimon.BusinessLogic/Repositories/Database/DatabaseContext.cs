using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Mimon.BusinessLogic.Repositories.Photos;
using Mimon.BusinessLogic.Repositories.Reactions;
using Mimon.BusinessLogic.Repositories.Users;
using Mimon.BusinessLogic.Repositories.UsersRelations;

namespace Mimon.BusinessLogic.Repositories.Database;

public sealed class DatabaseContext : DbContext
{
    public DatabaseContext(
        DbContextOptions<DatabaseContext> options,
        IOptions<DatabaseOptions> dbOptionsAccessor
    ) : base(options)
    {
        this.options = dbOptionsAccessor.Value;
        Database.EnsureCreated();
    }
        
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql(options.ConnectionString);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // составной ключ для связи челиков
        // уникальной записью считается пара из двух челиков
        modelBuilder.Entity<RelationStorageElement>()
            .HasKey(x => new { x.UserId, x.FriendId });
    }

    public DbSet<UserStorageElement> UsersStorage { get; set; }
    public DbSet<PhotoStorageElement> PhotosStorage { get; set; }
    public DbSet<RelationStorageElement> RelationsStorage { get; set; }
    public DbSet<ReactionStorageElement> ReactionsStorage { get; set; }

    private readonly DatabaseOptions options;
}