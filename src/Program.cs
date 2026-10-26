using Spectre.Console;
using Spectre.Console.Rendering;

// Эмуляция пространств имен наших систем (для компиляции в одном файле)
namespace FreudOS.Dashboard;

class Program
{
    static void Main(string[] args)
    {
        AnsiConsole.Clear();
        AnsiConsole.Write(
            new FigletText("Psych-Runtimes")
                .Centered()
                .Color(Color.Purple));

        AnsiConsole.MarkupLine("[bold yellow]Инициализация мета-контекста психологических рантаймов...[/]");
        Thread.Sleep(800);

        // Инициализация заглушек состояний для демонстрации (имитируем наши классы)
        var horneySecurity = 75.0;
        var horneyVitality = 90.0;
        var horneyAlienation = 12.0;

        var psycheLibidoEgo = 60.0;
        var psycheLibidoUnconscious = 40.0;
        var psycheShadowBuffer = "Aggression_Mother_Complex; Ambition_Suppressed;";

        var adlerInferiority = 45.0;
        var adlerSocialInterest = 55.0;
        string adlerStyle = "SociallyUsefulStyle";

        var freudLibido = 480.0;
        var freudTension = 25.0;
        var freudRepressedCount = 3;

        // Создаем интерактивный Live-дисплей Spectre.Console
        AnsiConsole.Live(CreateDashboard(
            horneySecurity, horneyVitality, horneyAlienation,
            psycheLibidoEgo, psycheLibidoUnconscious, psycheShadowBuffer,
            adlerInferiority, adlerSocialInterest, adlerStyle,
            freudLibido, freudTension, freudRepressedCount
        )).Start(ctx =>
        {
            // Демонстрация межсистемных триггеров:
            // В цикле зашита простейшая логика: когда во FreudOS растет вытеснение, данные перетекают в PsycheOS(Тень),
            // а падение безопасности в HorneyOS автоматически дестабилизирует AdlerOS, толкая ее в сверхкомпенсацию(RulingStyle).

            // Симуляция 50 системных тиков работы макро-психики
            for (int tick = 1; tick <= 50; tick++)
            {
                Thread.Sleep(250); // Скорость системного тика

                // ── ИЗМЕНЕНИЕ СОСТОЯНИЙ (Симуляция OnRuntimeTick) ──

                // HorneyOS: Растет налог на отчуждение, падает витальность и безопасность
                horneyAlienation += 1.2;
                horneyVitality = Math.Max(0, horneyVitality - (horneyAlienation * 0.05));
                if (horneyAlienation > 35)
                {
                    horneySecurity = Math.Max(0, horneySecurity - 1.5);
                }

                // FreudOS: Id давит, напряжение котла катексисов растет
                freudTension += 1.8;
                freudLibido -= (freudRepressedCount * 0.4);

                // AdlerOS: При падении базальной безопасности Хорни, у Адлера растет индекс неполноценности
                if (horneySecurity < 65)
                {
                    adlerInferiority = Math.Min(100, adlerInferiority + 2.0);
                    adlerStyle = "RulingStyle (Сверхкомпенсация)";
                }

                // PsycheOS: Аффекты перетекают в теневой буфер
                if (freudTension > 60)
                {
                    psycheLibidoUnconscious += 1.5;
                    psycheLibidoEgo -= 1.5;
                    if (tick % 5 == 0)
                    {
                        psycheShadowBuffer += "Thanatos_Projected;";
                    }
                }

                // Перерисовываем интерфейс с новыми замороженными слепками данных
                ctx.UpdateTarget(CreateDashboard(
                    horneySecurity, horneyVitality, horneyAlienation,
                    psycheLibidoEgo, psycheLibidoUnconscious, psycheShadowBuffer,
                    adlerInferiority, adlerSocialInterest, adlerStyle,
                    freudLibido, freudTension, freudRepressedCount
                ));
            }
        });

        AnsiConsole.MarkupLine("\n[bold green]💾 Системный цикл завершен. Текущие слепки архитектур сохранены в постоянную память.[/]");
    }

