namespace  Nu.OfficerMiniGame
{
    public interface ISupply
    {
        SupplyType SupplyType { get; set; }
        int UnitsSupplyPerPoint { get; set; }
        int UnitsSupplyRemaining { get; }

        int AdjustSupplies(int amount);

        void OnExhuastion();
    }
}