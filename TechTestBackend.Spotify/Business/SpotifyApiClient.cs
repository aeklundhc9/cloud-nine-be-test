using System.Net.Http.Headers;
using System.Text;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using TechTestBackend.Spotify.Business.Abstraction;
using TechTestBackend.Spotify.Integration;
using TestTestBackend.Data.Models;

namespace TechTestBackend.Spotify.Business;

public class SpotifyApiClient : ISpotifyApiClient
{
    private readonly IConfiguration _configuration;
    private const string SpotifyV1ApiBaseUrl = "https://api.spotify.com/v1";

    // TODO The API client should handle things like too many requests from Spotify with a policy

    public SpotifyApiClient(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task<IEnumerable<SpotifySong>> SearchForSongsByName(string name)
    {
        var client = await GetSpotifyAuthenticatedClient();
        var response = await client.GetAsync($"{SpotifyV1ApiBaseUrl}/search?q={name}&type=track");
        var deserializedObjects = JsonConvert.DeserializeObject<SpotifySearchApiModel>(await response.Content.ReadAsStringAsync());

        return deserializedObjects?.Tracks.Items.Select(x => new SpotifySong()
        {
            Name = x.Name,
            Id = x.Id
        }) ?? Enumerable.Empty<SpotifySong>();
    }

    public async Task<SpotifySong?> GetSong(string id)
    {
        var client = await GetSpotifyAuthenticatedClient();

        var response = await client.GetAsync($"{SpotifyV1ApiBaseUrl}/tracks/{id}/");
        var spotifyTrackApiModels = JsonConvert.DeserializeObject<SpotifyTrackApiModel>(await response.Content.ReadAsStringAsync());
        if (spotifyTrackApiModels == null)
            return null;

        return new SpotifySong
        {
            Id = spotifyTrackApiModels.Id,
            Name = spotifyTrackApiModels.Name
        };
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

        var accessTokenResponse = await client.PostAsync("https://accounts.spotify.com/api/token",
            new FormUrlEncodedContent(new[] { new KeyValuePair<string, string>("grant_type", "client_credentials") }));
        var deserializedTokenResponse = JsonConvert.DeserializeObject<SpotifyOauthTokenApiModel>(await accessTokenResponse.Content.ReadAsStringAsync());
        
        // TODO Should check for a header here instead
        if (string.IsNullOrEmpty(deserializedTokenResponse?.AccessToken))
            throw new Exception("Could not retrieve bearer token from Spotify");

        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", deserializedTokenResponse.AccessToken);
        return client;
    }

    private static string GetBase64EncodedCredentials(string clientId, string clientSecret)
    {
        var asciiEncodedCredentials = Encoding.ASCII.GetBytes($"{clientId}:{clientSecret}");
        return Convert.ToBase64String(asciiEncodedCredentials);
    }
}
