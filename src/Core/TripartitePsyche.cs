namespace FreudOS.Core;

public class TripartitePsyche
{
    private double _libidoPool = 500.0;
    private double _cathexisTension = 15.0;
    private double _repressionCost = 2.0;
    private readonly List<IDrive> _repressedBuffer = new(); // Теперь хранит строго типизированные влечения

    public void OnRuntimeTick()
    {
        double dynamicCost = _repressionCost * (_repressedBuffer.Count + 1);
        _libidoPool -= dynamicCost * 0.2;
        _cathexisTension += 1.5 * (_repressedBuffer.Count + 1);

        if (_libidoPool <= 0) _libidoPool = 0;

        if (_cathexisTension > 90.0)
        {
            throw new LibidoLeakException(
                $"Гидравлический взрыв! Напряжение катексиса ({_cathexisTension:F1}%) прорвало вытесненный буфер ({_repressedBuffer.Count} объектов). Конверсионный симптом!");
        }
    }

    /// <summary>
    /// Обработка входящего влечения вместо сырой строки.
    /// </summary>
    public void ProcessIdImpulse(IDrive drive, DefenseMechanism chosenDefense, out string log)
    {
        // Superego автоматически блокирует Thanatos (агрессию)
        bool isDeemedSinful = drive.Type == DriveType.Thanatos;

        if (!isDeemedSinful)
        {
            // Эрос безопасно разряжается, снижая общее напряжение системы
            _cathexisTension = Math.Max(0.0, _cathexisTension - drive.EnergyOutput);
            log = $"[green][[Ego]][/] Влечение '{drive.TechnicalLabel}' успешно реализовано. Напряжение упало на {drive.EnergyOutput} cx.";
            return;
        }

        // Если Superego накладывает вето, Эго применяет выбранную защиту
        if (chosenDefense == DefenseMechanism.Repression)
        {
            _repressedBuffer.Add(drive); // Сохраняем типизированный объект влечения
            _repressionCost += 2.5;
            _cathexisTension += drive.EnergyOutput * 0.8; // Рост напряжения пропорционален силе импульса
            log = $"[orange1][[Superego Вето!]][/] [maroon]Вытеснение:[/] {drive.TechnicalLabel} подавлен. Налог на удержание: [bold]{_repressionCost}[/] cx/tick.";
        }
        else
        {
            // Сублимация утилизирует деструктивную энергию влечения во благо пула либидо
            _cathexisTension = Math.Max(0.0, _cathexisTension - 15.0);
            _libidoPool += drive.EnergyOutput * 0.5;
            log = $"[orange1][[Superego Вето!]][/] [cyan3]Сублимация![/] Разрушительный {drive.TechnicalLabel} перенаправлен на рефакторинг легаси-кода.";
        }
    }

    public double GetTension() => _cathexisTension;
    public double GetLibido() => _libidoPool;
    public int GetRepressedCount() => _repressedBuffer.Count;
    public double GetRepressionCost() => _repressionCost;
}