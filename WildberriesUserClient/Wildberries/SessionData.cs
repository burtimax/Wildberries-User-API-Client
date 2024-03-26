namespace WildberriesUserClient.Wildberries;

public class SessionData
{
    private string _deviceId = "site_" + Guid.NewGuid().ToString().Replace("-", "");

    /// <summary>
    /// ИД устройства, сгенерированное значение для запросов.
    /// </summary>
    public string DeviceId => _deviceId;

    /// <summary>
    /// Параметр синхронизации корзины.
    /// </summary>
    public long BasketTs { get; set; } = 0;
    
    /// <summary>
    /// Идентификатор пользователя Wildberries.
    /// </summary>
    public string UserId { get; set; }
    
    /// <summary>
    /// Параметр, необходимый для авторизации.
    /// </summary>
    public string Sticker { get; set; }
    
    /// <summary>
    /// Токен доступа JWT.
    /// </summary>
    public string JwtAccess { get; set; } = null;
}