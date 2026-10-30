namespace FreudOS.Core
{
    /// <summary>
    /// Централизованный манифест констант, пресетов и пороговых значений макро-психики PsychOS.
    /// </summary>
    public static class PsychicConstants
    {
        /// <summary>
        /// Константы базовой конфигурации и пресетов инициализации рантайма.
        /// </summary>
        public static class RuntimePresets
        {
            // Дефолтные параметры (Стабильный режим)
            public const double DefaultHorneySecurity = 75.0;
            public const double DefaultHorneyVitality = 90.0;
            public const double DefaultHorneyAlienation = 12.0;
            public const double DefaultAdlerInferiority = 45.0;
            public const double DefaultAdlerSocialInterest = 55.0;

            public const double DefaultFreudLibido = 500.0;
            public const double DefaultFreudTension = 15.0;

            // Пограничный режим
            public const double BorderlineFreudTension = 65.0;

            // Невротический режим
            public const double NeuroticHorneyAlienation = 48.0; // В шаге от Pride Lockout (>50)
            public const double NeuroticHorneySecurity = 35.0;
            public const double NeuroticHorneyVitality = 40.0;
            public const double NeuroticAdlerInferiority = 75.0;
        }
        
        /// <summary>
        /// Параметры главного цикла симуляции и генератора случайных влечений Id.
        /// </summary>
        public static class Simulation
        {
            public const int TotalHeartbeats = 50;
            // Задержки потока (мс)
            public const int StartupDelayMs = 1500;
            public const int HeartbeatDelayMs = 300;
            public const int InterventionDelayMs = 2000;

            // Вероятностные веса (0-100)
            public const int ThanatosDriveChanceThreshold = 45; // Выше 45 — Thanatos, иначе Eros
            public const int RepressionMechanismThreshold = 50; // Выше 50 — Repression, иначе Sublimation
        }
        
        /// <summary>
        /// Константы низкоуровневых влечений Id (Фрейд).
        /// </summary>
        public static class Freud
        {
            public const double ErosBaseEnergy = 15.0;
            public const double ThanatosBaseEnergy = 25.0;
            public const double MaxCathexisThreshold = 0.90;
        }

        /// <summary>
        /// Параметры реактивного ядра и комплексов Юнга (PsycheOS).
        /// </summary>
        public static class Jung
        {
            public const double MotherComplexEnergyThreshold = 10.0;
            public const int MaxShadowBufferCapacity = 5;
            public const double DefaultEgoLibidoTarget = 60.0;
        }

        /// <summary>
        /// Метрики базальной тревоги и контура безопасности (Хорни).
        /// </summary>
        public static class Horney
        {
            public const double SecurityDroppedThreshold = 65.0;
            public const double SystemPanicThreshold = 20.0;
            public const double MaxAlienationForIntervention = 50.0;
        }

        /// <summary>
        /// Метрики сверхкомпенсации индивидуальной психологии (Адлер).
        /// </summary>
        public static class Adler
        {
            public const double InferiorityIncrementStep = 15.5;
        }
    }
}
