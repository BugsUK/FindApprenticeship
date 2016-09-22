using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SFA.Apprenticeships.Web.Recruit.ViewModels
{
    using Controllers;

    public class InformationRadiatorViewModel
    {
        public InformationRadiatorStatus DatabaseStatus { get; set; }
    }

    public enum InformationRadiatorStatus
    {
        Unknown,
        Exception,
        Warning,
        Success
    }
}