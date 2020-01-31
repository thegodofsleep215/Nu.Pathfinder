namespace Nu.OfficerMiniGame.Dal.Dto
{
    public class LoadoutCrewMember
    {
        public string Key { get => $"{Name} - {DutyType}"; }

        public string Name { get; set; }

        public DutyType DutyType { get; set; }

        public bool IsAssistant { get; set; }
    }
}
