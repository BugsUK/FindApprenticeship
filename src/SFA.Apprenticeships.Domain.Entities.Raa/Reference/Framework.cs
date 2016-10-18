namespace SFA.Apprenticeships.Domain.Entities.Raa.Reference
{
    using ReferenceData;

    public class Framework
    {
        public Framework()
        {
            Status = FrameworkStatusType.Active;
        }

        public int Id { get; set; }

        public string CodeName { get; set; }

        public string ShortName { get; set; }

        public string FullName { get; set; }

        public string ParentCategoryCodeName { get; set; }

        public FrameworkStatusType Status { get; set; }

        /// <summary>
        /// TODO: Do this a better way
        /// </summary>
        /// <returns></returns>
        public Category ToCategory()
        {
            return new Category(Id, CategoryPrefixes.GetFrameworkCode(CodeName), FullName, CategoryPrefixes.GetSectorSubjectAreaTier1Code(ParentCategoryCodeName), CategoryType.Framework, (CategoryStatus)(int)Status);
        }
    }
}
