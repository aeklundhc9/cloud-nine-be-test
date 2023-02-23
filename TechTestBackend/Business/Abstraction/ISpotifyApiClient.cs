using TestTestBackend.Data.Models;

namespace TechTestBackend.Business.Abstraction;

public interface ISpotifyApiClient
{
    IEnumerable<SpotifySong> GetTracks(string name);
    SpotifySong? GetTrack(string id);
}
