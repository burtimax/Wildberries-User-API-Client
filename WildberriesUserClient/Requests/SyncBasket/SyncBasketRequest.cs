using System.Text;
using Newtonsoft.Json;
using WildberriesUserClient.Wildberries;

namespace WildberriesUserClient.Requests.SyncBasket;

/// <summary>
/// Запрос добавления товара в корзину.
/// </summary>
[JsonObject(MemberSerialization.OptIn)]
public class SyncBasketRequest : RequestBase<SyncBasketResponseModel>
{
    /// <summary>
    /// Номер товара в номенклатуре.
    /// </summary>
    [JsonProperty("cod_1s")]
    public long? NomenclatureId { get; set; }

    /// <summary>
    /// Идентификатор свойства размера товара.
    /// </summary>
    [JsonProperty("chrt_id")]
    public long? SizeOptionId { get; set; }

    /// <summary>
    /// Кол-во товара в корзину.
    /// </summary>
    [JsonProperty("quantity")]
    public int? Quantity { get; set; }

    /// <summary>
    /// Время timestamp текущее.
    /// </summary>
    [JsonProperty("client_ts")]
    public long Timestamp { get; set; }

    /// <summary>
    /// Тип операции над корзиной.
    /// </summary>
    [JsonProperty("op_type")]
    public int OperatinType { get; set; } = 1;

    /// <summary>
    /// Непонятный параметр.
    /// </summary>
    [JsonProperty("target_url")]
    public string TargetUrl { get; set; } = "";
    
    public SyncBasketRequest(WildberriesClientOptions options, string deviceId, long ts, AddingProductDto? product) : base(options, options.CartStorageServiceHost, 
        $"/api/basket/sync?ts={ts}&device_id={deviceId}" + (ts == 0 ? "&remember_me=true" : ""), 
        mediaType: "application/json")
    {
        Timestamp = GetClientTs();
        
        if (product is not null)
        {
            NomenclatureId = product.NomenclatureId;
            SizeOptionId = product.SizeOptionId;
            Quantity = product.Quantity;
        }
    }

    public override HttpContent? ToHttpContent()
    {
        string payload = "";
        
        if (NomenclatureId is not null)
        {
            payload = JsonConvert.SerializeObject(this);
        }
        
        return new StringContent(
            content: $"[{payload}]",
            encoding: Encoding.UTF8,
            mediaType: MediaType
        );
    }

    public override HttpRequestMessage GetHttpRequest(SessionData sessionData)
    {
        HttpRequestMessage httpRequest = base.GetHttpRequest(sessionData);
        // httpRequest.Headers.Add("Deviceid", sessionData.DeviceId);
        httpRequest.Headers.Add("Host", ClientOptions.CartStorageServiceHost);
        httpRequest.Headers.Add("authority", ClientOptions.CartStorageServiceHost);
        httpRequest.Headers.Add("Origin", "https://www.wildberries.ru");
        httpRequest.Headers.Add("Referer", "https://www.wildberries.ru/catalog/171548667/detail.aspx");
        return httpRequest;
    }
    
    /// <summary>
    /// Текущий timastamp в секундах.
    /// </summary>
    private static long GetClientTs () =>
        new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds();
}