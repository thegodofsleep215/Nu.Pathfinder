namespace pfsim.Officer
{
    public class UnrulyCrewEvent
    {
        public int Roll { get; set; }

        public bool IsSerious { get; set; }

        public override string ToString()
        {
            var type = IsSerious ? "serious" : "regular";
            return $"The crew is getting unruly. Roll:{Roll} IsSerious:{IsSerious}";
        }
    }

}
