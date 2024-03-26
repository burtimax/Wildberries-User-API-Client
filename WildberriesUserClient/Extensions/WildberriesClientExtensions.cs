using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using WildberriesUserClient.Requests.Authorize;
using WildberriesUserClient.Requests.GetCard;
using WildberriesUserClient.Requests.LogOut;
using WildberriesUserClient.Requests.SendCode;
using WildberriesUserClient.Requests.SyncBasket;
using WildberriesUserClient.Wildberries;

namespace WildberriesUserClient.Extensions;

public static class WildberriesClientExtensions
{
    /// <summary>
    /// Отправить код подтверждения авторизации на телефон.
    /// </summary>
    /// <param name="client">API клиент.</param>
    /// <param name="phone">Телефон в формате [79123456789].</param>
    /// <param name="ctx"></param>
    /// <returns></returns>
    public static async Task<SendCodeResponseModel> SendAuthCodeAsync(this WildberriesClient client, string phone,
        CancellationToken ctx = default)
    {
        SendCodeRequest request = new(client.Options, phone);
        string json = await client.ThrowIfNull()
            .MakeRequestAsync(request, ctx);

        JObject o = JObject.Parse(json);
        
        // Неудачно прошла оперция отправки кода.
        if ((int) o["result"] != 0)
        {
            throw new Exception($"Запрос отправки кода авторизации вернул ошибочный результат: {json}");
        }
        
        SendCodeResponseModel result = JsonConvert.DeserializeObject<SendCodeResponseModel>(o["payload"].ToString()!)!;
        
        client.SessionData.Sticker = result.Sticker;
        client.SessionData.UserId = result.UserId;

        return result;
    }
    
    /// <summary>
    /// Авторизация клиента.
    /// </summary>
    /// <param name="client">API клиент.</param>
    /// <param name="code">6-значный код из приложения или СМС.</param>
    /// <param name="ctx"></param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    public static async Task<bool> AuthorizeAsync(this WildberriesClient client, int code,
        CancellationToken ctx = default)
    {
        AuthorizeRequest request = new(client.Options, client.SessionData.Sticker, code);
        string json = await client.ThrowIfNull()
            .MakeRequestAsync(request, ctx);

        JObject o = JObject.Parse(json);
        
        // Неудачно прошла оперция отправки кода.
        if ((int) o["result"] != 0)
        {
            throw new Exception($"Запрос авторизации вернул ошибочный результат: {json}");
        }
        
        AuthorizeResponseModel result = JsonConvert.DeserializeObject<AuthorizeResponseModel>(o["payload"].ToString()!)!;

        client.SessionData.JwtAccess = result.AccessToken; 

        return true;
    }
    
    /// <summary>
    /// Получение данных по товару из Wildberries.
    /// </summary>
    /// <param name="client">API клиент.</param>
    /// <param name="nomenclatureId">Номер номенклатуры.</param>
    /// <param name="ctx">Токен отмены операции.</param>
    /// <returns></returns>
    public static async Task<GetCardResponseModel> GetCard(this WildberriesClient client, long nomenclatureId,
        CancellationToken ctx = default)
    {
        GetCardRequest request = new(client.Options, nomenclatureId);
        string json = await client.ThrowIfNull()
            .MakeRequestAsync(request, ctx);

        JObject o = JObject.Parse(json);

        GetCardResponseModel result = new();
        result.DeserializeFromWbResponse(o["data"]["products"].First.ToString()!);
        
        return result;
    }
    
    /// <summary>
    /// Добавление товара в корзину.
    /// </summary>
    /// <param name="client">API клиент.</param>
    /// <param name="nomenclatureId">Код номенклатуры товара.</param>
    /// <param name="sizeOptionId">Параметр размерности товара.</param>
    /// <param name="quantity">Кол-во товара в корзину (ограничения [0..99]).</param>
    /// <param name="ctx"></param>
    /// <exception cref="Exception"></exception>
    public static async Task SyncBasket(this WildberriesClient client, 
        AddingProductDto? product,
        CancellationToken ctx = default)
    {
        if(product is not null && 
           (product.Quantity < 0 || product.Quantity > 99))
            throw new Exception("Кол-во товара, добавляемого в корзину должно быть в пределах 1..99.");
        
        SyncBasketRequest request = new(client.Options, client.SessionData.DeviceId, client.SessionData.BasketTs, product);
        string json = await client.ThrowIfNull()
            .MakeRequestAsync(request, ctx);

        JObject o = JObject.Parse(json);
        
        if (o["state"]!.Value<int>() != 0)
        {
            throw new Exception($"Запрос добавления товара в корзину вернул ошибочный результат: {json}");
        }

        client.SessionData.BasketTs = o["change_ts"]!.Value<long>();
    }
    
    /// <summary>
    /// Выход из аккаунта.
    /// </summary>
    /// <param name="client"></param>
    /// <param name="ctx"></param>
    /// <exception cref="Exception"></exception>
    public static async Task LogOut(this WildberriesClient client,
        CancellationToken ctx = default)
    {
        LogOutRequest request = new(client.Options);
        string json = await client.ThrowIfNull()
            .MakeRequestAsync(request, ctx);

        JObject o = JObject.Parse(json);
        
        if (o["result"]!.Value<int>() != 0)
        {
            throw new Exception($"Запрос выхода из аккаунта вернул ошибочный результат: {json}");
        }
    }
}