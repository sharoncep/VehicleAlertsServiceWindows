using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VehicleAlertsService.Entities
{
    /// <summary>
    /// The vehicle incharge details.
    /// </summary>
    public class VehicleIncharge
    {
        /// <summary>
        /// Gets or sets the first name.
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// Gets or sets last name.
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// Gets or sets the address.
        /// </summary>
        public string Address { get; set; }
        
        /// <summary>
        /// Gets or sets the email id.
        /// </summary>
        public string EmailId { get; set; }

        /// <summary>
        /// Gets or sets the secondary email id.
        /// </summary>
        public string SecondaryEmailId { get; set; }

        /// <summary>
        /// Gets or sets the contact number.
        /// </summary>
        public string ContactNumber { get; set; }

    }
}
