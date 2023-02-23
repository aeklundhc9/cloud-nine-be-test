using TestTestBackend.Data.Models;

namespace TechTestBackend.Business.Abstraction;

public interface ISpotifyService
{
    bool IdIsSpotifyLength(string id);
}
