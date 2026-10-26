using FreudOS.Core;
using Spectre.Console;
using Spectre.Console.Rendering;

// Эмуляция пространств имен наших систем (для компиляции в одном файле)
namespace FreudOS.Dashboard;
/*
class Program
{
    static void Main(string[] args)
    {
        AnsiConsole.Clear();
        AnsiConsole.Write(
            new FigletText("Psych-Runtimes")
                .Centered()
                .Color(Color.Purple));

        AnsiConsole.MarkupLine("[bold yellow]Инициализация мета-контекста психологических рантаймов с поддержкой IDrive...[/]");
        Thread.Sleep(800);

        // Инициализируем реальное низкоуровневое ядро Фрейда
        var freudCore = new TripartitePsyche();
        var random = new Random();

        // Остальные рантаймы пока остаются в режиме эмуляции слепков состояния
        var horneySecurity = 75.0;
        var horneyVitality = 90.0;
        var horneyAlienation = 12.0;

        var psycheLibidoEgo = 60.0;
        var psycheLibidoUnconscious = 40.0;
        var psycheShadowBuffer = "Aggression_Mother_Complex; Ambition_Suppressed;";

        var adlerInferiority = 45.0;
        var adlerSocialInterest = 55.0;
        string adlerStyle = "SociallyUsefulStyle";

        string currentFreudLog = "Инициализация полиморфного пула Id...";

        // Создаем интерактивный Live-дисплей Spectre.Console
        AnsiConsole.Live(CreateDashboard(
            horneySecurity, horneyVitality, horneyAlienation,
            psycheLibidoEgo, psycheLibidoUnconscious, psycheShadowBuffer,
            adlerInferiority, adlerSocialInterest, adlerStyle,
            freudCore, currentFreudLog
        )).Start(ctx =>
        {
            // Симуляция 50 системных тиков работы макро-психики
            for (int tick = 1; tick <= 50; tick++)
            {
                Thread.Sleep(250); // Скорость системного тика

                try
                {
                    // ── 1. ВЫЧИСЛЕНИЯ FREUDOS (Реальное ядро с IDrive) ──
                    freudCore.OnRuntimeTick();
                    currentFreudLog = $"[grey]Тик {tick}:[/] Пассивное удержание импульсов.";

                    // Каждые 3 тика Id генерирует строго типизированное влечение
                    if (tick % 3 == 0)
                    {
                        IDrive currentDrive = random.Next(100) > 45
                            ? new ThanatosDrive()
                            : new ErosDrive();

                        var defense = random.Next(100) > 50
                            ? DefenseMechanism.Repression
                            : DefenseMechanism.Sublimation;

                        freudCore.ProcessIdImpulse(currentDrive, defense, out currentFreudLog);
                    }
                }
                catch (LibidoLeakException ex)
                {
                    currentFreudLog = $"[blink bold red]CRITICAL: {ex.Message}[/]";
                    // Обновляем дашборд аварийным логом и прерываем симуляцию
                    ctx.UpdateTarget(CreateDashboard(
                        horneySecurity, horneyVitality, horneyAlienation,
                        psycheLibidoEgo, psycheLibidoUnconscious, psycheShadowBuffer,
                        adlerInferiority, adlerSocialInterest, adlerStyle,
                        freudCore, currentFreudLog
                    ));
                    break;
                }

                // ── 2. ИЗМЕНЕНИЕ ЭМУЛИРУЕМЫХ СОСТОЯНИЙ (Каскадные триггеры) ──

                // HorneyOS: Растет налог на отчуждение, падает витальность
                horneyAlienation += 1.2;
                horneyVitality = Math.Max(0, horneyVitality - (horneyAlienation * 0.05));
                if (horneyAlienation > 35)
                {
                    horneySecurity = Math.Max(0, horneySecurity - 1.5);
                }

                // AdlerOS: При дестабилизации безопасности Хорни — растет адлерианская неполноценность
                if (horneySecurity < 65)
                {
                    adlerInferiority = Math.Min(100, adlerInferiority + 2.0);
                    adlerStyle = "RulingStyle (Сверхкомпенсация)";
                }

                // PsycheOS: При росте напряжения у Фрейда, аффекты перетекают в тень Юнга
                if (freudCore.GetTension() > 50)
                {
                    psycheLibidoUnconscious += 1.2;
                    psycheLibidoEgo -= 1.2;
                    if (tick % 6 == 0)
                    {
                        psycheShadowBuffer += "Thanatos_Projected;";
                    }
                }

                // Перерисовываем интерфейс с новыми динамическими данными
                ctx.UpdateTarget(CreateDashboard(
                    horneySecurity, horneyVitality, horneyAlienation,
                    psycheLibidoEgo, psycheLibidoUnconscious, psycheShadowBuffer,
                    adlerInferiority, adlerSocialInterest, adlerStyle,
                    freudCore, currentFreudLog
                ));
            }
        });

        AnsiConsole.MarkupLine("\n[bold green]💾 Системный цикл завершен. Напряжение зафиксировано, слепки IDrive сохранены.[/]");
    }

    /// <summary>
    /// Фабрика макета дашборда. Собирает сетку панелей.
    /// Использует реальный экземпляр TripartitePsyche для вывода метрик Фрейда.
    /// </summary>
    private static IRenderable CreateDashboard(
        double hSec, double hVit, double hAl,
        double pEgo, double pUnc, string pShadow,
        double aInf, double aSoc, string aStyle,
        TripartitePsyche freud, string freudLog)
    {
        var grid = new Grid().AddColumns(2);

        // 🌟 1. ПАНЕЛЬ HORNEYOS
        var horneyTable = new Table().NoBorder().AddColumns("Параметр", "Значение");
        horneyTable.AddRow("Базальная безопасность (_security)", $"[green]{hSec:F1}%[/]");
        horneyTable.AddRow("Витальность Реального Я (_realSelfVitality)", $"[cyan]{hVit:F1} sp[/]");
        horneyTable.AddRow("Налог на отчуждение (_alienationEnergyCost)", $"[red]{hAl:F1} sp/tick[/]");

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

        var styleWidget = new Markup($"Текущий Стиль Жизни: [bold white]{aStyle}[/]").Justify(Justify.Left);

        var adlerRows = new Rows(adlerTable, styleWidget);
        var adlerPanel = new Panel(adlerRows)
            .Header("📉 AdlerOS Core (Адлер)")
            .BorderColor(Color.Orange1)
            .Expand();

        // 🩸 4. ПАНЕЛЬ FREUDOS (Честный рендеринг через переданный объект)
        var freudTable = new Table().NoBorder().AddColumns("Гидравлика", "Давление");

        string libidoColor = freud.GetLibido() < 150.0 ? "bold red" : "red3";
        freudTable.AddRow("Общий пул либидо (_libidoPool)", $"[{libidoColor}]{freud.GetLibido():F1} cx[/]");

        double tension = freud.GetTension();
        string tensionColor = tension > 80.0 ? "blink red" : tension > 50.0 ? "yellow" : "green";
        freudTable.AddRow("Напряжение катексиса (_cathexisTension)", $"[{tensionColor}]{tension:F1}%[/]");
        freudTable.AddRow("Вытесненные объекты IDrive в буфере", $"[maroon]{freud.GetRepressedCount()} объектов[/]");
        freudTable.AddRow("Текущий налог на удержание", $"[grey50]{freud.GetRepressionCost():F1} cx/tick[/]");

        // Объединяем таблицу Фрейда и лог обработки влечений IDrive
        var freudContent = new Rows(
            freudTable,
            new Rule("[bold grey]Диспетчер Защит Эго (IDrive Log)[/]").Justify(Justify.Left),
            new Markup(freudLog).Justify(Justify.Left)
        );

        var freudPanel = new Panel(freudContent)
            .Header("🩸 FreudOS Низкоуровневый Слой (C# 13 Contract)")
            .BorderColor(Color.Red3)
            .Expand();

        // Собираем всё в сетку
        grid.AddRow(horneyPanel, psychePanel);
        grid.AddRow(adlerPanel, freudPanel);

        return new Panel(grid)
            .Header("📊 ПСИХОЛОГИЧЕСКИЙ РАНТАЙМ МАКРО-ПСИХИКИ (МОНИТОРИНГ IDRIVE)")
            .BorderColor(Color.Grey)
            .Padding(1, 1, 1, 1);
    }
}
*/

