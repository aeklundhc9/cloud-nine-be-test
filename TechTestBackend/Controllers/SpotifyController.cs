using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using MessagePack.Internal;

namespace TechTestBackend.Controllers;

[ApiController]
[Route("api/spotify")]
public class SpotifyController : ControllerBase
{
    private readonly SongStorageContext _songStorageContext;

    // TODO lowercase rather than camelcase
    private const string SearchTracksRouteFragment = "searchTracks";
    private const string LikeRouteFragment = "like";
    private const string RemoveLikeRouteFragment = "removeLike";
    private const string ListLikedRouteFragment = "listLiked";

    public SpotifyController(SongStorageContext songStorageContext)
    {
        _songStorageContext = songStorageContext;
    }

    [HttpGet]
    [Route(SearchTracksRouteFragment)]
    public IActionResult SearchTracks(string name)
    {
        try
        {
            var apiModel = SpotifyHelper.GetTracks(name);

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
        var track = SpotifyHelper.GetTrack(id); //check if trak exists
        if (SpotifyId(id) == false)
        {
            return StatusCode(400);
        }

        var song = new SpotifySong(); //create new song
        song.Id = id;
        song.Name = track.Name;

        try
        {
            //crashes sometimes for some reason
            // we   have to look into this
            // 😢😢😢😢
            _songStorageContext.Songs.Add(song);
            _songStorageContext.SaveChanges();
        }
        catch (Exception e)
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
        var track = SpotifyHelper.GetTrack(id);
        if (SpotifyId(id) == false)
        {
            return StatusCode(400); // bad request wrong id not existing in spotify
            // Du tänjer på mitt tålamod 😿😿
        }

        var song = new SpotifySong
        {
            Id = id
        };

        try
        {
            _songStorageContext.Songs.Remove(song); // this is not working every tume
            _songStorageContext.SaveChanges();
        }
        catch (Exception)
        {
            // TODO Allow other status codes along with FE
            // Vad fn Hannes varför får jag inte ändra API-beteendet för
            return Ok();
        }

        return Ok();
    }

    [HttpGet]
    [Route(ListLikedRouteFragment)]
    public IActionResult ListLiked()
    {
        var songCount = _songStorageContext.Songs.Count();
        var songs = new List<SpotifySong>();

        // TODO Return 400 on invalid IDs
        if (songCount > 0)
        {
            for (var i = 0; i <= songCount - 1; i++)
            {
                var songId = _songStorageContext
                    .Songs.ToList()[i].Id;

                var track = SpotifyHelper.GetTrack(songId);
                if (track.Id == null)
                {
                    // TODO: remove song from database, but not sure how
                }
                else
                {
                    // not working for some reason so we have to do the check manually for now
                    // if(SongExists(track.Id) == false)

                    int numerofsong = songs.Count();
                    for (int num = 0; num <= numerofsong; num++)
                    {
                        try
                        {
                            if (songs[num].Id == songId)
                            {
                                break;
                            }
                            else if (num == numerofsong - 1)
                            {
                                for (var id = 0; id < numerofsong; id++)
                                {
                                    if (songs[id].Name == track.Name)
                                    {
                                        break; // we dont want to add the same song twice
                                        //does this break work?
                                    }
                                    else if (id == numerofsong - 1)
                                    {
                                        songs.Add(_songStorageContext.Songs.ToList()[i]);
                                    }
                                }
                            }
                        }
                        catch (Exception e)
                        {
                            // something went wrong, but it's not important
                            // du får snart humormedalj för det här
                            songs.Add(_songStorageContext.Songs.ToList()[i]);
                        }
                    }
                }
            }
        }

        //save the changes, just in case
        // jag tar tillbaka det
        _songStorageContext.SaveChanges();

        return Ok(songs);
    }

    private static bool SpotifyId(string id) => id.Length == 22;
}
