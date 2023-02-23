using Microsoft.EntityFrameworkCore;
using TestTestBackend.Data.Models;
using TestTestBackend.Data.Repositories.Abstraction;

namespace TestTestBackend.Data.Repositories;

public class SongStorageRepository : ISongStorageRepository
{
    private readonly SongStorageContext _songStorageContext;

    public SongStorageRepository(SongStorageContext songStorageContext)
    {
        _songStorageContext = songStorageContext;
    }

    public async Task AddSongToLikes(SpotifySong song)
    {
        if (_songStorageContext.Songs.Any(x => x.Id.Equals(song.Id)))
            throw new ArgumentException($"Song with ID {song.Id} is already liked", nameof(song));
        
        _songStorageContext.Songs.Add(song);
        await _songStorageContext.SaveChangesAsync();
    }

    public async Task RemoveSongFromLikes(string id)
    {
        var likedSong = await _songStorageContext.Songs.SingleOrDefaultAsync(x => x.Id.Equals(id));
        if (likedSong == null)
            return;
        
        _songStorageContext.Songs.Remove(likedSong);
        await _songStorageContext.SaveChangesAsync();
    }

    public async Task<IEnumerable<SpotifySong>> GetLikedSongs()
    {
        return await _songStorageContext.Songs.ToListAsync();
    }
}
