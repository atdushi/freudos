using FreudOS.HorneyOS;
using FreudOS.PsycheOS;
using Spectre.Console;
using System.Reflection;

/// <summary>
/// Инструмент глубокого анализа и интервенции. 
/// Обходит инкапсуляцию рантаймов через Reflection для стабилизации буферов.
/// </summary>
public class Psychoanalyst
{
    /// <summary>
    /// Производит сессию интервенции в реактивное ядро Юнга.
    /// </summary>
    public void PerformReflectionTherapy(IndividualMind psycheCore, HorneyMind horneyCore, int currentPulse)
    {
        AnsiConsole.MarkupLine($"\n[bold magenta][[Analyst]][/] Инициализация Reflection-интервенции на пульсе {currentPulse}...");

        try
        {
            // ПРОВЕРКА СОПРОТИВЛЕНИЯ: Гордость HorneyOS может заблокировать Аналитика
            if (horneyCore.AlienationCost > 50.0)
            {
                throw new ResistanceException(
                    "Критический уровень отчуждения в макро-психике! Невротическая гордость заблокировала Reflection-интервенцию. " +
                    "Система отвергает дебаг буфера Тени, защищая свой идеализированный образ.");
            }

            Type psycheType = psycheCore.GetType();

            // Извлекаем приватное поле буфера через метаданные Reflection
            FieldInfo? shadowBufferField = psycheType.GetField("_shadowDesiresBuffer", BindingFlags.NonPublic | BindingFlags.Instance);

            // Извлекаем приватные автосвойства либидо для ребалансировки энергии
            PropertyInfo? egoProperty = psycheType.GetProperty("EgoLibido", BindingFlags.Public | BindingFlags.Instance);
            PropertyInfo? unconsciousProperty = psycheType.GetProperty("UnconsciousLibido", BindingFlags.Public | BindingFlags.Instance);

            if (shadowBufferField == null)
            {
                AnsiConsole.MarkupLine("[bold red][[Analyst Error]][/] Ошибка: Не удалось получить доступ к структуре подсознания.");
                return;
            }

            // Получаем ссылку на реальный List<string> внутри запущенного ядра Юнга
            var internalList = (List<string>)shadowBufferField.GetValue(psycheCore)!;

            if (internalList.Count > 0)
            {
                AnsiConsole.MarkupLine($"[bold magenta][[Analyst]][/] Обнаружен раздутый буфер Тени ({internalList.Count} аффектов). Производится интеграция и очистка...");

                // Принудительно очищаем коллекцию прямо в памяти рантайма!
                internalList.Clear();
                psycheCore.InjectIntoShadow("Интеграция_Тени_Успешна;");

                // Возвращаем вытесненную энергию обратно в Эго-сознание (Ребаланс Либидо)
                if (egoProperty != null && unconsciousProperty != null)
                {
                    // Обходим private set через Reflection-подмену (backing field или GetSetMethod)
                    var egoBackingField = psycheType.GetField("<EgoLibido>k__BackingField", BindingFlags.NonPublic | BindingFlags.Instance);
                    var uncBackingField = psycheType.GetField("<UnconsciousLibido>k__BackingField", BindingFlags.NonPublic | BindingFlags.Instance);

                    if (egoBackingField != null && uncBackingField != null)
                    {
                        egoBackingField.SetValue(psycheCore, 60.0);       // Восстанавливаем контроль Эго
                        uncBackingField.SetValue(psycheCore, 40.0);       // Разгружаем бессознательное
                    }
                }

                AnsiConsole.MarkupLine("[bold green][[Analyst]][/] Успех! Буфер Тени очищен, Либидо Эго ребалансировано до базовых 60.0 cx.");

                // Публикуем событие успешной терапии в общую шину
                PsychicDataBus.Instance.Publish(new SecurityDroppedEvent(horneyCore.Security + 15.0), currentPulse); // Временное терапевтическое облегчение
            }
            else
            {
                AnsiConsole.MarkupLine("[bold magenta][[Analyst]][/] Буфер Тени чист. Вмешательство не требуется.");
            }
        }
        catch (ResistanceException ex)
        {
            AnsiConsole.MarkupLine($"\n[blink bold red][[Spectre.Console - Resistance]] Исключение: {ex.Message}[/]");
            AnsiConsole.MarkupLine("[bold magenta][[Analyst]][/] Интервенция провалена. Требуется предварительная проработка ShouldsTyrannyDecorator.");
        }
        catch (Exception ex)
        {
            AnsiConsole.MarkupLine($"[bold red][[Runtime Panic]][/] Критический сбой рефлексии Аналитика: {ex.Message}");
        }
    }
}