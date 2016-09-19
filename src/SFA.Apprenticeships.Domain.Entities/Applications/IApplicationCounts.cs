namespace SFA.Apprenticeships.Domain.Entities.Applications
{
    public interface IApplicationCounts
    {
        int NewApplications { get; }
        int AllApplications { get; }
    }

    public class ZeroApplicationCounts : IApplicationCounts
    {
        public int NewApplications { get { return 0; } }
        public int AllApplications { get { return 0; } }
    }

}
