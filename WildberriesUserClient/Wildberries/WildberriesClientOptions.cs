namespace WildberriesUserClient.Wildberries;

public class WildberriesClientOptions
{
    /// <summary>
    /// Версия приложения WB.
    /// </summary>
    /// <remarks>
    /// В заголовках запроса браузера такой параметр.
    /// </remarks>
    public string WbAppVersion { get; set; } = "9.4.25.1";
    
    /// <summary>
    /// Адрес сервиса авторизации.
    /// </summary>
    public string AuthServiceHost { get; set; } = "wbx-auth.wildberries.ru";

    /// <summary>
    /// Адрес сервиса карточек товаров.
    /// </summary>
    public string CardServiceHost { get; set; } = "card.wb.ru";

    /// <summary>
    /// Адрес сервиса корзины товаров.
    /// </summary>
    public string CartStorageServiceHost { get; set; } = "cart-storage-api.wildberries.ru";
}