namespace FreudOS.PsycheOS;

/// <summary>
/// Маркерный интерфейс для всех ментальных событий шины.
/// </summary>
public interface IPsychicEvent { }

#region ── КОНТРАКТЫ КРОСС-СИСТЕМНЫХ СОБЫТИЙ ──

public record IdImpulseRepressedEvent(string Label, double EnergyCost, Core.DriveType DriveType) : IPsychicEvent;

public record SecurityDroppedEvent(double CurrentSecurity) : IPsychicEvent;

public record MacroPsychicPanicEvent(string Reason) : IPsychicEvent;

#endregion

/// <summary>
/// Строго типизированная реактивная шина данных (Pub/Sub) с поддержкой CSV-логирования.
/// </summary>
public class PsychicDataBus
{
    private static readonly Lazy<PsychicDataBus> _instance = new(() => new PsychicDataBus());
    public static PsychicDataBus Instance => _instance.Value;

    private readonly Dictionary<Type, List<Action<IPsychicEvent>>> _subscribers = new();
    private readonly string _logPath = Path.Combine("generated", "psychic_bus_log.csv");

    private PsychicDataBus()
    {
        // Инициализируем директорию и структуру CSV-лога при создании шины
        try
        {
            var directory = Path.GetDirectoryName(_logPath);
            if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
            File.WriteAllText(_logPath, "Pulse,SourceRuntime,EventType,Payload\n");
        }
        catch { }
    }

    /// <summary>
    /// Регистрация реактивного подписчика на определенный тип события.
    /// </summary>
    public void Subscribe<TEvent>(Action<TEvent> handler) where TEvent : IPsychicEvent
    {
        var t = typeof(TEvent);
        if (!_subscribers.ContainsKey(t))
        {
            _subscribers[t] = [];
        }
        _subscribers[t].Add(ev => handler((TEvent)ev));
    }

    /// <summary>
    /// Публикация события в эфир макро-психики с фиксацией в физическом логе.
    /// </summary>
    public void Publish<TEvent>(TEvent @event, int pulse = 0) where TEvent : IPsychicEvent
    {
        string src = "Unknown", payload = "";
        switch (@event)
        {
            case IdImpulseRepressedEvent e:
                src = "FreudOS";
                payload = $"\"{e.Label} repressed; energy={e.EnergyCost}\"";
                break;
            case SecurityDroppedEvent e:
                src = "HorneyOS";
                payload = $"\"Security drop to {e.CurrentSecurity:F1}%\"";
                break;
            case MacroPsychicPanicEvent e:
                src = "System";
                payload = $"\"PANIC: {e.Reason}\"";
                break;
        }

        try 
        { 
            File.AppendAllText(_logPath, $"{pulse},{src},{typeof(TEvent).Name},{payload}\n"); 
        }
        catch (IOException)
        {
            // Защита от блокировок файла при высокоинтенсивных тиках
        }

        if (_subscribers.TryGetValue(typeof(TEvent), out var handlers))
        {
            foreach (var h in handlers)
            {
                h(@event);
            }
        }
    }
}
