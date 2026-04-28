using Domain.Interfaces.Repositories;
using Domain.Models;
using HtmlAgilityPack;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;

namespace Services.ScheduleScraperService;

public class ScheduleScraperService
{
    private readonly IScheduleRepository _repository;
    private const string BaseUrl = "https://lks.bmstu.ru";
    private readonly Regex _roomRegex = new(@"(\d{3,4})([а-яА-Я]?)", RegexOptions.Compiled);

    public ScheduleScraperService(IScheduleRepository repository)
    {
        _repository = repository;
    }

    public async Task StartAsync()
    {
        var options = new ChromeOptions();
        options.AddArgument("--no-sandbox");
        options.AddArgument("--disable-dev-shm-usage");
        options.AddArgument("--incognito");          // РЕЖИМ ИНКОГНИТО (чистые куки и кэш)
        options.AddArgument("--disable-cache");      // Полное отключение кэширования

        using IWebDriver driver = new ChromeDriver(options);
        Console.WriteLine("Браузер запущен. Загрузка главной страницы...");

        // 1. Открываем главную страницу
        driver.Navigate().GoToUrl($"https://lks.bmstu.ru/schedule?opened=8c1b7bb8-e690-11db-89c3-000cf1a7cbf0%2Ca31b5b3d-82fc-1029-96dd-000347adedc6%2Ca31bb563-82fc-1029-96dd-000347adedc6");

        // 2. Ждем отрисовки кнопок-аккордеонов (факультетов)
        var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(15));
        try
        {
            // Ждем появления хотя бы одной кнопки с текстом кафедры или факультета
            wait.Until(d => d.FindElements(By.CssSelector("button._accordion-button_229cw_74")).Count > 0);
        }
        catch (WebDriverTimeoutException)
        {
            Console.WriteLine("Ошибка: Страница не загрузилась за 15 секунд.");
            return;
        }

        Console.WriteLine("Страница отрисована. Собираем все ссылки на группы...");

        // 3. Собираем ссылки на группы из отрендеренного HTML
        // Ищем все <a>, которые ведут на расписание
        var links = driver.FindElements(By.CssSelector("a[href*='/schedule/']"))
            .Select(a => a.GetAttribute("href"))
            .Where(h => h.Split('/').Last().Length > 20) // GUID группы длинный
            .Distinct()
            .ToList();

        if (!links.Any())
        {
            Console.WriteLine("Группы не найдены. Возможно, нужно сначала 'кликнуть' по факультетам.");
            // Если ссылки скрыты, можно запустить скрипт, который "видит" их в коде:
            var allGuidsInJs = new Regex(@"[0-9a-f]{8}-[0-9a-f]{4}-[0-9a-f]{4}-[0-9a-f]{4}-[0-9a-f]{12}")
                .Matches(driver.PageSource)
                .Select(m => m.Value)
                .Distinct()
                .ToList();

            foreach (var guid in allGuidsInJs)
            {
                await ParseGroupById(guid);
                await Task.Delay(300);
            }
        }
        else
        {
            Console.WriteLine($"Найдено групп: {links.Count}");
            foreach (var link in links)
            {
                await ParseGroupSchedule(link);
                await Task.Delay(300);
            }
        }
    }
    private async Task ParseGroupSchedule(string url)
    {
        var options = new ChromeOptions();
        options.AddArgument("--headless");
        options.AddArgument("--incognito");
        options.AddArgument("--disable-cache");
        options.AddArgument("--window-size=1920,1080");

        using IWebDriver driver = new ChromeDriver(options);
        IJavaScriptExecutor js = (IJavaScriptExecutor)driver;

        try
        {
            Console.WriteLine($"[Selenium] Парсинг группы: {url}");
            driver.Navigate().GoToUrl(url);

            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(20));

            // 1. Ждем появления карточек пар (у них класс начинается на _pair-card)
            // Это признак того, что React отрисовал данные
            wait.Until(d => d.FindElements(By.CssSelector("div[class*='_pair-card_']")).Count > 0);

            // 2. Вытаскиваем тексты всех элементов, которые могут содержать аудиторию.
            // Мы берем: ссылки на комнаты (как на скрине), span-ы внутри списков аудиторий 
            // и ячейки на случай старой верстки.
            var roomTexts = (ReadOnlyCollection<object>)js.ExecuteScript(@"
            var results = [];
            // Ищем все элементы аудиторий и ссылки на них
            var selectors = 'div[class*=""_pair-card-audience""] span, a[href*=""/rooms/""], td';
            var elements = document.querySelectorAll(selectors);
            
            for (var i = 0; i < elements.length; i++) {
                var txt = elements[i].innerText.trim();
                // Фильтр: если в строке есть цифры и она короткая (аудитория)
                if (/\d/.test(txt) && txt.length > 0 && txt.length < 15) {
                    results.push(txt);
                }
            }
            return results;
        ");

            Console.WriteLine($"    Найдено сырых текстовых узлов: {roomTexts.Count}");

            // 3. Обработка найденных текстов
            int savedCount = 0;
            foreach (var textObj in roomTexts)
            {
                string text = textObj?.ToString() ?? "";

                // Ваша регулярка: (\d{3,4})([а-яА-Я]?)
                var match = _roomRegex.Match(text);
                if (match.Success)
                {
                    string fullRoom = match.Value;
                    string digits = match.Groups[1].Value;
                    string letter = match.Groups[2].Value.ToLower();

                    // По умолчанию МГТУ: если буквы нет, скорее всего Главное здание
                    if (string.IsNullOrEmpty(letter)) letter = "гз";

                    await SaveToDb(fullRoom, digits, letter);
                    savedCount++;
                }
            }
            Console.WriteLine($"    Успешно обработано аудиторий: {savedCount}");
        }
        catch (WebDriverTimeoutException)
        {
            Console.WriteLine($"    [!] Пропуск: Расписание не загрузилось или пустое (день самоподготовки).");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"    [!] Ошибка: {ex.Message}");
        }
    }
    private async Task SaveToDb(string fullNumber, string digits, string letter)
    {
        // Твое правило этажа
        int floor = digits.Length == 3
            ? int.Parse(digits[0].ToString())
            : int.Parse(digits.Substring(0, 2));

        try
        {
            var building = await _repository.GetBuildingByLetterAsync(letter);
            if (building == null)
            {
                building = new Building(Guid.NewGuid(), letter);
                await _repository.AddBuildingAsync(building);
                await _repository.SaveChangesAsync();
                Console.WriteLine($"[Building] Создан: {letter}");
            }

            if (!await _repository.RoomExistsAsync(fullNumber))
            {
                var room = new Room(Guid.NewGuid(), fullNumber, floor, building.Id);
                await _repository.AddRoomAsync(room);
                await _repository.SaveChangesAsync();
                Console.WriteLine($"[Room] Добавлена: {fullNumber} (этаж {floor}, корпус {letter})");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[DB Error] {fullNumber}: {ex.Message}");
        }
    }

    private async Task ParseGroupById(string guid)
    {
        await ParseGroupSchedule($"{BaseUrl}/schedule/{guid}");
    }
}