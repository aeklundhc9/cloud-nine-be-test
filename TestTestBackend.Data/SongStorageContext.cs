using Microsoft.EntityFrameworkCore;
using TestTestBackend.Data.Models;

namespace TestTestBackend.Data;

public class SongStorageContext : DbContext
{
#pragma warning disable CS8618
    public SongStorageContext(DbContextOptions<SongStorageContext> options)
        : base(options)
    { }
#pragma warning restore CS8618

    public DbSet<SpotifySong> Songs { get; set; }
}
