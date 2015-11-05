using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace SFA.Apprenticeship.Api.AvService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IVacancyDetailsService" in both code and config file together.
    [ServiceContract]
    public interface IVacancyDetailsService
    {
        [OperationContract]
        void DoWork();
    }
}
