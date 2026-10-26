namespace FreudOS.Core;

/// <summary>
/// Типы защитных механизмов Эго.
/// </summary>
public enum DefenseMechanism
{
    Repression,   // Вытеснение (запись в скрытый буфер за счет постоянной траты cx/tick)
    Sublimation,  // Сублимация (легальная конвертация cx в полезный код / фичи)
    Projection    // Проекция (сброс аффекта во внешние рантаймы, например в AdlerOS)
}