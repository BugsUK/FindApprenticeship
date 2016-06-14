namespace ApprenticeshipScraper.CmdLine.Services
{
    using System;
    using System.Linq;

    using ApprenticeshipScraper.CmdLine.Extensions;
    using ApprenticeshipScraper.CmdLine.Models;
    using ApprenticeshipScraper.CmdLine.Steps;

    using TinyIoC;

    internal sealed class Application
    {
        private readonly TinyIoCContainer container;

        public Application(TinyIoCContainer container)
        {
            this.container = container;
        }

        public void Run(ApplicationArguments arguments)
        {
            foreach (var step in this.container.ResolveAll<IStep>())
            {
                Console.Write($"Run {step.NiceName()}? [Y] ");
                var input = Console.ReadLine()?.ToUpper();
                if (string.IsNullOrEmpty(input) || input == "Y")
                {
                    new TimedStep(step).Run(arguments);
                }
                else
                {
                    Console.WriteLine();
                }
            }
        }
    }
}