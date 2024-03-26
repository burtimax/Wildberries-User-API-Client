using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using WildberriesUserClient.Wildberries;

namespace WildberriesUserClient.Requests.Authorize;

/// <summary>
/// Запрос авторизации.
/// </summary>
[JsonObject(MemberSerialization.OptIn, NamingStrategyType = typeof(SnakeCaseNamingStrategy))]
public class AuthorizeRequest : RequestBase<AuthorizeResponseModel>
{
    [JsonProperty(Required = Required.Always)]
    public string Sticker { get; set; }

    [JsonProperty(Required = Required.Always)]
    public int Code { get; set; }
    
    public AuthorizeRequest(WildberriesClientOptions options, string sticker, int code) : base(options, options.AuthServiceHost, "/v2/auth")
    {
        Sticker = sticker;
        Code = code;
        Method = HttpMethod.Post;
    }
    
    public override HttpRequestMessage GetHttpRequest(SessionData sessionData)
    {
        HttpRequestMessage httpRequest = base.GetHttpRequest(sessionData);
        httpRequest.Headers.Add("Deviceid", sessionData.DeviceId);
        httpRequest.Headers.Add("Host", ClientOptions.AuthServiceHost);
        httpRequest.Headers.Add("Origin", "https://www.wildberries.ru");
        httpRequest.Headers.Add("Referer", "https://www.wildberries.ru/security/login?returnUrl=https%3A%2F%2Fwww.wildberries.ru%2F");
        httpRequest.Headers.Add("authority", ClientOptions.AuthServiceHost);
        httpRequest.Headers.Add("method", "POST");
        httpRequest.Headers.Add("path", MethodName);
        httpRequest.Headers.Add("scheme", "https");
        return httpRequest;
    }
}