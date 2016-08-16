namespace SFA.Apprenticeships.Application.Vacancy
{
    public interface IServiceStrategy<T> where T : class
    {
        StrategyResult<T> Execute(T input);
    }
}