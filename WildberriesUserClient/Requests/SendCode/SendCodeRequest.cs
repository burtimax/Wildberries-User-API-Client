using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using WildberriesUserClient.Wildberries;

namespace WildberriesUserClient.Requests.SendCode;

/// <summary>
/// Запрос отправки кода на телефон.
/// </summary>
[JsonObject(MemberSerialization.OptIn, NamingStrategyType = typeof(SnakeCaseNamingStrategy))]
public class SendCodeRequest : RequestBase<SendCodeResponseModel> 
{
    /// <summary>
    /// Номер телефона в формате [79123456789].
    /// </summary>
    [JsonProperty(Required = Required.Always)]
    public string PhoneNumber { get; set; }
    
    /// <summary>
    /// Конструктор.
    /// </summary>
    public SendCodeRequest(WildberriesClientOptions options, string phoneNumber) : base(options, options.AuthServiceHost, "/v2/code")
    {
        PhoneNumber = phoneNumber;
        Method = HttpMethod.Post;
    }

    public override HttpRequestMessage GetHttpRequest(SessionData sessionData)
    {
        HttpRequestMessage httpRequest = base.GetHttpRequest(sessionData);
        httpRequest.Headers.Add("Deviceid", sessionData.DeviceId);
        httpRequest.Headers.Add("Host", ClientOptions.AuthServiceHost);
        return httpRequest;
    }
}