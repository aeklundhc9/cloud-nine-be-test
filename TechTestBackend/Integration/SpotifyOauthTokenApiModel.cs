using Newtonsoft.Json;
#pragma warning disable CS8618

namespace TechTestBackend.Integration;

public class SpotifyOauthTokenApiModel
{
    [JsonProperty("access_token")]
    public string AccessToken { get; set; }
}
