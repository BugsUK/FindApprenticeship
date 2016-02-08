using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SFA.Apprenticeships.Infrastructure.Repositories.Sql.Schemas.UserProfile.Entities
{
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("UserProfile.AgencyUser")]
    public class AgencyUser
    {
        public int Id { get; set; }

        public DateTime DateCreated { get; set; }

        public DateTime? DateUpdated { get; set; }

        public string Username { get; set; }

        public int TeamId { get; set; }

        public int RoleId { get; set; }
    }
}
