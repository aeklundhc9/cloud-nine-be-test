using System.Net.Http.Headers;
using System.Text;
using Newtonsoft.Json;
using TechTestBackend.Business.Abstraction;
using TestTestBackend.Data.Models;

namespace TechTestBackend.Business;

public class SpotifyApiClient : ISpotifyApiClient
{
    private readonly IConfiguration _configuration;
    private const string SpotifyV1ApiBaseUrl = "https://api.spotify.com/v1";

    public SpotifyApiClient(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task<IEnumerable<SpotifySong>> SearchForSongsByName(string name)
    {
        var client = await GetSpotifyAuthenticatedClient();
        var response = await client.GetAsync($"{SpotifyV1ApiBaseUrl}/search?q={name}&type=track");
        dynamic deserializedObjects = JsonConvert.DeserializeObject(await response.Content.ReadAsStringAsync())!;

        var songs = JsonConvert.DeserializeObject<SpotifySong[]>(deserializedObjects.tracks.items.ToString());

        return songs;
    }

    public async Task<SpotifySong?> GetSong(string id)
    {
        var client = await GetSpotifyAuthenticatedClient();

        var response = await client.GetAsync($"{SpotifyV1ApiBaseUrl}/tracks/{id}/");
        dynamic objects = JsonConvert.DeserializeObject(await response.Content.ReadAsStringAsync())!;

        var song = JsonConvert.DeserializeObject<SpotifySong>(objects.ToString());

        return song;
    }

    private async Task<HttpClient> GetSpotifyAuthenticatedClient()
    {
        var clientId = _configuration.GetValue<string>("TechTestBackend:Spotify:ClientId");
        var clientSecret = _configuration.GetValue<string>("TechTestBackend:Spotify:ClientSecret");
        if (clientId == null || clientSecret == null)
            throw new Exception("Missing client ID or secret from configuration");

        var base64EncodedCredentials = GetBase64EncodedCredentials(clientId, clientSecret);

        var client = new HttpClient();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", base64EncodedCredentials);

        var accessTokenResponse = await client.PostAsync("https://accounts.spotify.com/api/token", new FormUrlEncodedContent(new[] { new KeyValuePair<string, string>("grant_type", "client_credentials") }));
        dynamic deserializedTokenResponse = JsonConvert.DeserializeObject(await accessTokenResponse.Content.ReadAsStringAsync())!;
        if (deserializedTokenResponse == null)
            throw new Exception("Could not retrieve bearer token from Spotify");

        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", deserializedTokenResponse.access_token.ToString());
        return client;
    }

    private static string GetBase64EncodedCredentials(string clientId, string clientSecret)
    {
        var asciiEncodedCredentials = Encoding.ASCII.GetBytes($"{clientId}:{clientSecret}");
        return Convert.ToBase64String(asciiEncodedCredentials);
    }
}
