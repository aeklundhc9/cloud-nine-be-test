using TestTestBackend.Data.Models;

namespace TechTestBackend.Business.Abstraction;

public interface ISpotifyApiClient
{
    Task<IEnumerable<SpotifySong>> SearchForSongsByName(string name);
    Task<SpotifySong?> GetSong(string id);
}
