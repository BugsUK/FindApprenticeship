namespace SFA.Apprenticeships.Application.Vacancy
{
    public class StrategyResult<T> where T : class
    {
        public string Code { get; private set; }
        public T Result { get; private set; }

        public StrategyResult(string code, T result)
        {
            Code = code;
            Result = result;
        }

        public StrategyResult(string code) : this(code, null) {}
    }
}