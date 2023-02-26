using Newtonsoft.Json;

#pragma warning disable CS8618

namespace TechTestBackend.Spotify.Integration;

public class SpotifySearchApiModel
{
    [JsonProperty("tracks")] public SpotifySearchTracksApiModel Tracks { get; set; }

    public class SpotifySearchTracksApiModel
    {
        [JsonProperty("href")] public string Href { get; set; }

        [JsonProperty("limit")] public int Limit { get; set; }

        [JsonProperty("next")] public string Next { get; set; }

        [JsonProperty("offset")] public int Offset { get; set; }

        [JsonProperty("previous")] public string Previous { get; set; }

        [JsonProperty("total")] public int Total { get; set; }

        [JsonProperty("items")] public List<Item> Items { get; set; }
    }

    public class Item
    {
        // NOTE: There are more properties to map if one wanted to, but not necessary for the current scope
        [JsonProperty("available_markets")] public List<string> AvailableMarkets { get; set; }

        [JsonProperty("disc_number")] public int DiscNumber { get; set; }

        [JsonProperty("duration_ms")] public int DurationMs { get; set; }

        [JsonProperty("explicit")] public bool Explicit { get; set; }

        [JsonProperty("href")] public string Href { get; set; }

        [JsonProperty("id")] public string Id { get; set; }

        [JsonProperty("is_playable")] public bool IsPlayable { get; set; }

        [JsonProperty("name")] public string Name { get; set; }

        [JsonProperty("popularity")] public int Popularity { get; set; }

        [JsonProperty("preview_url")] public string PreviewUrl { get; set; }

        [JsonProperty("track_number")] public int TrackNumber { get; set; }

        [JsonProperty("type")] public string Type { get; set; }

        [JsonProperty("uri")] public string Uri { get; set; }

        [JsonProperty("is_local")] public bool IsLocal { get; set; }

        [JsonProperty("genres")] public List<string> Genres { get; set; }

        [JsonProperty("album_type")] public string AlbumType { get; set; }

        [JsonProperty("total_tracks")] public int TotalTracks { get; set; }

        [JsonProperty("release_date")] public string ReleaseDate { get; set; }

        [JsonProperty("release_date_precision")]
        public string ReleaseDatePrecision { get; set; }

        [JsonProperty("label")] public string Label { get; set; }

        [JsonProperty("album_group")] public string AlbumGroup { get; set; }

        [JsonProperty("collaborative")] public bool Collaborative { get; set; }

        [JsonProperty("description")] public string Description { get; set; }

        [JsonProperty("public")] public bool Public { get; set; }

        [JsonProperty("snapshot_id")] public string SnapshotId { get; set; }

        [JsonProperty("html_description")] public string HtmlDescription { get; set; }

        [JsonProperty("is_externally_hosted")] public bool IsExternallyHosted { get; set; }

        [JsonProperty("languages")] public List<string> Languages { get; set; }

        [JsonProperty("media_type")] public string MediaType { get; set; }

        [JsonProperty("publisher")] public string Publisher { get; set; }

        [JsonProperty("total_episodes")] public int TotalEpisodes { get; set; }

        [JsonProperty("audio_preview_url")] public string AudioPreviewUrl { get; set; }

        [JsonProperty("language")] public string Language { get; set; }

        [JsonProperty("edition")] public string Edition { get; set; }

        [JsonProperty("total_chapters")] public int TotalChapters { get; set; }
    }
}
