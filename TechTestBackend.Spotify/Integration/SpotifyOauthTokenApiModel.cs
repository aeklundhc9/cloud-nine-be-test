using Newtonsoft.Json;

#pragma warning disable CS8618

namespace TechTestBackend.Spotify.Integration;

public class SpotifyOauthTokenApiModel
{
    [JsonProperty("access_token")]
    public string AccessToken { get; set; }
}