class Program
{
    static void Main(string[] args)
    {
        AnsiConsole.Clear();
        AnsiConsole.Write(
            new FigletText("Psych-Runtimes")
                .Centered()
                .Color(Color.Purple));

        AnsiConsole.MarkupLine("[bold yellow]Добро пожаловать в панель управления макро-психикой![/]\n");

        // ── ИНТЕРАКТИВНОЕ СТАРТОВОЕ МЕНЮ ──

        // 1. Выбор базового режима симуляции
        var simMode = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("Выберите [bold cyan]начальный режим функционирования[/] рантайма:")
                .PageSize(5)
                .AddChoices([
                    "Стабильный (Базовые настройки)",
                    "Пограничный (Повышенное напряжение Id)",
                    "Невротический (Критическое отчуждение Я)",
                    "Кастомный (Ручная настройка параметров)"
                ]));

        // Инициализация дефолтных параметров
        var horneySecurity = 75.0;
        var horneyVitality = 90.0;
        var horneyAlienation = 12.0;

        var psycheLibidoEgo = 60.0;
        var psycheLibidoUnconscious = 40.0;
        var psycheShadowBuffer = "Aggression_Mother_Complex; ";

        var adlerInferiority = 45.0;
        var adlerSocialInterest = 55.0;
        string adlerStyle = "SociallyUsefulStyle";

