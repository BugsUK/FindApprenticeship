namespace SFA.Apprenticeships.Infrastructure.Processes.Extensions
{
    using SFA.Infrastructure.Interfaces;

    public static class EtlValidationLoggingExtensions
    {
        public static void Warn(this ILogService logService, bool strictEtlValidation, string message, params object[] args)
        {
            if (strictEtlValidation)
            {
                logService.Warn(message, args);
            }
            else
            {
                logService.Info(message, args);
            }
        }
    }
}