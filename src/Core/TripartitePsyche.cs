namespace FreudOS.Core;

/// <summary>
/// Трехкомпонентное ядро психики по Фрейду.
/// </summary>
public class TripartitePsyche
{
    // Внутренние ресурсы состояния
    private double _libidoPool = 500.0;          // Общий объем энергии (Катексисы)
    private double _cathexisTension = 15.0;      // Процент напряжения (0% - 100%)
    private double _repressionCost = 2.0;        // Налог на удержание вытесненного (cx/tick)

    // Буфер вытесненных, но неотработанных импульсов Id
    private readonly List<string> _repressedUnconsciousBuffer = new();

    /// <summary>
    /// Системный тик ядра FreudOS.
    /// </summary>
    public void OnRuntimeTick()
    {
        // 1. Постоянный налог на удержание старых конфликтов выжигает либидо из пула Эго
        double dynamicCost = _repressionCost * (_repressedUnconsciousBuffer.Count + 1);
        _libidoPool -= dynamicCost;

        // 2. Рост напряжения из-за пассивного давления Id
        _cathexisTension += 2.5 * (_repressedUnconsciousBuffer.Count + 1);

        // 3. Проверка критических порогов
        if (_libidoPool <= 0.0)
        {
            _libidoPool = 0.0;
            Console.WriteLine("[FreudOS - Panic] Эго полностью обесточено (0 cx). Наступил кататонический ступор.");
            return;
        }

        if (_cathexisTension > 90.0)
        {
            // Гидравлический прорыв буфера
            throw new LibidoLeakException(
                $"[Libido Leak] Напряжение катексиса достигло {_cathexisTension:F1}%. " +
                $"Вытесненный буфер переполнен ({_repressedUnconsciousBuffer.Count} объектов). " +
                $"Импульсы Id прорвали защиту Эго через конверсионный симптом!");
        }
    }

    /// <summary>
    /// Обработка входящего импульса со стороны Id (хочу здесь и сейчас).
    /// </summary>
    public void ProcessIdImpulse(string rawImpulse, DefenseMechanism chosenDefense)
    {
        Console.WriteLine($"[Id] Сгенерирован сырой импульс влечения: '{rawImpulse}'");

        // Superego оценивает импульс на соответствие жестким внутренним правилам кодовой базы
        bool isDeemedSinful = rawImpulse.Contains("Thanatos") || rawImpulse.Contains("Aggression") || rawImpulse.Contains("Taboo");

        if (!isDeemedSinful)
        {
            // Эго удовлетворяет импульс напрямую в реальности
            Console.WriteLine($"[Ego] Импульс безопасен. Разрешено прямое выполнение. Напряжение падает.");
            _cathexisTension = Math.Max(0.0, _cathexisTension - 10.0);
            return;
        }

        // Если Superego накладывает вето, Эго обязано задействовать диспетчер защит
        Console.WriteLine("[Superego] ВНИМАНИЕ: Обнаружено нарушение спецификации идеального поведения! Доступ к выполнению заблокирован.");

        switch (chosenDefense)
        {
            case DefenseMechanism.Repression:
                // Импульс вытесняется в буфер бессознательного
                _repressedUnconsciousBuffer.Add(rawImpulse);
                _repressionCost += 4.5; // Налог на постоянное удержание растет
                _cathexisTension += 15.0; // Локальное давление в котле увеличивается
                Console.WriteLine($"[Ego - Защита] Механизм: Вытеснение. Импульс заморожен в неразмеченной памяти. Налог на удержание вырос до {_repressionCost} cx/tick.");
                break;

            case DefenseMechanism.Sublimation:
                // Энергия легально перенаправляется в рефакторинг или написание тестов
                _cathexisTension = Math.Max(0.0, _cathexisTension - 20.0);
                _libidoPool += 10.0; // Продуктивная деятельность возвращает часть катексиса в пул
                Console.WriteLine($"[Ego - Защита] Механизм: Сублимация! Энергия влечения перенаправлена на написание кода на C# 13. Напряжение успешно сброшено.");
                break;

            case DefenseMechanism.Projection:
                // Сброс импульса, перевод его в строковый формат для отправки в другие рантаймы
                Console.WriteLine($"[Ego - Защита] Механизм: Проекция. Ошибка объявлена внешним багом. Импульс подготовлен к сериализации в AdlerOS/HorneyOS.");
                // Здесь будет вызов шины данных
                break;
        }
    }

    // Геттеры для мониторинга Spectre.Console
    public double GetTension() => _cathexisTension;
    public double GetLibido() => _libidoPool;
    public int GetRepressedCount() => _repressedUnconsciousBuffer.Count;
}
