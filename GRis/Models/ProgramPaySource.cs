namespace GRis.Models
{
    public class ProgramPaySource
    {
        public int ProgramPaySourceId { get; set; }

        public int ProgramId { get; set; }

        public int PaySourceId { get; set; }

        public double Percentage { get; set; }
    }
}