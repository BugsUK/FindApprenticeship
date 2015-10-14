namespace SFA.Apprenticeship.Api.AvmsCompatability.DataContracts.Version50
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using Common;

    [DataContract(Namespace = CommonNamespaces.ExternalInterfaces, Name = "CreateMessageRequest")]
    public class VacancyTransferLogData
    {
        [DataMember]
        public int VacancyId { get; set; }

        [DataMember]
        public int VacancyReference { get; set; }

        [DataMember]
        public string VacancyTitle { get; set; }

        [DataMember]
        public VacancyStatus VacancyStatus { get; set; }

        public int EmployerId { get; set; }
        public string EmployerName { get; set; }

        [DataMember]
        public int NewTrainingProviderId { get; set; }

        [DataMember]
        public string NewTrainingProviderName { get; set; }

        [DataMember]
        public int NewTrainingProviderUKPRN { get; set; }

        [DataMember]
        public bool NewTrainingProviderIsNASProvider { get; set; }

        [DataMember]
        public int NewNASRegionId { get; set; }

        [DataMember]
        public int OldNASRegionId { get; set; }

        [DataMember]
        public int OldTrainingProviderId { get; set; }

        [DataMember]
        public string OldTrainingProviderName { get; set; }

        [DataMember]
        public int OldTrainingProviderUKPRN { get; set; }

        [DataMember]
        public bool OldTrainingProviderIsNASProvider { get; set; }

        [DataMember]
        public int NewDelivererId { get; set; }

        [DataMember]
        public string NewDelivererName { get; set; }

        [DataMember]
        public int NewDelivererUKPRN { get; set; }

        [DataMember]
        public int OldDelivererId { get; set; }

        [DataMember]
        public string OldDelivererName { get; set; }

        [DataMember]
        public int OldDelivererUKPRN { get; set; }

        [DataMember]
        public int NewVacancyManagerId { get; set; }

        [DataMember]
        public string NewVacancyManagerName { get; set; }

        [DataMember]
        public int NewVacancyManagerUKPRN { get; set; }

        [DataMember]
        public int OldVacancyManagerId { get; set; }

        [DataMember]
        public string OldVacancyManagerName { get; set; }

        [DataMember]
        public int OldVacancyManagerUKPRN { get; set; }

    }

    [DataContract(Namespace = "http://www.avms.lsc.gov.uk/services/CreateMessage", Name = "CreateMessageRequest")]
    public class VacancyTransferLogListData
    {
        [DataMember]
        public List<VacancyTransferLogData> VacancyTransferLogList { get; set; }


    }
}
