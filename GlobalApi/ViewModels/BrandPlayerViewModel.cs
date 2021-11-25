using System.Text.Json.Serialization;

namespace Tgs.Global.ViewModels
{
    public record BrandPlayerViewModel
    {
        [JsonPropertyName("playercode")]
        public string PlayerCode { get; init; } = default!;


        [JsonPropertyName("status")]
        public PlayerStatus Status { get; init; }


        [JsonPropertyName("username")]
        public string UserName { get; init; } = default!;
    }
}