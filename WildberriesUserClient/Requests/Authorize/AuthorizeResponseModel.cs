using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace WildberriesUserClient.Requests.Authorize;

[JsonObject(MemberSerialization.OptOut, NamingStrategyType = typeof(SnakeCaseNamingStrategy))]
public class AuthorizeResponseModel
{
    /// <summary>
    /// Токен доступа JWT.
    /// </summary>
    public string AccessToken { get; set; }
}