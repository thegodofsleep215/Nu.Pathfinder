namespace Nu.OfficerMiniGame.Dal.Dto
{
    public class CrewMemberStats
    {
        public string Name { get; set; }

        public string Title { get; set; }

        public CrewSkills Skills { get; set; } = new CrewSkills();
    }
}
