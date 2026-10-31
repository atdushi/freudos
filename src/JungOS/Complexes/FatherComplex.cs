using FreudOS.Core;

namespace FreudOS.JungOS.Complexes;

public class FatherComplex : IComplexSubscriber
{
    private readonly JungianMind _mind;

    public string ComplexName => "Father_Complex";

    public FatherComplex(JungianMind mind) => _mind = mind;

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
