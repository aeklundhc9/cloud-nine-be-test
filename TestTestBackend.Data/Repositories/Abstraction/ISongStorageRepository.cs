using TestTestBackend.Data.Models;

namespace TestTestBackend.Data.Repositories.Abstraction;

public interface ISongStorageRepository
{
    Task AddSongToLikes(SpotifySong song);
    Task RemoveSongFromLikes(string id);
    Task<IEnumerable<SpotifySong>> GetLikedSongs();
}
