using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;

namespace WildberriesUserClient.Requests.GetCard;

/// <summary>
/// Наименование 
/// </summary>
[JsonObject(MemberSerialization.OptOut, NamingStrategyType = typeof(CamelCaseNamingStrategy))]
public class GetCardResponseModel
{
    /// <summary>
    /// Идентификатор номенклатуры.
    /// </summary>
    public long NomenclatureId { get; set; }

    /// <summary>
    /// Наименование товара.
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    /// Бренд.
    /// </summary>
    public string? Brand { get; set; }
    
    /// <summary>
    /// Поставщик.
    /// </summary>
    public string? Supplier { get; set; }

    /// <summary>
    /// Размеры товара.
    /// </summary>
    public List<CardSizeModel> Sizes { get; set; }
    
    /// <summary>
    /// Десериализация из json ответа.
    /// </summary>
    /// <remarks>
    /// Сделал лобовой маппинг при десериализации.
    /// </remarks>
    public void DeserializeFromWbResponse(string productJson)
    {
        JObject jObj = JObject.Parse(productJson);
        this.NomenclatureId = jObj["id"]!.Value<long>();
        this.Name = jObj["name"]!.Value<string>();
        this.Brand = jObj["brand"]?.Value<string>();
        this.Supplier = jObj["supplier"]?.Value<string>();
        this.Sizes = JsonConvert.DeserializeObject<List<CardSizeModel>>(jObj["sizes"]!.ToString());
    }
}