using Microsoft.AspNetCore.Mvc;
using TechTestBackend.Spotify.Business.Abstraction;
using TestTestBackend.Data.Models;
using TestTestBackend.Data.Repositories.Abstraction;

namespace TechTestBackend.Controllers;

[ApiController]
[Route("api/spotify")]
public class SpotifyController : ControllerBase
{
    private readonly ISpotifyService _spotifyService;
    private readonly ISpotifyApiClient _spotifyApiClient;
    private readonly ISongStorageRepository _songStorageRepository;

    // TODO lowercase rather than camelcase
    private const string SearchTracksRouteFragment = "searchTracks";
    private const string LikeRouteFragment = "like";
    private const string RemoveLikeRouteFragment = "removeLike";
    private const string ListLikedRouteFragment = "listLiked";

    public SpotifyController(ISpotifyService spotifyService,
        ISpotifyApiClient spotifyApiClient,
        ISongStorageRepository songStorageRepository)
    {
        _spotifyService = spotifyService;
        _spotifyApiClient = spotifyApiClient;
        _songStorageRepository = songStorageRepository;
    }

    [HttpGet]
    [Route(SearchTracksRouteFragment)]
    public async Task<IActionResult> SearchTracks(string name)
    {
        try
        {
            var apiModels = await _spotifyApiClient.SearchForSongsByName(name);

            return Ok(apiModels);
        }
        catch (Exception)
        {
            // TODO Tell frontisar to accept better status codes
            return NotFound();
        }
    }

    [HttpPost]
    [Route(LikeRouteFragment)]
    public async Task<IActionResult> Like(string id)
    {
        if (!_spotifyService.IdIsSpotifyLength(id))
            return BadRequest();

        var track = await _spotifyApiClient.GetSong(id);
        if (track == null)
        {
            // TODO Return 400
            return Ok();
        }

        try
        {
            var song = new SpotifySong
            {
                Id = id,
                Name = track.Name
            };
            await _songStorageRepository.AddSongToLikes(song);
        }
        catch (ArgumentException)
        {
            // not sure if this is the best way to handle this
            // It ain't, but we're keeping it for compatibility for now
            return Ok();
        }

        return Ok();
    }

    [HttpPost]
    [Route(RemoveLikeRouteFragment)]
    public IActionResult RemoveLike(string id)
    {
        if (!_spotifyService.IdIsSpotifyLength(id))
            return BadRequest();

        _songStorageRepository.RemoveSongFromLikes(id);

        return Ok();
    }

    [HttpGet]
    [Route(ListLikedRouteFragment)]
    public async Task<IActionResult> ListLiked()
    {
        var likedSongs = await _songStorageRepository.GetLikedSongs();
        return Ok(likedSongs);
    }
}
