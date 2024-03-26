using System.Net;
using System.Net.Http.Headers;
using WildberriesUserClient.Requests;

namespace WildberriesUserClient.Wildberries;

public class WildberriesClient
{
    private readonly HttpClient _httpClient;
    public SessionData SessionData { get; set; } = new();
    private CookieContainer Cookies { get; set; }
    
    public WildberriesClientOptions Options { get; set; }
    
    public WildberriesClient(HttpClient httpClient = null, Action<WildberriesClientOptions> action = null)
    {
        _httpClient = httpClient ?? new HttpClient();
        Cookies = new();
        Options = new();
        action?.Invoke(Options);
    }
    
    public async Task<string> MakeRequestAsync<TResponse>(RequestBase<TResponse> request, CancellationToken cancellationToken)
    {
       HttpRequestMessage httpRequest = request.GetHttpRequest(SessionData);
        
       SetDefaultHeaders(httpRequest);
       
        // Добавляем куки в запрос.
        AddCookieToRequest(request.Uri, httpRequest);

        // Вставить JWT в запрос.
        if (string.IsNullOrEmpty(SessionData.JwtAccess) == false)
        {
            httpRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", SessionData.JwtAccess);
        }

        // httpRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", "eyJhbGciOiJSUzI1NiIsInR5cCI6IkpXVCJ9.eyJpYXQiOjE3MTE0MTEyOTAsInZlcnNpb24iOjIsInVzZXIiOiI4MzA5Mjk5NiIsInNoYXJkX2tleSI6IjgiLCJjbGllbnRfaWQiOiJ3YiIsInNlc3Npb25faWQiOiJhZTYyZjkwMmQyYWU0N2MxYWY3M2MwMDk4ZTA2YjZlZSIsInVzZXJfcmVnaXN0cmF0aW9uX2R0IjoxNjcyMzgyMzI4LCJ2YWxpZGF0aW9uX2tleSI6ImJhZDg5ZTUzNGQ4ZjVhMzBhM2E4NWNlNDY2NTlhNDNhYWE3Y2Y0NDhhOWI2NGRiYjEyM2NjODAxMzdkZTcyNWIiLCJwaG9uZSI6IkQwTytzOEJ6NGxSazNVbXJIM0IwK0E9PSJ9.BWLkFtUy1MYx8MbqfNnlhLsWiAUiIfPBha9kJBirSS2djDpAfLLXs8mbdvSvLbIl37vyOR1ynV-OsrMVHTRUgd7W41TQUSNvbtlVAFC7Q7BQrccaywCWK38nFUTKmvXUJ-IkMCfqLoWKvAqZmVfy-WT2isvxG0y9V3XCNEpMMjk8cKnFBbVt6ZCTqB7-C_w8hzPf0WbL611lga_Y_lfXc8SUQTXl5lnND8xfJsLwfPbVrLYYqAM7oNZOFfc0s1IoDpWQ1mB5Ne0qO_-BYYrb4tISdVql73JlKCKohgRWLLPEwcCZFUIAogBob-juWc8RhHSbP9dK-QAfAUt-MXO-4g");        
        
        // Отправляем запрос
        var result = await _httpClient.SendAsync(httpRequest, cancellationToken);

        if (result.IsSuccessStatusCode == false)
        {
            throw new Exception($"API exception. URI [{request.Uri}]");
        }

        // Сохраняем куки ответа.
        SaveCookie(request.Uri, result);

        string content = await result.Content.ReadAsStringAsync(cancellationToken);
        return content;
    }

    /// <summary>
    /// Установить заголовки по умолчанию для запросов.
    /// </summary>
    private void SetDefaultHeaders(HttpRequestMessage httpRequest)
    {
        httpRequest.Headers.Add("Accept", "*/*");
        httpRequest.Headers.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/122.0.0.0 Safari/537.36");
        httpRequest.Headers.Add("Sec-Ch-Ua", "\"Chromium\";v=\"122\", \"Not(A:Brand\";v=\"24\", \"Google Chrome\";v=\"122\"");
        httpRequest.Headers.Add("Sec-Ch-Ua-Mobile", "?1");
        httpRequest.Headers.Add("Sec-Ch-Ua-Platform", "Android");
        httpRequest.Headers.Add("Wb-AppType", "web");
        httpRequest.Headers.Add("Wb-AppVersion", Options.WbAppVersion);
        httpRequest.Headers.Add("Sec-Fetch-Mode", "cors");
        httpRequest.Headers.Add("Sec-Fetch-Dest", "empty");
        httpRequest.Headers.Add("Sec-Fetch-Site", "same-site");
        httpRequest.Headers.Add("Accept-Language", "ru-RU,ru;q=0.9,en-US;q=0.8,en;q=0.7");
    }

    /// <summary>
    /// Добавляем cookie, связанные с доменом Uri в запрос.
    /// </summary>
    /// <param name="uri">Uri запроса.</param>
    /// <param name="httpRequest">Обзект запроса.</param>
    private void AddCookieToRequest(Uri uri, HttpRequestMessage httpRequest)
    {
        string? cookieHeader = Cookies.GetCookieHeader(uri);
        if (string.IsNullOrEmpty(cookieHeader) == false)
        {
            httpRequest.Headers.Add("cookie", cookieHeader);
        }
    }

    /// <summary>
    /// Сохраняем куки ответа в хранилище куков.
    /// </summary>
    /// <param name="uri"></param>
    /// <param name="responseMessage"></param>
    private void SaveCookie(Uri uri, HttpResponseMessage responseMessage)
    {
        if (responseMessage.Headers.Contains("Set-Cookie"))
        {
            foreach (var ch in responseMessage.Headers.GetValues("Set-Cookie"))
                // Добавляем заголовки кук в CookieContainer.
                Cookies.SetCookies(uri, ch);
        }
    }
}