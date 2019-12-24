namespace SCAS.Domain.Competition
{
    public enum CompetitionType
    {
        MultiSport, 
        UniqueSport
    }

    public abstract class CompetitionBase
    {
        public string Id { get; internal set; }
        public string Name { get; internal set; }
        public CompetitionType Type { get; internal set; }
    }
}
