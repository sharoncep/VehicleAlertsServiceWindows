using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VehicleAlertsService.Enums;

namespace VehicleAlertsService.Entities
{
    /// <summary>
    /// The sms alert entity.
    /// </summary>
    public class SmsAlert
    {
        #region Member Variables

        /// <summary>
        /// The vehicle alert id.
        /// </summary>
        private int _vehicleAlertId;

        /// <summary>
        /// The variable that holds the vehicle number.
        /// </summary>
        private string _vehicleNumber;

        /// <summary>
        /// The variable that holds the vehicle alert type.
        /// </summary>
        private AlertType _alertType;

        /// <summary>
        /// The variable that holds the alert occurrence time.
        /// </summary>
        private DateTime _alertOccurrenceTime;

        /// <summary>
        /// The variable that holds the vehicle incharge contact number.
        /// </summary>
        private string _contactNumber;

        /// <summary>
        /// The variable that holds the sms message to be sent.
        /// </summary>
        private string _smsMessage;

        #endregion

        #region Member Properties

        /// <summary>
        /// Gets the vehicle alert id.
        /// </summary>
        public int VehicleAlertId
        {
            get
            {
                return this._vehicleAlertId;
            }
        }

        /// <summary>
        /// Gets the sms contact number.
        /// </summary>
        public string ContactNumber
        {
            get
            {
                return this._contactNumber;
            }
        }

        public string Text
        {
            get
            {
                // Generate the sms text.
                //return string.Format(
                //        "An alert of type: {0} has been has occurred for {1} at {2} {3}",
                //        Enum.GetName(typeof(AlertType),this._alertType),
                //        this._vehicleNumber,
                //        this._alertOccurrenceTime.ToShortDateString(),
                //        this._alertOccurrenceTime.ToShortTimeString()
                //    );

                return this._smsMessage;
            }
        }

        #endregion

        #region Constructor

        /// <summary>
        /// Creates a new instance of Sms Alert.
        /// </summary>
        /// <param name="vehicleAlertId">The vehicle alert id.</param>
        /// <param name="vehicleNumber">The vehicle number.</param>
        /// <param name="alertType">The alert type.</param>
        /// <param name="alertOccurrenceTime">The alert occurrence time.</param>
        /// <param name="contactNumber">The vehicle incharge contact number.</param>
        /// <param name="smsMessage">The sms message to be sent.</param>
        public SmsAlert(int vehicleAlertId, string vehicleNumber, AlertType alertType, DateTime alertOccurrenceTime, string contactNumber, string smsMessage)
        {
            this._vehicleAlertId = vehicleAlertId;
            this._vehicleNumber = vehicleNumber;
            this._alertType = alertType;
            this._alertOccurrenceTime = alertOccurrenceTime;
            this._contactNumber = contactNumber;
            this._smsMessage = smsMessage;
        }

        #endregion
    }
}
