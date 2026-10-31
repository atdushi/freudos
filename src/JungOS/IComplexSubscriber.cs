using FreudOS.Core;

namespace FreudOS.JungOS;

/// <summary>
/// Интерфейс для автономных психических комплексов Юнга.
/// </summary>
public interface IComplexSubscriber
{
    string ComplexName { get; }

    void OnIdImpulseIntercepted(IdImpulseRepressedEvent ev);
}
