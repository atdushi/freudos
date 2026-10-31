using Spectre.Console;
using Spectre.Console.Rendering;
using FreudOS.AdlerOS;
using FreudOS.Core;
using FreudOS.HorneyOS;
using FreudOS.Core.Drive;
using FreudOS.JungOS;

namespace FreudOS;

/*

🩸 Этап 1. Сердцебиение Id (Пульс влечений)

В главном цикле Main симулируется только один процесс — зарождение импульса в недрах FreudOS.
• Если импульс созидательный (Eros), он разряжается и сбрасывает общее давление системы.
• Если импульс деструктивный (Thanatos), строгое Superego накладывает на него вето, принуждая Эго применить механизм Вытеснения (Repression).

🚀 Этап 2. Выстрел в шину данных (Publish)

В момент вытеснения ядро Фрейда пакует все данные о типе и силе влечения в неизменяемый рекорд IdImpulseRepressedEvent и публикует его в PsychicDataBus.Instance.Publish(). В этот момент Фрейд свою задачу выполнил — он не знает, кто и как будет обрабатывать этот пакет данных.

⚡ Этап 3. Реактивный взрыв подписок (Subscribe)

Шина данных мгновенно транслирует пакет трем независимым рантаймам-слушателям:
1. 🌑 Юнг (JungOS): Ловит импульс. Либидо сознательного Эго тут же совершает гидравлический сдвиг и перетекает в Бессознательное. Сигнал улетает в архетипические комплексы: FatherComplex видит Thanatos и выбрасывает аффект вины, а MotherComplex уводит Эго в глубокую регрессию. Все слепки оседают в буфере Тони (ShadowDesires).
2. 🌟 Хорни (HorneyOS): Ловит этот же импульс. Система воспринимает скрытый прорыв бессознательного как угрозу целостности. ShouldsTyrannyDecorator принудительно задирает налог на отчуждение (_alienationEnergyCost), выжигая витальность Реального Я. Безопасность системы падает. Если она падает ниже 65%, Хорни генерирует свое собственное событие в эфир — SecurityDroppedEvent.
3. 📉 Адлер (AdlerOS): Сидит в режиме ожидания. На импульсы Фрейда он не подписан, но он чутко слушает сигналы от Хорни. Как только в шине пролетает пакет SecurityDroppedEvent, Адлер понимает: базальная опора системы уничтожена. Включается механизм сверхкомпенсации: индекс неполноценности (_inferiorityIndex) взлетает, а паттерн Strategy на лету переключает жизненный стиль в агрессивный RulingStyle.

🩺 Особый контур: Интервенция Аналитика (Psychoanalyst.cs)

Аналитик работает как внешний контролирующий скрипт (Reflection-дебаггер).
• Мониторинг: На каждом ударе пульса программа проверяет размер буфера Тени у Юнга.
• Прорыв инкапсуляции: Если Тень раздувается (больше 5 элементов), аналитик активирует терапию. Через механизмы System.Reflection он взламывает приватное поле Юнга, принудительно очищает Тень (.Clear()) и возвращает Эго-Либидо к стабильному значению.
• Блокировка (Гордость): Если Хорни на данном шаге накопила слишком высокий «налог на отчуждение» (система находится в состоянии жесткой психологической защиты), терапия блокируется. Аналитик натыкается на стену сопротивления, и симуляция продолжается в критическом режиме.

*/

