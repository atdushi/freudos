using Spectre.Console;
using Spectre.Console.Rendering;
using FreudOS.PsycheOS;
using FreudOS.AdlerOS;
using FreudOS.Core;
using FreudOS.HorneyOS;

namespace FreudOS.Dashboard;

/*

🩸 Этап 1. Сердцебиение Id (Пульс влечений)

В главном цикле Main симулируется только один процесс — зарождение импульса в недрах FreudOS.
• Если импульс созидательный (Eros), он разряжается и сбрасывает общее давление системы.
• Если импульс деструктивный (Thanatos), строгое Superego накладывает на него вето, принуждая Эго применить механизм Вытеснения (Repression).

🚀 Этап 2. Выстрел в шину данных (Publish)

В момент вытеснения ядро Фрейда пакует все данные о типе и силе влечения в неизменяемый рекорд IdImpulseRepressedEvent и публикует его в PsychicDataBus.Instance.Publish(). В этот момент Фрейд свою задачу выполнил — он не знает, кто и как будет обрабатывать этот пакет данных.

⚡ Этап 3. Реактивный взрыв подписок (Subscribe)

Шина данных мгновенно транслирует пакет трем независимым рантаймам-слушателям:
1. 🌑 Юнг (PsycheOS): Ловит импульс. Либидо сознательного Эго тут же совершает гидравлический сдвиг и перетекает в Бессознательное. Сигнал улетает в архетипические комплексы: FatherComplex видит Thanatos и выбрасывает аффект вины, а MotherComplex уводит Эго в глубокую регрессию. Все слепки оседают в буфере Тони (ShadowDesires).
2. 🌟 Хорни (HorneyOS): Ловит этот же импульс. Система воспринимает скрытый прорыв бессознательного как угрозу целостности. ShouldsTyrannyDecorator принудительно задирает налог на отчуждение (_alienationEnergyCost), выжигая витальность Реального Я. Безопасность системы падает. Если она падает ниже 65%, Хорни генерирует свое собственное событие в эфир — SecurityDroppedEvent.
3. 📉 Адлер (AdlerOS): Сидит в режиме ожидания. На импульсы Фрейда он не подписан, но он чутко слушает сигналы от Хорни. Как только в шине пролетает пакет SecurityDroppedEvent, Адлер понимает: базальная опора системы уничтожена. Включается механизм сверхкомпенсации: индекс неполноценности (_inferiorityIndex) взлетает, а паттерн Strategy на лету переключает жизненный стиль в агрессивный RulingStyle.

 */

class Program
{
    // Глобальные стейты систем, обновляемые ИСКЛЮЧИТЕЛЬНО через реактивные коллбэки шины
    public static HorneyMind HorneyCore { get; private set; } = null!;
    public static IndividualMind PsycheCore { get; private set; } = null!;
    public static AdlerianMind AdlerCore { get; private set; } = null!;
    public static TripartitePsyche FreudCore { get; private set; } = null!;

