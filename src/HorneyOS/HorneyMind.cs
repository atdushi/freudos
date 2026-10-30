using FreudOS.PsycheOS;

namespace FreudOS.HorneyOS;

public class HorneyMind
{
    public double Security { get; private set; }
    public double Vitality { get; private set; }
    public double AlienationCost { get; private set; }

    public HorneyMind(double security, double vitality, double alienationCost)
    {
        Security = security;
        Vitality = vitality;
        AlienationCost = alienationCost;

        // Хорни больше не считает тики! Она реагирует на вытеснения Фрейда.
        // Каждое вытесненное бессознательное влечение повышает отчужденность и роняет безопасность.
        PsychicDataBus.Instance.Subscribe<IdImpulseRepressedEvent>(ev =>
        {
            AlienationCost += 1.5;
            Vitality = Math.Max(0.0, Vitality - (AlienationCost * 0.05));

            if (AlienationCost > 25.0)
            {
                Security = Math.Max(0.0, Security - 2.0);
                // Публикуем падение безопасности дальше по цепочке — Адлер подхватит!
                PsychicDataBus.Instance.Publish(new SecurityDroppedEvent(Security));
            }

            if (Security < 20.0)
            {
                throw new ResistanceException("Критический уровень базальной тревоги! Рантайм Хорни аварийно остановлен.");
            }
        });
    }
}