using System.Globalization;

namespace Lab01_Converter;

class Program
{
    // Іменовані константи для коефіцієнтів конвертації та фізичних меж
    private const double KmInMile = 1.609344;
    private const double KgInPound = 0.45359237;
    private const double AbsoluteZeroCelsius = -273.15;
    private const double AbsoluteZeroFahrenheit = -459.67;

    static void Main()
    {
        // Встановлюємо підтримку символів Юнікоду (°C, °F тощо)
        Console.OutputEncoding = System.Text.Encoding.UTF8;

        bool isRunning = true;

        while (isRunning)
        {
            Console.WriteLine("\n================== Конвертер величин ==================");
            Console.WriteLine("1. Вихід");
            Console.WriteLine("2. км -> милі");
            Console.WriteLine("3. милі -> км");
            Console.WriteLine("4. кг -> фунти");
            Console.WriteLine("5. фунти -> кг");
            Console.WriteLine("6. °C -> °F");
            Console.WriteLine("7. °F -> °C");
            Console.WriteLine("=======================================================");

            int choice = ReadInt("Оберіть пункт меню (1-7): ", 1, 7);

            // switch expression керує продовженням роботи циклу
            isRunning = choice switch
            {
                1 => false,
                2 => HandleKmToMiles(),
                3 => HandleMilesToKm(),
                4 => HandleKgToPounds(),
                5 => HandlePoundsToKg(),
                6 => HandleCelsiusToFahrenheit(),
                7 => HandleFahrenheitToCelsius(),
                _ => true
            };
        }

        Console.WriteLine("\nРоботу застосунку завершено.");
    }

    // === Обробники пунктів меню (Input + Validation + Output) ===

    static bool HandleKmToMiles()
    {
        if (TryReadValidDouble("Введіть відстань у км: ", minValue: 0, out double km, "Відстань не може бути від'ємною"))
        {
            double miles = KmToMiles(km);
            Console.WriteLine($"\n[Результат]: {km:F2} км = {miles:F2} милі");
        }
        return true;
    }

    static bool HandleMilesToKm()
    {
        if (TryReadValidDouble("Введіть відстань у милях: ", minValue: 0, out double miles, "Відстань не може бути від'ємною"))
        {
            double km = MilesToKm(miles);
            Console.WriteLine($"\n[Результат]: {miles:F2} милі = {km:F2} км");
        }
        return true;
    }

    static bool HandleKgToPounds()
    {
        if (TryReadValidDouble("Введіть масу в кг: ", minValue: 0, out double kg, "Маса не може бути від'ємною"))
        {
            double pounds = KgToPounds(kg);
            Console.WriteLine($"\n[Результат]: {kg:F2} кг = {pounds:F2} фунти");
        }
        return true;
    }

    static bool HandlePoundsToKg()
    {
        if (TryReadValidDouble("Введіть масу у фунтах: ", minValue: 0, out double pounds, "Маса не може бути від'ємною"))
        {
            double kg = PoundsToKg(pounds);
            Console.WriteLine($"\n[Результат]: {pounds:F2} фунти = {kg:F2} кг");
        }
        return true;
    }

    static bool HandleCelsiusToFahrenheit()
    {
        if (TryReadValidDouble("Введіть температуру в °C: ", minValue: AbsoluteZeroCelsius, out double c,
            $"Температура не може бути нижчою за абсолютний нуль ({AbsoluteZeroCelsius:F2} °C)"))
        {
            double f = CelsiusToFahrenheit(c);
            Console.WriteLine($"\n[Результат]: {c:F2} °C = {f:F2} °F");
        }
        return true;
    }

    static bool HandleFahrenheitToCelsius()
    {
        if (TryReadValidDouble("Введіть температуру в °F: ", minValue: AbsoluteZeroFahrenheit, out double f,
            $"Температура не може бути нижчою за абсолютний нуль ({AbsoluteZeroFahrenheit:F2} °F)"))
        {
            double c = FahrenheitToCelsius(f);
            Console.WriteLine($"\n[Результат]: {f:F2} °F = {c:F2} °C");
        }
        return true;
    }

    // === "Робочі" чисті методи конвертації (без Console.Write/Read) ===

    static double KmToMiles(double km) => km / KmInMile;

    static double MilesToKm(double miles) => miles * KmInMile;

    static double KgToPounds(double kg) => kg / KgInPound;

    static double PoundsToKg(double pounds) => pounds * KgInPound;

    static double CelsiusToFahrenheit(double c) => (c * 9.0 / 5.0) + 32.0;

    static double FahrenheitToCelsius(double f) => (f - 32.0) * 5.0 / 9.0;

    // === Допоміжні методи валідації введення ===

    static int ReadInt(string prompt, int min, int max)
    {
        while (true)
        {
            Console.Write(prompt);
            string? input = Console.ReadLine();

            if (int.TryParse(input, out int value) && value >= min && value <= max)
            {
                return value;
            }

            Console.WriteLine($"Некоректний вибір. Введіть ціле число від {min} до {max}.");
        }
    }

    static bool TryReadValidDouble(string prompt, double minValue, out double value, string errorMessage)
    {
        Console.Write(prompt);
        string? input = Console.ReadLine();

        // Замінюємо кому на крапку для коректного парсингу незалежно від регіональних налаштувань ОС
        string normalizedInput = (input ?? "").Trim().Replace(',', '.');

        if (!double.TryParse(normalizedInput, NumberStyles.Float, CultureInfo.InvariantCulture, out value))
        {
            Console.WriteLine("Помилка: введено не число.");
            return false;
        }

        // Перевірка межового (граничного) фізичного випадку
        if (value < minValue)
        {
            Console.WriteLine($"Помилка валідації: {errorMessage}. Операцію скасовано.");
            return false;
        }

        return true;
    }
}