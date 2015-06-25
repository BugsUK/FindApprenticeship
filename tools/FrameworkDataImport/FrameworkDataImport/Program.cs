namespace FrameworkDataImport
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using CsvHelper;

    public class Program
    {
        public static void Main(string[] args)
        {
            var filePath = args.Last();

            var file = File.OpenText(filePath);

            var csv = new CsvReader(file);
            csv.Configuration.RegisterClassMap<FrameworkClassMap>();
            csv.Configuration.TrimFields = true;
            csv.Configuration.IgnoreReadingExceptions = true;

            try
            {
                var records = csv.GetRecords<Framework>().ToList();
                Console.Out.WriteLine("Read {0} frameworks", records.Count);

                var topLevelCategories =
                    records.Select(r => new Category {FullName = r.Sector})
                        .Distinct(new CategoryComparer())
                        .OrderBy(c => c.FullName).ToList();

                foreach (var topLevelCategory in topLevelCategories)
                {
                    topLevelCategory.CodeName = GetCategoryCode(topLevelCategory.FullName);
                    topLevelCategory.SubCategories = records.Where(r => r.Sector == topLevelCategory.FullName)
                    .Select(d =>
                        new Category
                        {
                            FullName = d.Name,
                            CodeName = d.Number.ToString(),
                            Levels = d.Levels
                        }).OrderBy(c => c.FullName).ToList();
                }

                var categories = topLevelCategories;
            }
            catch (Exception ex)
            {
                var csvHelperExceptionData = ex.Data["CsvHelper"];
                Console.Error.WriteLine(csvHelperExceptionData);
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
                return x.FullName == y.FullName;
            }

            public int GetHashCode(Category obj)
            {
                return obj.FullName.GetHashCode();
            }
        }
    }
}
