namespace ApprenticeshipScraper.CmdLine
{
    using System;
    using System.Linq;

    using ApprenticeshipScraper.CmdLine.Models;
    using ApprenticeshipScraper.CmdLine.Services;
    using ApprenticeshipScraper.CmdLine.Settings;

    using Fclp;

    using TinyIoC;

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
                OutputSettings(container);
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

        private static void OutputSettings(TinyIoCContainer container)
        {
            var settings = container.Resolve<IGlobalSettings>();

            foreach (var propertyInfo in settings.GetType().GetProperties())
            {
                var getter = propertyInfo.GetGetMethod();
                Console.WriteLine($" {propertyInfo.Name} = {getter.Invoke(settings, new object[] {})}");
            }
            Console.WriteLine();
        }
    }
}