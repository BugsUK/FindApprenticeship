﻿namespace SFA.Apprenticeship.Api.AvService.DataContracts.Version51
{
    using System.Runtime.Serialization;
    using Namespaces.Version51;

    [DataContract(Namespace = Namespace.Uri)]
    public class ApprenticeshipTypeData
    {
        [DataMember]
        public string CodeName { get; set; }
        [DataMember]
        public string ShortName { get; set; }
        [DataMember]
        public string FullName { get; set; }
    }
}
