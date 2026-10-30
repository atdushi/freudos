using FreudOS.Core;

namespace FreudOS.Jung;

public class FatherComplex : IComplexSubscriber
{
    private readonly JungMind _mind;

    public string ComplexName => "Father_Complex";

    public FatherComplex(JungMind mind) => _mind = mind;

    public void OnIdImpulseIntercepted(IdImpulseRepressedEvent ev)
    {
        // Отцовский комплекс селективно перехватывает разрушительные деструктивные импульсы (Thanatos)
        if (ev.DriveType == Core.Drive.DriveType.Thanatos && ! _mind.GetShadowBufferString().EndsWith("Аффект_Отца(Проекция_Вины); "))
        {
            _mind.InjectIntoShadow("Аффект_Отца(Проекция_Вины);");
            _mind.TransferLibidoToUnconscious(5.0);
        }
    }
}