        var freudInitialLibido = 500.0;
        var freudInitialTension = 15.0;

        // Корректировка параметров на основе выбранного пресета
        switch (simMode)
        {
            case "Пограничный (Повышенное напряжение Id)":
                freudInitialTension = 65.0;
                psycheLibidoUnconscious = 70.0;
                psycheLibidoEgo = 30.0;
                psycheShadowBuffer += "Thanatos_Active; ";
                break;

            case "Невротический (Критическое отчуждение Я)":
                horneyAlienation = 48.0; // В шаге от Pride Lockout (>50)
                horneySecurity = 35.0;
                horneyVitality = 40.0;
                adlerInferiority = 75.0;
                adlerStyle = "RulingStyle (Сверхкомпенсация)";
                break;

            case "Кастомный (Ручная настройка параметров)":
                AnsiConsole.Write(new Rule("[yellow]Конфигурация ресурсов состояния[/]").Justify(Justify.Left));

                horneySecurity = AnsiConsole.Prompt(
                    new TextPrompt<double>("Базальная безопасность HorneyOS [green](0-100%)[/]:")
                        .DefaultValue(75.0)
                        .ValidationErrorMessage("[red]Введите корректное число процентов![/]")
                        .Validate(n => n is >= 0 and <= 100));

                horneyAlienation = AnsiConsole.Prompt(
                    new TextPrompt<double>("Начальный налог на отчуждение [red](sp/tick)[/]:")
                        .DefaultValue(12.0));

                freudInitialTension = AnsiConsole.Prompt(
                    new TextPrompt<double>("Начальное напряжение катексиса FreudOS [purple](0-100%)[/]:")
                        .DefaultValue(15.0)
                        .Validate(n => n is >= 0 and <= 100));
                break;
        }

        AnsiConsole.MarkupLine($"\n[bold green] Настройки зафиксированы. Режим: {simMode}.[/]");
        AnsiConsole.MarkupLine("[bold yellow]Запуск мониторинга через 1.5 секунды...[/]");
        Thread.Sleep(1500);

        // Инициализируем реальное ядро Фрейда с кастомными/пресетными параметрами
        var freudCore = new TripartitePsyche(freudInitialLibido, freudInitialTension);
        string currentFreudLog = "Рантайм успешно развернут из стартового меню.";

        // Создаем интерактивный Live-дисплей Spectre.Console
        AnsiConsole.Live(CreateDashboard(
            horneySecurity, horneyVitality, horneyAlienation,
            psycheLibidoEgo, psycheLibidoUnconscious, psycheShadowBuffer,
            adlerInferiority, adlerSocialInterest, adlerStyle,
            freudCore, currentFreudLog
        )).Start(ctx =>
        {
            // Симуляция 50 системных тиков работы макро-психики
            for (int tick = 1; tick <= 50; tick++)
            {
                Thread.Sleep(250); // Скорость системного тика

                try
                {
                    // ── 1. ВЫЧИСЛЕНИЯ FREUDOS (Реальное ядро с IDrive) ──
                    freudCore.OnRuntimeTick();
                    currentFreudLog = $"[grey]Тик {tick}:[/] Пассивное удержание импульсов.";

                    if (tick % 3 == 0)
                    {
                        var randomVal = new Random().Next(100);
                        IDrive currentDrive = randomVal > 45 ? new ThanatosDrive() : new ErosDrive();
                        var defense = randomVal > 50 ? DefenseMechanism.Repression : DefenseMechanism.Sublimation;

                        freudCore.ProcessIdImpulse(currentDrive, defense, out currentFreudLog);
                    }
                }
                catch (LibidoLeakException ex)
                {
                    currentFreudLog = $"[blink bold red]CRITICAL: {ex.Message}[/]";
                    ctx.UpdateTarget(CreateDashboard(
                        horneySecurity, horneyVitality, horneyAlienation,
                        psycheLibidoEgo, psycheLibidoUnconscious, psycheShadowBuffer,
                        adlerInferiority, adlerSocialInterest, adlerStyle,
                        freudCore, currentFreudLog
                    ));
                    break;
                }

                // ── 2. ИЗМЕНЕНИЕ ЭМУЛИРУЕМЫХ СОСТОЯНИЙ (Каскадные триггеры) ──
                horneyAlienation += 1.2;
                horneyVitality = Math.Max(0, horneyVitality - (horneyAlienation * 0.05));
                if (horneyAlienation > 35)
                {
                    horneySecurity = Math.Max(0, horneySecurity - 1.5);
                }

                if (horneySecurity < 65)
                {
                    adlerInferiority = Math.Min(100, adlerInferiority + 2.0);
                    adlerStyle = "RulingStyle (Сверхкомпенсация)";
                }

                if (freudCore.GetTension() > 50)
                {
                    psycheLibidoUnconscious += 1.2;
                    psycheLibidoEgo -= 1.2;
                    if (tick % 6 == 0)
                    {
                        psycheShadowBuffer += "Thanatos_Projected; ";
                    }
                }

                // Перерисовываем интерфейс с новыми динамическими данными
                ctx.UpdateTarget(CreateDashboard(
                    horneySecurity, horneyVitality, horneyAlienation,
                    psycheLibidoEgo, psycheLibidoUnconscious, psycheShadowBuffer,
                    adlerInferiority, adlerSocialInterest, adlerStyle,
                    freudCore, currentFreudLog
                ));
            }
        });

