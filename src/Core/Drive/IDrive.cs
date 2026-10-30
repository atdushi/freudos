namespace FreudOS.Core.Drive;

/// <summary>
/// Строго типизированный интерфейс влечения Id.
/// </summary>
public interface IDrive
{
    DriveType Type { get; }

    string TechnicalLabel { get; }

    double EnergyOutput { get; } // Сколько катексисов (cx) генерирует этот импульс
}