namespace FreudOS.PsycheOS;

public class AnimaProxy : IComplexSubscriber
{
    private readonly IndividualMind _mind;
    public string ComplexName => "Anima_Proxy";

    public AnimaProxy(IndividualMind mind) => _mind = mind;

    public void OnIdImpulseIntercepted(IdImpulseRepressedEvent ev)
    {
        // Анима («душа») реагирует на вытесненные созидательные или эмоциональные импульсы (Eros)
        if (ev.DriveType == Core.DriveType.Eros && !_mind.GetShadowBufferString().EndsWith("Аффект_Анимы(Иррациональная_Идеализация); "))
        {
            _mind.InjectIntoShadow("Аффект_Анимы(Иррациональная_Идеализация);");
        }
    }
}