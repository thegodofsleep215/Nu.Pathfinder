namespace pfsim.Officer
{
    public class WatchResultEvent
    {
        public bool Success { get; set; }

        public override string ToString()
        {
            return Success ? "The watch was successfull." : "The watch was a failure.";
        }
    }

}
