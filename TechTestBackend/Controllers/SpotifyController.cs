using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using TechTestBackend.Business.Abstraction;
using TestTestBackend.Data;
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
    public IActionResult SearchTracks(string name)
    {
        try
        {
            var apiModel = _spotifyApiClient.GetTracks(name);

            return Ok(apiModel);
        }
        catch (Exception)
        {
            // TODO Tell frontisar to accept better status codes
            return NotFound();
        }
    }

    [HttpPost]
    [Route(LikeRouteFragment)]
    public IActionResult Like(string id)
    {
        if (!_spotifyService.IdIsSpotifyLength(id))
            return BadRequest();

        var track = _spotifyApiClient.GetTrack(id);
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
            _songStorageRepository.AddSongToLikes(song);
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
