// See https://aka.ms/new-console-template for more information

using System.Text.RegularExpressions;
using Newtonsoft.Json;
using WildberriesOrderProduct;
using WildberriesUserClient.Extensions;
using WildberriesUserClient.Requests.GetCard;
using WildberriesUserClient.Requests.SyncBasket;
using WildberriesUserClient.Wildberries;


Console.WriteLine("Начало работы программы.");

WildberriesClient client = new(action: SetWildberriesOptionsFromConstants);

try
{
    string phone = InputPhone();
    await client.SendAuthCodeAsync(phone);
    int code = InputCode();
    
    bool isAuth = await client.AuthorizeAsync(code);
    Console.WriteLine("Авторизация прошла успешно\n");

    // Запрос синхронизации корзины.
    await client.SyncBasket(null);
    
    // Добавляем товары.
    while (true)
    {
        try
        {
            Console.WriteLine("Введите код номенклатуры товара для добавления в корзину. (Или 0 для выхода)");
            if (long.TryParse(Console.ReadLine(), out var nm) == false) return;
            if (nm == 0) break;
            GetCardResponseModel card = await client.GetCard(nm);
            PrintCard(card);
            long sizeOptionId = ChoseSizeOptionIdIfNeed(card);
            int quantity = InputQuantity();
            await client.SyncBasket(new AddingProductDto()
            {
                NomenclatureId = card.NomenclatureId,
                SizeOptionId = sizeOptionId,
                Quantity = quantity,
            });
            Console.WriteLine($"Товар [{card.Name}] добавлен в корзину в количестве [{quantity}] шт.\n");
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }
}
catch (Exception e)
{
    Console.WriteLine(e);
}
finally
{
    await client.LogOut();
    Console.WriteLine("Выход из аккаунта.");
}

Console.WriteLine("Программа завершена.");



// Методы и процедуры.
#region Методы и процедуры консольной программы

static void SetWildberriesOptionsFromConstants(WildberriesClientOptions options)
{
    options.AuthServiceHost = AppConstants.AuthServiceHost;
    options.CardServiceHost = AppConstants.CardServiceHost;
    options.CartStorageServiceHost = AppConstants.CartStorageServiceHost;
    options.WbAppVersion = AppConstants.WbAppVersion;
}

/// Ввести кол-во для заказа
static int InputQuantity()
{
    while (true)
    {
        Console.WriteLine("Введи кол-во товара (от 1 до 99).");
        if (int.TryParse(Console.ReadLine(), out int q) &&
            q > 0 && q < 100)
        {
            return q;
        }
    }
}

/// Выбрать размер из ряда
static long ChoseSizeOptionIdIfNeed(GetCardResponseModel card)
{
    if (card.Sizes.Count == 1) return card.Sizes.First().OptionId;

    while (true)
    {
        Console.WriteLine("Выбери номер размерного ряда товара?");
    
        int c = 0;
        foreach (var size in card.Sizes)
        {
            Console.Write($"{++c}) {size.Name ?? size.OrigName}; ");
        }
    
        Console.WriteLine();

        if (InputSizeOptionNum(card.Sizes.Count, out int num))
        {
            return card.Sizes[--num].OptionId;
        }
    }
}

/// Ввести выбранный размер товара.
static bool InputSizeOptionNum(int max, out int num)
{
    if (int.TryParse(Console.ReadLine(), out int n) && n > 0 && n <= max)
    {
        num = n;
        return true;
    }

    num = -1;
    return false;
}

/// Вывести информацию по карточке товара.
static void PrintCard(GetCardResponseModel card)
{
    Console.WriteLine($"Наим: [{card.Name}], Бренд: [{card.Brand}]");
}

/// Ввести телефон.
static string InputPhone()
{
    Regex regex = new Regex(@"^7\d{10}$");

    while (true)
    {
        Console.WriteLine("Введите номер телефона в формате [79123456789]");
        string phone = Console.ReadLine();

        var match = regex.Match(phone);

        if (match.Success) return match.Value;
    }
}

/// Ввод кода авторизации.
static int InputCode()
{
    int code = 0;
    
    while (true)
    {
        Console.WriteLine("Введите 6-значный код");
        code = int.Parse(Console.ReadLine());

        if (IsCodeValid(code)) return code;
    
        Console.WriteLine("Нормально общайся, да!");
    }
}

/// Проверка валидности кода.
static bool IsCodeValid(int code) => code / 100000 > 0;

#endregion

