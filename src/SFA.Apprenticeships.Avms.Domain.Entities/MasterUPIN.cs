namespace SFA.Apprenticeships.Avms.Domain.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("MasterUPIN")]
    public partial class MasterUPIN
    {
        public int MasterUPINId { get; set; }

        public int UKPRN { get; set; }

        public int UPIN { get; set; }
    }
}
