using FreudOS.PsycheOS;

namespace FreudOS.Core;

/// <summary>
/// Трехкомпонентная структура распределения и вытеснения катексисов.
/// </summary>
public class TripartitePsyche
{
    private double _libidoPool;
    private double _cathexisTension;
    private double _repressionCost = 2.0;
    private readonly List<IDrive> _repressedBuffer = new();

    public TripartitePsyche(double initialLibido, double initialTension)
    {
        _libidoPool = initialLibido;
        _cathexisTension = initialTension;
    }

    /// <summary>
    /// Основной конвейер обработки импульсов Id через защитные механизмы Эго.
    /// </summary>
    public void ProcessIdImpulse(IDrive drive, DefenseMechanism chosenDefense, out string log, int pulse)
    {
        // Каждое проходящее влечение (даже сублимированное) создает микро-напряжение удержания подсознания
        double dynamicCost = _repressionCost * (_repressedBuffer.Count + 1);
        _libidoPool -= dynamicCost * 0.05;
        _cathexisTension += 0.5 * (_repressedBuffer.Count + 1);

        if (_libidoPool <= 0) _libidoPool = 0;

        // Проверка критического взрыва котла при накоплении вытесненного материала
        if (_cathexisTension > 90.0)
        {
            PsychicDataBus.Instance.Publish(new MacroPsychicPanicEvent("Крах гидравлического котла Фрейда!"), pulse);
            throw new LibidoLeakException("Гидравлический взрыв Фрейда!");
        }

        // Если Эрос сублимируется — напряжение падает напрямую в реальности
        if (drive.Type == DriveType.Eros && chosenDefense == DefenseMechanism.Sublimation)
        {
            _cathexisTension = Math.Max(0.0, _cathexisTension - drive.EnergyOutput);
            log = $"[green][[Ego]][/] Реализован {drive.TechnicalLabel}. Давление сброшено.";
            return;
        }

        if (chosenDefense == DefenseMechanism.Repression)
        {
            _repressedBuffer.Add(drive);
            _repressionCost += 1.5;
            _cathexisTension += drive.EnergyOutput * 0.6;
            log = $"[maroon][[Bus -> Вытеснение]][/] {drive.TechnicalLabel} вытеснен Эго.";

            // ПУБЛИКАЦИЯ В ШИНУ: запускает каскад Юнга, Хорни и Адлера реактивно!
            PsychicDataBus.Instance.Publish(new IdImpulseRepressedEvent(drive.TechnicalLabel, drive.EnergyOutput, drive.Type), pulse);
        }
        else
        {
            // Сублимация Танатоса или заблокированного Эроса перерабатывает энергию в пул либидо Эго
            _cathexisTension = Math.Max(0.0, _cathexisTension - 10.0);
            _libidoPool += drive.EnergyOutput * 0.3;
            log = $"[cyan3][[Ego]][/] Сублимация {drive.TechnicalLabel} во благо Энергии.";
        }
    }

    public double GetTension() => _cathexisTension;
    public double GetLibido() => _libidoPool;
    public int GetRepressedCount() => _repressedBuffer.Count;
}