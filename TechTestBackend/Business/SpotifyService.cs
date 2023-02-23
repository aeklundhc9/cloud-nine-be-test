using TechTestBackend.Business.Abstraction;

namespace TechTestBackend.Business;

public class SpotifyService : ISpotifyService
{
    public bool IdIsSpotifyLength(string id) => id.Length == 22;
}
