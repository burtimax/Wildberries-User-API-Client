using Newtonsoft.Json;

namespace WildberriesUserClient.Requests.GetCard;

/// <summary>
/// Размер товара.
/// </summary>
[JsonObject(MemberSerialization.OptIn)]
public class CardSizeModel
{
    [JsonProperty("name")]
    public string Name { get; set; }
    
    [JsonProperty("origName")]
    public string OrigName { get; set; }
    
    [JsonProperty("optionId")]
    public long OptionId { get; set; }
}