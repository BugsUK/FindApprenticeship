using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SFA.Apprenticeships.Domain.Entities.Raa.Reference
{
    using ReferenceData;

    public class Occupation
    {
        public int Id { get; set; }

        public string CodeName { get; set; }

        public string ShortName { get; set; }

        public string FullName { get; set; }

        public IEnumerable<Framework> Frameworks { get; set; }

        /// <summary>
        /// TODO: Do this a better way
        /// </summary>
        /// <returns></returns>
        public Category ToCategory()
        {
            var category = new Category(Id, CategoryPrefixes.GetSectorSubjectAreaTier1Code(CodeName), FullName, CategoryType.SectorSubjectAreaTier1);
            Frameworks.ToList().ForEach(f => category.SubCategories.Add(f.ToCategory()));
            return category;
        }
    }
}
