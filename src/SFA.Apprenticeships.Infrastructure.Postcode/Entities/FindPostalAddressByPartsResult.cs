using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SFA.Apprenticeships.Infrastructure.Postcode.Entities
{
    public class FindPostalAddressByPartsResult
    {
        public string Id { get; set; }

        /// <summary>
        /// 1st line of the address
        /// </summary>
        public string StreetAddress { get; set; }

        /// <summary>
        /// This is the rest of the address, after line1
        /// </summary>
        public string Place { get; set; }
    }
}
