using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace WildberriesUserClient.Requests.SendCode;

/// <summary>
///  Модель ответа
/// </summary>
[JsonObject(MemberSerialization.OptOut, NamingStrategyType = typeof(SnakeCaseNamingStrategy))]
public class SendCodeResponseModel
{
    public string AuthMethod { get; set; }
    public int Ttl { get; set; }
    public string Sticker { get; set; }
    public string UserId { get; set; }
}