class Program
{
    // Глобальные стейты систем, обновляемые ИСКЛЮЧИТЕЛЬНО через реактивные коллбэки шины
    public static HorneyMind HorneyCore { get; private set; } = null!;
    public static JungianMind JungCore { get; private set; } = null!;
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
                .Title("Выберите [bold cyan]начальный режим functioning[/] рантайма:")
                .PageSize(5)
                .AddChoices([
                    "Стабильный (Базовые настройки)",
                    "Пограничный (Повышенное напряжение Id)",
                    "Невротический (Критическое отчуждение Я)",
                    "Кастомный (Ручная настройка параметров)"
                ]));

        // Инициализация дефолтных параметров из конфигурационного манифеста
        var horneySecurity = PsychicConstants.RuntimePresets.DefaultHorneySecurity;
        var horneyVitality = PsychicConstants.RuntimePresets.DefaultHorneyVitality;
        var horneyAlienation = PsychicConstants.RuntimePresets.DefaultHorneyAlienation;

        var adlerInferiority = PsychicConstants.RuntimePresets.DefaultAdlerInferiority;
        var adlerSocialInterest = PsychicConstants.RuntimePresets.DefaultAdlerSocialInterest;
        AdlerLifeStyle adlerStyle = AdlerLifeStyle.SociallyUseful;

        var freudInitialLibido = PsychicConstants.RuntimePresets.DefaultFreudLibido;
        var freudInitialTension = PsychicConstants.RuntimePresets.DefaultFreudTension;

        // Корректировка параметров на основе выбранного пресета
        switch (simMode)
        {
            case "Пограничный (Повышенное напряжение Id)":
                freudInitialTension = PsychicConstants.RuntimePresets.BorderlineFreudTension;
                break;

            case "Невротический (Критическое отчуждение Я)":
                horneyAlienation = PsychicConstants.RuntimePresets.NeuroticHorneyAlienation;
                horneySecurity = PsychicConstants.RuntimePresets.NeuroticHorneySecurity;
                horneyVitality = PsychicConstants.RuntimePresets.NeuroticHorneyVitality;
                adlerInferiority = PsychicConstants.RuntimePresets.NeuroticAdlerInferiority;
                adlerStyle = AdlerLifeStyle.Ruling;
                break;

            case "Кастомный (Ручная настройка параметров)":
                AnsiConsole.Write(new Rule("[yellow]Конфигурация ресурсов состояния[/]").Justify(Justify.Left));

                horneySecurity = AnsiConsole.Prompt(
                    new TextPrompt<double>("Базальная безопасность HorneyOS [green](0-100%)[/]:")
                        .DefaultValue(PsychicConstants.RuntimePresets.DefaultHorneySecurity)
                        .ValidationErrorMessage("[red]Введите корректное число процентов![/]")
                        .Validate(n => n is >= 0 and <= 100));

                horneyAlienation = AnsiConsole.Prompt(
                    new TextPrompt<double>("Начальный налог на отчуждение [red](sp/tick)[/]:")
                        .DefaultValue(PsychicConstants.RuntimePresets.DefaultHorneyAlienation));

                freudInitialTension = AnsiConsole.Prompt(
                    new TextPrompt<double>("Начальное напряжение катексиса FreudOS [purple](0-100%)[/]:")
                        .DefaultValue(PsychicConstants.RuntimePresets.DefaultFreudTension)
                        .Validate(n => n is >= 0 and <= 100));
                break;
        }

        // Инициализируем ядра (они сами подпишутся на шину внутри своих конструкторов!)
        HorneyCore = new HorneyMind(horneySecurity, horneyVitality, horneyAlienation);
        JungCore = new JungianMind();
        AdlerCore = new AdlerianMind(adlerInferiority, adlerSocialInterest, adlerStyle);
        FreudCore = new TripartitePsyche(freudInitialLibido, freudInitialTension);

        AnsiConsole.MarkupLine($"\n[bold green] Настройки зафиксированы. Режим: {simMode}.[/]");
        AnsiConsole.MarkupLine($"[bold yellow]Запуск мониторинга через {PsychicConstants.Simulation.StartupDelayMs / 1000.0:F1} секунды...[/]");
        Thread.Sleep(PsychicConstants.Simulation.StartupDelayMs);

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
            for (int heartbeat = 1; heartbeat <= PsychicConstants.Simulation.TotalHeartbeats; heartbeat++)
            {
                if (globalPanic) break;

                Thread.Sleep(PsychicConstants.Simulation.HeartbeatDelayMs);

                try
                {
                    // Единственный генератор активности — Id производит влечение.
                    var rand = new Random().Next(100);
                    IDrive drive = rand > PsychicConstants.Simulation.ThanatosDriveChanceThreshold ? new ThanatosDrive() : new ErosDrive();
                    var defense = rand > PsychicConstants.Simulation.RepressionMechanismThreshold ? DefenseMechanism.Repression : DefenseMechanism.Sublimation;

                    // Этот метод внутри бросит событие вытеснения в шину, если Superego наложит вето
                    FreudCore.ProcessIdImpulse(drive, defense, out currentFreudLog, heartbeat);

                    // Триггер активации Psychoanalyst на основе вместимости Тени
                    if (JungCore.GetShadowBufferString().Split(' ', StringSplitOptions.RemoveEmptyEntries).Length > PsychicConstants.Jung.MaxShadowBufferCapacity)
                    {
                        // Аварийное вмешательство аналитика прямо посреди системных пульсаций!
                        analyst.PerformReflectionTherapy(JungCore, HorneyCore, heartbeat);

                        // Небольшая задержка, чтобы пользователь успел прочитать лог интервенции на дашборде
                        Thread.Sleep(PsychicConstants.Simulation.InterventionDelayMs);
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
        psycheTable.AddRow("Либидо Эго (Сознание)", $"[yellow]{JungCore.EgoLibido:F1} cx[/]");
        psycheTable.AddRow("Либидо Бессознательного", $"[purple]{JungCore.UnconsciousLibido:F1} cx[/]");
        psycheTable.AddRow("Буфер тени (ShadowDesires)", $"[grey39]{JungCore.GetShadowBufferString()}[/]");

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
