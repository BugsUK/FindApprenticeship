namespace SFA.Apprenticeships.Data.Migrate.Faa
{
    using System.Threading;
    using System.Threading.Tasks;

    public interface IMigrationProcessor
    {
        Task Process(CancellationToken cancellationToken);
    }
}