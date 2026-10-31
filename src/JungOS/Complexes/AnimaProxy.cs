using FreudOS.Core;

namespace FreudOS.JungOS.Complexes;

public class AnimaProxy : IComplexSubscriber
{
    private readonly JungianMind _mind;

    public string ComplexName => "Anima_Proxy";

    public AnimaProxy(JungianMind mind) => _mind = mind;

    public void OnIdImpulseIntercepted(IdImpulseRepressedEvent ev)
    {
        // Анима («душа») реагирует на вытесненные созидательные или эмоциональные импульсы (Eros)
        if (ev.DriveType == Core.Drive.DriveType.Eros && !_mind.GetShadowBufferString().EndsWith("Аффект_Анимы(Иррациональная_Идеализация); "))
        {
            _mind.InjectIntoShadow("Аффект_Анимы(Иррациональная_Идеализация);");
        }
    }
}