    static void Main(string[] args)
    {
        AnsiConsole.Clear();
        AnsiConsole.Write(new FigletText("Reactive PsychOS").Centered().Color(Color.MediumPurple));
        AnsiConsole.MarkupLine("[bold cyan]Запуск 100% событийно-ориентированной архитектуры психики...[/]\n");
        
        //--------------------------------------------------------------------------------------------------------------------
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

        var adlerInferiority = 45.0;
        var adlerSocialInterest = 55.0;
        AdlerStyle adlerStyle = AdlerStyle.SociallyUseful;

        var freudInitialLibido = 500.0;
        var freudInitialTension = 15.0;

        // Корректировка параметров на основе выбранного пресета
        switch (simMode)
        {
            case "Пограничный (Повышенное напряжение Id)":
                freudInitialTension = 65.0;
                break;

            case "Невротический (Критическое отчуждение Я)":
                horneyAlienation = 48.0; // В шаге от Pride Lockout (>50)
                horneySecurity = 35.0;
                horneyVitality = 40.0;
                adlerInferiority = 75.0;
                adlerStyle = AdlerStyle.Ruling;
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

        // Инициализируем ядра (они сами подпишутся на шину внутри своих конструкторов!)
        HorneyCore = new HorneyMind(horneySecurity, horneyVitality, horneyAlienation);
        PsycheCore = new IndividualMind();
        AdlerCore = new AdlerianMind(adlerInferiority, adlerSocialInterest, adlerStyle);
        FreudCore = new TripartitePsyche(freudInitialLibido, freudInitialTension);

        AnsiConsole.MarkupLine($"\n[bold green] Настройки зафиксированы. Режим: {simMode}.[/]");
        AnsiConsole.MarkupLine("[bold yellow]Запуск мониторинга через 1.5 секунды...[/]");
        Thread.Sleep(1500);

        //---------------------------------------------------------------------------------------------------------


        string currentFreudLog = "Шина инициализирована. Время движется через импульсы Id.";
        bool globalPanic = false;

        // Перехват фатального сбоя всей макро-психики из шины
        PsychicDataBus.Instance.Subscribe<MacroPsychicPanicEvent>(ev =>
        {
            globalPanic = true;
            currentFreudLog = $"[blink bold red]МАКРО-ПАНИКА: {ev.Reason}[/]";
        });

        var analyst = new Psychoanalyst();

        AnsiConsole.Live(CreateDashboard(currentFreudLog)).Start(ctx =>
        {
            // Главный цикл симуляции теперь НЕ вызывает методы ядер напрямую. 
            // Он просто имитирует удары пульса Id (Heartbeat), а рантаймы реагируют реактивно!
            for (int heartbeat = 1; heartbeat <= 50; heartbeat++)
            {
                if (globalPanic) break;

                Thread.Sleep(300);

                try
                {
                    // Единственный генератор активности — Id производит влечение.
                    // Каждые 2 шага пульса прорывается импульс
                    var rand = new Random().Next(100);
                    IDrive drive = rand > 45 ? new ThanatosDrive() : new ErosDrive();
                    var defense = rand > 50 ? DefenseMechanism.Repression : DefenseMechanism.Sublimation;

                    // Этот метод внутри бросит событие вытеснения в шину, если Superego наложит вето
                    FreudCore.ProcessIdImpulse(drive, defense, out currentFreudLog, heartbeat);

                    // Psychoanalyst и триггер его активации (например, если буфер Тени раздувается больше 5 элементов)
                    if (PsycheCore.GetShadowBufferString().Split(' ', StringSplitOptions.RemoveEmptyEntries).Length > 5)
                    {
                        // Аварийное вмешательство аналитика прямо посреди системных пульсаций!
                        analyst.PerformReflectionTherapy(PsycheCore, HorneyCore, heartbeat);

                        // Небольшая задержка, чтобы пользователь успел прочитать лог интервенции на дашборде
                        Thread.Sleep(2000);
                    }
                }
                catch (Exception ex) when (ex is LibidoLeakException or ResistanceException)
                {
                    currentFreudLog = $"[blink bold red]CRITICAL SHOCK: {ex.Message}[/]";
                    ctx.UpdateTarget(CreateDashboard(currentFreudLog));
                    break;
                }

                ctx.UpdateTarget(CreateDashboard(currentFreudLog));
            }
        });

        AnsiConsole.MarkupLine("\n[bold green]💾 Чистая реактивная сессия завершена. Все OnRuntimeTick удалены без потери стабильности.[/]");
    }

    private static IRenderable CreateDashboard(string freudLog)
    {
        var grid = new Grid().AddColumns(2);

        // 🌟 1. ПАНЕЛЬ HORNEYOS (Реактивная)
        var horneyTable = new Table().NoBorder().AddColumns("Параметр", "Значение");
        horneyTable.AddRow("Базальная безопасность (_security)", $"[green]{HorneyCore.Security:F1}%[/]");
        horneyTable.AddRow("Витальность Реального Я (_realSelfVitality)", $"[cyan]{HorneyCore.Vitality:F1} sp[/]");
        horneyTable.AddRow("Налог на отчуждение (_alienationEnergyCost)", $"[red]{HorneyCore.AlienationCost:F1} sp/pulse[/]");
        string horneyStatus = HorneyCore.AlienationCost > 50.0 ? "[blink red]Pride Блокировка![/]" : "[green]Разрешен дебаг[/]";
        horneyTable.AddRow("Статус Валидатора Гордости", horneyStatus);

        // 🌑 2. ПАНЕЛЬ PSYCHEOS (Реактивная)
        var psycheTable = new Table().NoBorder().AddColumns("Ресурс", "Пул энергии");
        psycheTable.AddRow("Либидо Эго (Сознание)", $"[yellow]{PsycheCore.EgoLibido:F1} cx[/]");
        psycheTable.AddRow("Либидо Бессознательного", $"[purple]{PsycheCore.UnconsciousLibido:F1} cx[/]");
        psycheTable.AddRow("Буфер тени (ShadowDesires)", $"[grey39]{PsycheCore.GetShadowBufferString()}[/]");

        // 📉 3. ПАНЕЛЬ ADLEROS (Реактивная)
        var adlerTable = new Table().NoBorder().AddColumns("Метрика", "Показатель");
        adlerTable.AddRow("Индекс неполноценности", $"[orange1]{AdlerCore.InferiorityIndex:F1}%[/]");
        adlerTable.AddRow("Социальный интерес", $"[green3]{AdlerCore.SocialInterest:F1}%[/]");
        var adlerContent = new Rows(adlerTable, new Markup($"Текущий Жизненный Стиль: [bold white]{AdlerCore.CurrentStyle}[/]").Justify(Justify.Left));

        // 🩸 4. ПАНЕЛЬ FREUDOS (Реактивный Источник Волновых Сигналов)
        var freudTable = new Table().NoBorder().AddColumns("Гидравлика", "Давление");
        string libidoColor = FreudCore.GetLibido() < 150.0 ? "bold red" : "red3";
        freudTable.AddRow("Общий пул либидо (_libidoPool)", $"[{libidoColor}]{FreudCore.GetLibido():F1} cx[/]");
        double tension = FreudCore.GetTension();
        string tensionColor = tension > 80.0 ? "blink red" : tension > 50.0 ? "yellow" : "green";
        freudTable.AddRow("Напряжение катексиса (_cathexisTension)", $"[{tensionColor}]{tension:F1}%[/]");
        freudTable.AddRow("Вытесненные объекты IDrive в буфере", $"[maroon]{FreudCore.GetRepressedCount()} объектов[/]");
        var freudContent = new Rows(freudTable, new Rule("[bold grey]Диспетчер Защит Эго (IDrive Log)[/]").Justify(Justify.Left), new Markup(freudLog).Justify(Justify.Left));

        grid.AddRow(new Panel(horneyTable).Header("🌟 HorneyOS (Реактивная)").BorderColor(Color.DeepSkyBlue1).Expand(),
                    new Panel(psycheTable).Header("🌑 PsycheOS Core (Юнг)").BorderColor(Color.Purple).Expand());
        grid.AddRow(new Panel(adlerContent).Header("📉 AdlerOS Core (Адлер)").BorderColor(Color.Orange1).Expand(),
                    new Panel(freudContent).Header("🩸 FreudOS (Генератор Событий)").BorderColor(Color.Red3).Expand());

        return new Panel(grid).Header("📊 100% СОБЫТИЙНО-ОРИЕНТИРОВАННАЯ МАКРО-ПСИХИКА (АКТИВНЫЙ ПУЛЬС ID)").BorderColor(Color.Grey).Padding(1, 1, 1, 1);
    }
}
