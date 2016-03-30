using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SFA.Apprenticeships.Domain.Entities.Raa.Reference
{
    public class Occupation
    {
        public int Id { get; set; }

        public string CodeName { get; set; }

        public string ShortName { get; set; }

        public string FullName { get; set; }

        public IEnumerable<Framework> Frameworks { get; set; }
    }
}
