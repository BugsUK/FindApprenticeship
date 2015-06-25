namespace FrameworkDataImport.Process
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using CsvHelper;
    using Entities;
    using SFA.Apprenticeships.Domain.Entities.ReferenceData;

    public class FrameworkDataProvider
    {
        private readonly string _filePath;

        public FrameworkDataProvider(string filePath)
        {
            _filePath = filePath;
        }

        public IEnumerable<Category> GetCategories()
        {
            var file = File.OpenText(_filePath);

            var csv = new CsvReader(file);
            csv.Configuration.RegisterClassMap<FrameworkClassMap>();
            csv.Configuration.TrimFields = true;
            csv.Configuration.IgnoreReadingExceptions = true;

            try
            {
                var records = csv.GetRecords<Framework>().ToList();
                Console.WriteLine("Read {0} frameworks", records.Count);

                var categories =
                    records.Distinct(new FrameworkSectorComparer())
                        .Select(r => new Category {FullName = r.Sector, CodeName = GetCategoryCode(r.Sector)})
                        .OrderBy(c => c.FullName)
                        .ToList();

                foreach (var category in categories)
                {
                    category.SubCategories =
                        records.Where(r => r.Sector == category.FullName)
                            .Select(d => d.ToCategory(category.CodeName))
                            .OrderBy(c => c.FullName)
                            .ToList();
                }

                return categories;
            }
            catch (Exception ex)
            {
                var csvHelperExceptionData = ex.Data["CsvHelper"];
                Console.Error.WriteLine(csvHelperExceptionData);
                throw;
            }
        }
        public class FrameworkSectorComparer : IEqualityComparer<Framework>
        {
            public bool Equals(Framework x, Framework y)
            {
                return string.Equals(x.Sector, y.Sector);
            }

            public int GetHashCode(Framework obj)
            {
                unchecked
                {
                    return (obj.Sector != null ? obj.Sector.GetHashCode() : 0);
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
    }
}