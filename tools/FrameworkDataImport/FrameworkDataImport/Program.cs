namespace FrameworkDataImport
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using CsvHelper;
    using SFA.Apprenticeships.Application.ReferenceData;

    public class Program
    {
        public static void Main(string[] args)
        {
            var container = IoC.Initialize();
            var referenceDataProvider = container.GetInstance<IReferenceDataProvider>();

            var filePath = args.Last();

            var file = File.OpenText(filePath);

            var csv = new CsvReader(file);
            csv.Configuration.RegisterClassMap<FrameworkClassMap>();
            csv.Configuration.TrimFields = true;
            csv.Configuration.IgnoreReadingExceptions = true;

            try
            {
                var records = csv.GetRecords<Framework>().ToList();
                Console.WriteLine("Read {0} frameworks", records.Count);

                var topLevelCategories =
                    records.Select(r => new Category {FullName = r.Sector, CodeName = GetCategoryCode(r.Sector)})
                        .Distinct(new CategoryComparer())
                        .OrderBy(c => c.FullName)
                        .ToList();

                foreach (var topLevelCategory in topLevelCategories)
                {
                    topLevelCategory.CodeName = GetCategoryCode(topLevelCategory.FullName);
                    topLevelCategory.SubCategories =
                        records.Where(r => r.Sector == topLevelCategory.FullName)
                            .Select(d => d.ToCategory())
                            .OrderBy(c => c.FullName)
                            .ToList();
                }

                var categories = topLevelCategories;

                var existingCategories = referenceDataProvider.GetCategories().Select(c => c.ToCategory()).ToList();

                Console.WriteLine("EXISTING Vs NEW");
                CompareCategories(existingCategories, categories);

                Console.WriteLine("NEW Vs EXISTING");
                CompareCategories(categories, existingCategories);
            }
            catch (Exception ex)
            {
                var csvHelperExceptionData = ex.Data["CsvHelper"];
                Console.Error.WriteLine(csvHelperExceptionData);
            }

            Console.WriteLine("Press any key to exit");
            Console.ReadKey();
        }

        private static void CompareCategories(List<Category> existingCategories, List<Category> categories)
        {
            var missingCategories = existingCategories.Except(categories, new CategoryComparer()).ToList();

            if (missingCategories.Count > 0)
            {
                Console.WriteLine("The following Categories are missing:");
                foreach (var category in missingCategories)
                {
                    Console.Out.WriteLine("{0} {1} containing sub categories:", category.CodeName, category.FullName);
                    foreach (var subCategory in category.SubCategories)
                    {
                        Console.Out.WriteLine(" - {0} {1}", subCategory.CodeName, subCategory.FullName);
                    }
                }
            }

            foreach (var existingCategory in existingCategories)
            {
                var category = categories.SingleOrDefault(c => c.CodeName == existingCategory.CodeName);
                if (category != null)
                {
                    var missingSubCategories =
                        existingCategory.SubCategories.Except(category.SubCategories, new CategoryComparer()).ToList();
                    if (missingSubCategories.Count > 0)
                    {
                        Console.WriteLine("The following Sub Categories are missing from {0} {1}:", category.CodeName,
                            category.FullName);
                        foreach (var subCategories in missingSubCategories)
                        {
                            Console.Out.WriteLine(" - {0} {1}", subCategories.CodeName, subCategories.FullName);
                        }
                    }
                }
            }
        }

        private static string GetCategoryCode(string fullName)
        {
            switch (fullName)
            {
                case "Agriculture, Horticulture and Animal Care":
                    return "ALB";
                case "Arts, Media and Publishing":
                    return "ACC";
                case "Business, Administration and Law":
                    return "AHR";
                case "Construction, Planning and the Built Environment":
                    return "CST";
                case "Education and Training":
                    return "13";
                case "Engineering and Manufacturing Technologies":
                    return "MFP";
                case "Health, Public Services and Care":
                    return "PUB";
                case "Information and Communication Technology":
                    return "ITC";
                case "Leisure, Travel and Tourism":
                    return "HTL";
                case "Retail and Commercial Enterprise":
                    return "HBY";
                case "Science and Mathematics":
                    return "02";
            }

            throw new Exception(string.Format("Category {0} was not recognized", fullName));
        }

        public class CategoryComparer : IEqualityComparer<Category>
        {
            public bool Equals(Category x, Category y)
            {
                if (ReferenceEquals(x, y)) return true;
                if (ReferenceEquals(x, null)) return false;
                if (ReferenceEquals(y, null)) return false;
                if (x.GetType() != y.GetType()) return false;
                return string.Equals(x.CodeName, y.CodeName);
            }

            public int GetHashCode(Category obj)
            {
                unchecked
                {
                    return (obj.CodeName != null ? obj.CodeName.GetHashCode() : 0);
                }
            }
        }
    }
}
