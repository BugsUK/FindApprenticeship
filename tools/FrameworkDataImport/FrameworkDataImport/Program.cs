namespace FrameworkDataImport
{
    using System;
    using System.Linq;
    using Process;
    using SFA.Apprenticeships.Application.ReferenceData;

    public class Program
    {
        public static void Main(string[] args)
        {
            var elasticUri = new Uri(args[0]);
            var filePath = args.Last();

            var container = IoC.Initialize();

            var provider = new FrameworkDataProvider(filePath);

            var categories = provider.GetCategories().ToList();

            var frameworkDataComparer = container.GetInstance<IFrameworkDataComparer>();
            frameworkDataComparer.Compare(categories);

            var frameworkDataLoader = new FrameworkDataLoader(elasticUri);
            frameworkDataLoader.Load(categories);

            Console.WriteLine("Press any key to retrieve categories from index");
            Console.ReadKey();

            var frameworkDataReferenceDataProvider = container.GetInstance<IReferenceDataProvider>("FrameworkDataProvider");
            var providedCategories = frameworkDataReferenceDataProvider.GetCategories().ToList();
            Console.WriteLine("Provided {0} Categories", providedCategories.Count);
            var counter = 0;
            foreach (var providedCategory in providedCategories)
            {
                Console.WriteLine("{0} {1} {2}", providedCategory.CodeName, providedCategory.FullName, providedCategory.ParentCategoryCodeName);
                foreach (var subCategory in providedCategory.SubCategories)
                {
                    Console.WriteLine(" - {0} {1} {2}", subCategory.CodeName, subCategory.FullName, subCategory.ParentCategoryCodeName);
                    counter++;
                }
            }
            Console.WriteLine("Provided {0} Sub Categories", counter);

            Console.WriteLine("Press any key to exit");
            Console.ReadKey();
        }
    }
}
