namespace WildberriesUserClient.Requests;

/// <summary>
/// Запрос абстракция.
/// </summary>
public interface IRequest<TResponse>
{
    /// <summary>
    /// Метод запроса.
    /// </summary>
    public HttpMethod Method { get; }
    
    /// <summary>
    /// Привести запрос к HttpContent.
    /// </summary>
    /// <returns></returns>
    public HttpContent? ToHttpContent();
}