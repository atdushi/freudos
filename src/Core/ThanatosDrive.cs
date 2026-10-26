namespace FreudOS.Core
{
    public class ThanatosDrive : IDrive
    {
        public DriveType Type => DriveType.Thanatos;

        public string TechnicalLabel => "Thanatos_Aggression_To_Prod";
        
        public double EnergyOutput => 25.0; // Разрушительные импульсы сильнее давят на буфер
    }
}
