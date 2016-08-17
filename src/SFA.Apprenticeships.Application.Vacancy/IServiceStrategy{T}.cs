namespace SFA.Apprenticeships.Application.Vacancy
{
    using Interfaces.Service;

    public interface IServiceStrategy<T>
    {
        StrategyResult Execute(T input);
    }
}