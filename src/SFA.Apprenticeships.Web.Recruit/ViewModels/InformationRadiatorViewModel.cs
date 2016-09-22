using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SFA.Apprenticeships.Web.Recruit.ViewModels
{
    public class InformationRadiatorViewModel
    {
        public bool DatabaseError { get; set; }
        public bool DatabaseSuccess { get; set; }
        public Exception DatabaseException { get; set; }
    }
}