    /// <summary>
    /// Фабрика макета дашборда. Собирает сетку панелей.
    /// Исправляет потенциальные ошибки приведения типов (CS0311) через строгую изоляцию рендереров.
    /// </summary>
    private static IRenderable CreateDashboard(
        double hSec, double hVit, double hAl,
        double pEgo, double pUnc, string pShadow,
        double aInf, double aSoc, string aStyle,
        double fLib, double fTen, int fRepCount)
    {
        // Главная сетка на 2 колонки
        var grid = new Grid().AddColumns(2);

        // 🌟 1. ПАНЕЛЬ HORNEYOS
        var horneyTable = new Table().NoBorder().AddColumns("Параметр", "Значение");
        horneyTable.AddRow("Базальная безопасность (_security)", $"[green]{hSec:F1}%[/]");
        horneyTable.AddRow("Витальность Реального Я (_realSelfVitality)", $"[cyan]{hVit:F1} sp[/]");
        horneyTable.AddRow("Налог на отчуждение (_alienationEnergyCost)", $"[red]{hAl:F1} sp/tick[/]");

        // Добавляем индикатор триггера гордости
        string horneyStatus = hAl > 50.0 ? "[blink red]Pride Блокировка![/]" : "[green]Разрешен дебаг[/]";
        horneyTable.AddRow("Статус Валидатора Гордости", horneyStatus);

        var horneyPanel = new Panel(horneyTable)
            .Header("🌟 HorneyOS Core v9.0")
            .BorderColor(Color.DeepSkyBlue1)
            .Expand();

        // 🌑 2. ПАНЕЛЬ PSYCHEOS
        var psycheTable = new Table().NoBorder().AddColumns("Ресурс", "Пул энергии");
        psycheTable.AddRow("Либидо Эго (Сознание)", $"[yellow]{pEgo:F1} cx[/]");
        psycheTable.AddRow("Либидо Бессознательного", $"[purple]{pUnc:F1} cx[/]");
        psycheTable.AddRow("Буфер тени (ShadowDesires)", $"[grey39]{pShadow}[/]");

        var psychePanel = new Panel(psycheTable)
            .Header("🌑 PsycheOS Core (Юнг)")
            .BorderColor(Color.Purple)
            .Expand();

        // 📉 3. ПАНЕЛЬ ADLEROS
        var adlerTable = new Table().NoBorder().AddColumns("Метрика", "Показатель");
        adlerTable.AddRow("Индекс неполноценности", $"[orange1]{aInf:F1}%[/]");
        adlerTable.AddRow("Социальный интерес", $"[green3]{aSoc:F1}%[/]");

        // Использование встроенного расширения Spectre.Console для IRenderable
        var styleWidget = new Markup($"Текущий Стиль Жизни: [bold white]{aStyle}[/]").Justify(Justify.Left);

        var adlerRows = new Rows(adlerTable, styleWidget);
        var adlerPanel = new Panel(adlerRows)
            .Header("📉 AdlerOS Core (Адлер)")
            .BorderColor(Color.Orange1)
            .Expand();

        // 🩸 4. ПАНЕЛЬ FREUDOS
        var freudTable = new Table().NoBorder().AddColumns("Гидравлика", "Давление");
        freudTable.AddRow("Общий пул либидо (_libidoPool)", $"[red3]{fLib:F1} cx[/]");

        // Визуализация критического порога напряжения
        string tensionColor = fTen > 75 ? "blink red" : fTen > 50 ? "yellow" : "green";
        freudTable.AddRow("Напряжение катексиса (_cathexisTension)", $"[{tensionColor}]{fTen:F1}%[/]");
        freudTable.AddRow("Вытесненные объекты Id в буфере", $"[red3]{fRepCount} шт.[/]");

        var freudPanel = new Panel(freudTable)
            .Header("🩸 FreudOS Низкоуровневый Слой (Фрейд)")
            .BorderColor(Color.Red3)
            .Expand();

        // Раскладываем панели по ячейкам сетки
        grid.AddRow(horneyPanel, psychePanel);
        grid.AddRow(adlerPanel, freudPanel);

        // Возвращаем общую панель-контейнер
        return new Panel(grid)
            .Header("📊 ПСИХОЛОГИЧЕСКИЙ РАНТАЙМ МАКРО-ПСИХИКИ (МОНИТОРИНГ)")
            .BorderColor(Color.Grey)
            .Padding(1, 1, 1, 1);
    }
}
