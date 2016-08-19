namespace SFA.Apprenticeships.Application.Interfaces.Service
{
    public class StrategyResult
    {
        public string Code { get; private set; }

        public StrategyResult(string code)
        {
            Code = code;
        }
    }
}