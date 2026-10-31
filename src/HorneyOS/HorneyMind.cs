using FreudOS.Core;

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
            AlienationCost += PsychicConstants.Horney.AlienationIncrementStep;
            Vitality = Math.Max(0.0, Vitality - (AlienationCost * PsychicConstants.Horney.VitalityDegradationFactor));

            if (AlienationCost > PsychicConstants.Horney.AlienationTriggerThreshold)
            {
                Security = Math.Max(0.0, Security - PsychicConstants.Horney.SecurityDecrementStep);
                // Публикуем падение безопасности дальше по цепочке — Адлер подхватит!
                PsychicDataBus.Instance.Publish(new SecurityDroppedEvent(Security));
            }

            if (Security < PsychicConstants.Horney.SystemPanicThreshold)
            {
                throw new ResistanceException("Критический уровень базальной тревоги! Рантайм Хорни аварийно остановлен.");
            }
        });
    }
}