using System.Text.Json.Serialization;

namespace RPSSL.API.Infrastructure.External.Models
{
    public class RandomNumberResponse
    {
        [JsonPropertyName("random_number")]
        public int RandomNumber { get; set; }
    }
}
