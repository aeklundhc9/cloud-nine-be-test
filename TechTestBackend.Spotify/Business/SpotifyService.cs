using TechTestBackend.Spotify.Business.Abstraction;

namespace TechTestBackend.Spotify.Business;

public class SpotifyService : ISpotifyService
{
    public bool IdIsSpotifyLength(string id) => id.Length == 22;
}
