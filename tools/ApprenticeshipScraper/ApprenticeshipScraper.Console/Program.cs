namespace ApprenticeshipScraper.CmdLine
{
    using System;
    using System.Linq;

    using ApprenticeshipScraper.CmdLine.Models;
    using ApprenticeshipScraper.CmdLine.Services;

    using Fclp;

    public static class Program
    {
        private static void Main(string[] args)
        {
            Console.WriteLine($"FAA web scraper {AssemblyInfo.GetAssemblyVersion()}");
            Console.WriteLine();

            var p = new FluentCommandLineParser<ApplicationArguments>();

            p.Setup(arg => arg.Site).As('s', "site").Required();
            p.Setup(arg => arg.Directory).As('d', "directory").Required();

            var result = p.Parse(args);

            if (result.HasErrors == false)
            {
                var container = IoC.RegisterDependencies();
                container.Resolve<Application>().Run(p.Object);
            }
            else
            {
                Console.Write($" Usage: {AssemblyInfo.GetExeName()}");
                foreach (var option in p.Options.Where(x => x.IsRequired))
                {
                    Console.Write($" /{option.ShortName} = <{option.LongName}>");
                }
                Console.WriteLine();
                Console.WriteLine(" Site : PRE, PROD");
                Console.WriteLine();
                Console.WriteLine();
                Console.WriteLine(result.ErrorText);
            }
        }
    }
}