using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VehicleAlertsService.Enums;

namespace VehicleAlertsService.Entities
{
    /// <summary>
    /// The vehicle alert entity.
    /// </summary>
    public class VehicleAlert
    {
        /// <summary>
        /// Gets or sets the vehicle alert id.
        /// </summary>
        public int VehicleAlertId { get; set; }

        /// <summary>
        /// Gets or sets the alert type.
        /// </summary>
        public AlertType AlertType { get; set; }

        /// <summary>
        /// Gets or sets the alert notificaton type.
        /// </summary>
        public AlertNotificationType AlertNotificationType { get; set; }

        /// <summary>
        /// Gets or sets the vehicle number.
        /// </summary>
        public string VehicleNumber { get; set; }

        /// <summary>
        /// Gets or sets the vehicle incharge name.
        /// </summary>
        public string VehicleInchargeName { get; set; }

        /// <summary>
        /// Gets or sets the vehicle incharge email id.
        /// </summary>
        public string EmailId { get; set; }

        /// <summary>
        /// Gets or sets the vehicle incharge secondary email id.
        /// </summary>
        public string SecondaryEmailId { get; set; }

        /// <summary>
        /// Gets or sets the vehicle incharge contact number.
        /// </summary>
        public string ContactNumber { get; set; }

        /// <summary>
        /// Gets or sets the email sent flag.
        /// </summary>
        public bool? EmailSent { get; set; }

        /// <summary>
        /// Gets or sets the sms sent flag.
        /// </summary>
        public bool? SmsSent { get; set; }

        /// <summary>
        /// Gets or sets the alert occurrence time.
        /// </summary>
        public DateTime AlertOccurrenceTime { get; set; }

        /// <summary>
        /// Gets or sets the SMS message for the vehicle incharge.
        /// </summary>
        public string SmsMessage { get; set; }
    }
}
