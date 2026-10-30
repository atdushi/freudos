using FreudOS.Core;

namespace FreudOS.Jung;

/// <summary>
/// Рантайм индивидуальной психики (Юнг). 
/// Работает на 100% реактивной модели через подписку на системную шину.
/// </summary>
public class JungMind
{
    // Распределение психической энергии (Libido)
    public double EgoLibido { get; private set; } = 60.0;          // Слой сознания
    public double UnconsciousLibido { get; private set; } = 40.0;  // Слой бессознательного

    // Динамический буфер накопления аффектов Личного и Коллективного Бессознательного (Тень)
    private readonly List<string> _shadowDesiresBuffer = new();
    private readonly List<IComplexSubscriber> _complexes = new();

    public JungMind()
    {
        // Инициализация и регистрация автономных комплексов («созвездий») в ядре
        _complexes.Add(new MotherComplex(this));
        _complexes.Add(new FatherComplex(this));
        _complexes.Add(new AnimaProxy(this));

        // ПОДПИСКА НА ШИНУ ДАННЫХ:
        // Рантайм Юнга слушает вытесненные импульсы Id Фрейда напрямую из эфира
        PsychicDataBus.Instance.Subscribe<IdImpulseRepressedEvent>(OnIdImpulseRepressed);
    }

    /// <summary>
    /// Реактивный перехват вытесненного материала.
    /// </summary>
    private void OnIdImpulseRepressed(IdImpulseRepressedEvent ev)
    {
        // Энергия вытесненного импульса автоматически совершает гидравлический сдвиг:
        // Часть либидо Эго (сознательного) уходит на подпитку Бессознательного
        double energyShift = ev.EnergyCost * 0.2;
        TransferLibidoToUnconscious(energyShift);

        // Оповещаем все автономные комплексы. Происходит актуализация комплексов (Constellation)
        foreach (var complex in _complexes)
        {
            complex.OnIdImpulseIntercepted(ev);
        }
    }

    /// <summary>
    /// Переток психической энергии вглубь подсознания.
    /// </summary>
    public void TransferLibidoToUnconscious(double amount)
    {
        EgoLibido = Math.Max(0.0, EgoLibido - amount);
        UnconsciousLibido += amount;
    }

    /// <summary>
    /// Инжекция нового вытесненного аффекта или символа в буфер Тени.
    /// </summary>
    public void InjectIntoShadow(string affectLog)
    {
        _shadowDesiresBuffer.Add(affectLog);
    }

    /// <summary>
    /// Собирает все слепки Тени в единую строку для вывода на дашборд.
    /// </summary>
    public string GetShadowBufferString()
    {
        return _shadowDesiresBuffer.Count == 0
            ? "Чисто (Интеграция Тени стабильна);"
            : string.Join(" ", _shadowDesiresBuffer);
    }
}