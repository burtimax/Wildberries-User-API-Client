namespace WildberriesOrderProduct;

/// <summary>
/// Константы приложения.
/// </summary>
public class AppConstants
{
    
    
    /// <summary>
    /// Версия приложения WB.
    /// </summary>
    /// <remarks>
    /// В заголовках запроса браузера такой параметр.
    /// </remarks>
    public const string WbAppVersion = "9.4.25.1";
    
    /// <summary>
    /// Адрес сервиса авторизации.
    /// </summary>
    public const string AuthServiceHost = "wbx-auth.wildberries.ru";

    /// <summary>
    /// Адрес сервиса карточек товаров.
    /// </summary>
    public const string CardServiceHost = "card.wb.ru";

    /// <summary>
    /// Адрес сервиса корзины товаров.
    /// </summary>
    public const string CartStorageServiceHost = "cart-storage-api.wildberries.ru";
}