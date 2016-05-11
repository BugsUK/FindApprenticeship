namespace SFA.Apprenticeships.Data.Migrate.Faa
{
    using System.Threading;

    public interface IMigrationProcessor
    {
        void Process(CancellationToken cancellationToken);
    }
}