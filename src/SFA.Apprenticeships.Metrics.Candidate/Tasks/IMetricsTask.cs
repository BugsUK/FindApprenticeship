namespace SFA.Apprenticeships.Metrics.Candidate.Tasks
{
    public interface IMetricsTask
    {
        string TaskName { get; }

        void Run(); 
    }
}