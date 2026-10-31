using FreudOS.Core;

namespace FreudOS.AdlerOS;

public class AdlerianMind
{
    public double InferiorityIndex { get; private set; }

    public double SocialInterest { get; private set; }

    public AdlerLifeStyle CurrentStyle { get; private set; }

    public AdlerianMind(double inferiorityIndex, double socialInterest, AdlerLifeStyle currentStyle = AdlerLifeStyle.SociallyUseful)
    {
        InferiorityIndex = inferiorityIndex;
        SocialInterest = socialInterest;
        CurrentStyle = currentStyle;

        // Чистая реакция на событие падения безопасности из Хорни
        PsychicDataBus.Instance.Subscribe<SecurityDroppedEvent>(ev =>
        {
            if (ev.CurrentSecurity < PsychicConstants.Horney.SecurityDroppedThreshold)
            {
                InferiorityIndex = Math.Min(100.0, InferiorityIndex + PsychicConstants.Adler.InferiorityIncrementStep);
                SocialInterest = Math.Max(0.0, SocialInterest - PsychicConstants.Adler.SocialInterestDecrementStep);
                CurrentStyle = AdlerLifeStyle.Ruling;
            }
        });
    }
}
