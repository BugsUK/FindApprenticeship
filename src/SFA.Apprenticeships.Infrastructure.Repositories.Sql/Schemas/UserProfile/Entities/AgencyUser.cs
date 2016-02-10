namespace SFA.Apprenticeships.Infrastructure.Repositories.Sql.Schemas.UserProfile.Entities
{
    using System;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("UserProfile.AgencyUser")]
    public class AgencyUser
    {
        private AgencyUserRole _role;
        private AgencyUserTeam _team;

        public int AgencyUserId { get; set; }

        public DateTime DateCreated { get; set; }

        public DateTime? DateUpdated { get; set; }

        public string Username { get; set; }

        public int? TeamId { get; set; }
        
        public int? RoleId { get; set; }

        [NotMapped]
        public AgencyUserRole Role
        {
            get
            {
                return _role;
            }

            set
            {
                _role = value;
                RoleId = value?.AgencyUserRoleId;
            }
        }

        [NotMapped]
        public AgencyUserTeam Team
        {
            get
            {
                return _team;
            }

            set
            {
                _team = value;
                TeamId = value?.AgencyUserTeamId;
            }
        }
    }
}
