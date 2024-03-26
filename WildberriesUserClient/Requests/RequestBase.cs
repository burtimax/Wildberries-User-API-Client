using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using WildberriesUserClient.Wildberries;

namespace WildberriesUserClient.Requests;

/// <summary>
/// Базовый класс запроса.
/// </summary>
/// <typeparam name="TResponse">Класс ответа.</typeparam>
[JsonObject(MemberSerialization.OptOut, NamingStrategyType = typeof(SnakeCaseNamingStrategy))]
public class RequestBase<TResponse> : IRequest<TResponse>
{
    /// <summary>
    /// Конфигурации API клиента.
    /// </summary>
    [JsonIgnore]
    public WildberriesClientOptions ClientOptions { get; set; }
    
    /// <inheritdoc />
    [JsonIgnore]
    public HttpMethod Method { get; set; }

    /// <summary>
    /// Адрес хоста запроса.
    /// </summary>
    [JsonIgnore]
    public string Host { get; set; }

    /// <summary>
    /// Наименование метода.
    /// </summary>
    [JsonIgnore]
    protected string MethodName { get; set; }

    /// <summary>
    /// Uri запроса.
    /// </summary>
    [JsonIgnore]
    public Uri Uri { get; set; }
    
    /// <summary>
    /// Mime тип.
    /// </summary>
    [JsonIgnore]
    public string MediaType;

    /// <summary>
    /// Конструктор.
    /// </summary>
    /// <param name="options">Конфигурации Wildberries клиента.</param>
    /// <param name="host">Адрес сервиса запроса.</param>
    /// <param name="methodName">Наименование метода API.</param>
    /// <param name="method">Метод запроса.</param>
    /// <param name="mediaType"></param>
    public RequestBase(WildberriesClientOptions options, 
        string host, 
        string methodName, 
        HttpMethod? method = null, 
        string mediaType = "plain/text")
    {
        Host = host;
        Method = method ?? HttpMethod.Post;
        MethodName = methodName;
        Uri = ConfigureUri(Host, MethodName);
        MediaType = mediaType;
        ClientOptions = options;
    }

    /// <summary>
    /// Конфигурация URL запроса.
    /// </summary>
    /// <param name="host">Адрес хоста сервиса.</param>
    /// <param name="methodName">Наименование метода API.</param>
    /// <returns></returns>
    private Uri ConfigureUri(string host, string methodName)
    {
        return new Uri($"https://{Host}{MethodName}");
    }
    
    /// <summary>
    /// Получить объект запроса для HttpClient.
    /// </summary>
    /// <param name="sessionData">Данные сессии.</param>
    /// <returns></returns>
    public virtual HttpRequestMessage GetHttpRequest(SessionData sessionData)
    {
        var httpRequest = new HttpRequestMessage(method: Method, requestUri: Uri)
        {
            Content = this.ToHttpContent()
        };
        
        return httpRequest;
    }
    
    /// <inheritdoc />
    public virtual HttpContent? ToHttpContent() =>
        new StringContent(
            content: JsonConvert.SerializeObject(this),
            encoding: Encoding.UTF8,
            mediaType: MediaType
        );
}