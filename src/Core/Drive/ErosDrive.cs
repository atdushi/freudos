namespace FreudOS.Core.Drive
{
    public class ErosDrive : IDrive
    {
        public DriveType Type => DriveType.Eros;

        public string TechnicalLabel => "Eros_Libidinal_Fixation";
        
        public double EnergyOutput => 15.0;
    }
}
