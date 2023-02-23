using Microsoft.EntityFrameworkCore;

namespace TechTestBackend;

public class SongStorageContext : DbContext
{
    public SongStorageContext(DbContextOptions<SongStorageContext> options)
        : base(options)
    { }

    public DbSet<SpotifySong> Songs { get; set; }
}
