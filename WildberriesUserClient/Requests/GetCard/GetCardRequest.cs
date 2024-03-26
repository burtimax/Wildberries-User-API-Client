using System.Text.Json.Serialization;
using WildberriesUserClient.Wildberries;

namespace WildberriesUserClient.Requests.GetCard;

/// <summary>
/// Запрос на получение карточки товара.
/// </summary>
public class GetCardRequest : RequestBase<GetCardResponseModel>
{
    [JsonIgnore]
    public long NomenclatureId { get; set; }
    
    public GetCardRequest(WildberriesClientOptions options, long nomenclatureId) : base(options, options.CardServiceHost, $"/cards/v2/detail?nm={nomenclatureId}", HttpMethod.Get)
    {
        NomenclatureId = nomenclatureId;
    }

    public override HttpContent? ToHttpContent()
    {
        return null;
    }
}