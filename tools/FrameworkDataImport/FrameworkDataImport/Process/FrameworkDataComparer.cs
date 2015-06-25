namespace FrameworkDataImport.Process
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Entities;
    using SFA.Apprenticeships.Application.ReferenceData;
    using SFA.Apprenticeships.Domain.Entities.ReferenceData;

    public class FrameworkDataComparer : IFrameworkDataComparer
    {
        private readonly IReferenceDataProvider _referenceDataProvider;

        public FrameworkDataComparer(IReferenceDataProvider referenceDataProvider)
        {
            _referenceDataProvider = referenceDataProvider;
        }

        public void Compare(List<Category> categories)
        {
            var existingCategories = _referenceDataProvider.GetCategories().ToList();

            Console.WriteLine("EXISTING Vs NEW");
            CompareCategories(existingCategories, categories);

            Console.WriteLine("NEW Vs EXISTING");
            CompareCategories(categories, existingCategories);
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
    }
}