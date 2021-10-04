using System.Text.Json.Serialization;

namespace Tgs.Global.ViewModels
{
    public record BrandPlayerViewModel
    {
        [JsonPropertyName("playercode")]
        public string PlayerCode { get; init; }


        [JsonPropertyName("username")]
        public string UserName { get; init; }


        [JsonPropertyName("status")]
        public PlayerStatus Status { get; init; }
    }
}