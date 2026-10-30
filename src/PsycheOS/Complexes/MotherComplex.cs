namespace FreudOS.PsycheOS;

public class MotherComplex : IComplexSubscriber
{
    private readonly IndividualMind _m;
    public string ComplexName => "Mother_Complex";
    public MotherComplex(IndividualMind m) => _m = m;

    public void OnIdImpulseIntercepted(IdImpulseRepressedEvent ev)
    {
        // Добавляем проверку: если этот аффект уже есть в буфере последним, не дублируем его
        if (ev.EnergyCost > 10 && !_m.GetShadowBufferString().EndsWith("Аффект_Матери(Регрессия_Эго); "))
        {
            _m.InjectIntoShadow("Аффект_Матери(Регрессия_Эго);");
        }
    }
}
