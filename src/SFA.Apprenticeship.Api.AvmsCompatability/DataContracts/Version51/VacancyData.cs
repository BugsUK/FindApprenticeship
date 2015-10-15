namespace SFA.Apprenticeship.Api.AvmsCompatability.DataContracts.Version51
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using Common;

    [DataContract(Namespace = CommonNamespaces.ExternalInterfacesRel51)]
    public class VacancyData
    {
        [DataMember(IsRequired = true, Order = 1)]
        public Decimal Wage { get; set; }

        [DataMember(IsRequired = true, Order = 2)]
        public WageType WageType { get; set; }

        [DataMember(IsRequired = true, Order = 3)]
        public string WorkingWeek { get; set; }

        [DataMember(IsRequired = false, Order = 4)]
        public string SkillsRequired { get; set; }

        [DataMember(IsRequired = false, Order = 5)]
        public string QualificationRequired { get; set; }

        [DataMember(IsRequired = false, Order = 6)]
        public string PersonalQualities { get; set; }

        [DataMember(IsRequired = false, Order = 7)]
        public string FutureProspects { get; set; }

        [DataMember(IsRequired = false, Order = 8)]
        public string OtherImportantInformation { get; set; }

        [DataMember(IsRequired = true, Order = 9)]
        public VacancyLocationType LocationType { get; set; }

        [DataMember(IsRequired = true, Order = 10)]
        public List<SiteVacancyData> LocationDetails { get; set; }

        [DataMember(IsRequired = false, Order = 11)]
        public string RealityCheck { get; set; }

        [DataMember(IsRequired = false, Order = 12)]
        public string SupplementaryQuestion1 { get; set; }

        [DataMember(IsRequired = false, Order = 13)]
        public string SupplementaryQuestion2 { get; set; }
    }
}