        AnsiConsole.MarkupLine("\n[bold green]💾 Системный цикл завершен. Текущие слепки архитектур сохранены в постоянную память.[/]");
    }

    private static IRenderable CreateDashboard(
        double hSec, double hVit, double hAl,
        double pEgo, double pUnc, string pShadow,
        double aInf, double aSoc, string aStyle,
        TripartitePsyche freud, string freudLog)
    {
        var grid = new Grid().AddColumns(2);

        // 🌟 1. ПАНЕЛЬ HORNEYOS
        var horneyTable = new Table().NoBorder().AddColumns("Параметр", "Значение");
        horneyTable.AddRow("Базальная безопасность (_security)", $"[green]{hSec:F1}%[/]");
        horneyTable.AddRow("Витальность Реального Я (_realSelfVitality)", $"[cyan]{hVit:F1} sp[/]");
        horneyTable.AddRow("Налог на отчуждение (_alienationEnergyCost)", $"[red]{hAl:F1} sp/tick[/]");

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
        adlerTable.AddRow("Social Interest", $"[green3]{aSoc:F1}%[/]");

        var styleWidget = new Markup($"Текущий Стиль Жизни: [bold white]{aStyle}[/]").Justify(Justify.Left);

        var adlerRows = new Rows(adlerTable, styleWidget);
        var adlerPanel = new Panel(adlerRows)
            .Header("📉 AdlerOS Core (Адлер)")
            .BorderColor(Color.Orange1)
            .Expand();

        // 🩸 4. ПАНЕЛЬ FREUDOS
        var freudTable = new Table().NoBorder().AddColumns("Гидравлика", "Давление");
        string libidoColor = freud.GetLibido() < 150.0 ? "bold red" : "red3";
        freudTable.AddRow("Общий пул либидо (_libidoPool)", $"[{libidoColor}]{freud.GetLibido():F1} cx[/]");

        double tension = freud.GetTension();
        string tensionColor = tension > 80.0 ? "blink red" : tension > 50.0 ? "yellow" : "green";
        freudTable.AddRow("Напряжение катексиса (_cathexisTension)", $"[{tensionColor}]{tension:F1}%[/]");
        freudTable.AddRow("Вытесненные объекты IDrive в буфере", $"[maroon]{freud.GetRepressedCount()} объектов[/]");
        freudTable.AddRow("Текущий налог на удержание", $"[grey50]{freud.GetRepressionCost():F1} cx/tick[/]");
        var freudContent = new Rows(
        freudTable,
        new Rule("[bold grey]Диспетчер Защит Эго (IDrive Log)[/]").Justify(Justify.Left),
        new Markup(freudLog).Justify(Justify.Left)
        );
        var freudPanel = new Panel(freudContent)
        .Header("🩸 FreudOS Низкоуровневый Слой (C# 13 Contract)")
        .BorderColor(Color.Red3)
        .Expand();
        grid.AddRow(horneyPanel, psychePanel);
        grid.AddRow(adlerPanel, freudPanel);
        return new Panel(grid)
        .Header("📊 ПСИХОЛОГИЧЕСКИЙ РАНТАЙМ МАКРО-ПСИХИКИ (МОНИТОРИНГ IDRIVE)")
        .BorderColor(Color.Grey)
        .Padding(1, 1, 1, 1);
    }
}