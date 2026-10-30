using FreudOS.Core;

namespace FreudOS.Jung;

public class MotherComplex : IComplexSubscriber
{
    private readonly JungMind _mind;
    
    public string ComplexName => "Mother_Complex";

    public MotherComplex(JungMind mind) => _mind = mind;

    public void OnIdImpulseIntercepted(IdImpulseRepressedEvent ev)
    {
        // Добавляем проверку: если этот аффект уже есть в буфере последним, не дублируем его
        if (ev.EnergyCost > 10 && !_mind.GetShadowBufferString().EndsWith("Аффект_Матери(Регрессия_Эго); "))
        {
            _mind.InjectIntoShadow("Аффект_Матери(Регрессия_Эго);");
        }
    }
}
