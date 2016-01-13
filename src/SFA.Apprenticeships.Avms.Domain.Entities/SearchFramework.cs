namespace SFA.Apprenticeships.Avms.Domain.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class SearchFramework
    {
        [Key]
        public int SearchFrameworksId { get; set; }

        public int FrameworkId { get; set; }

        public int SavedSearchCriteriaId { get; set; }

        public virtual ApprenticeshipFramework ApprenticeshipFramework { get; set; }

        public virtual SavedSearchCriteria SavedSearchCriteria { get; set; }
    }
}
