using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using VehicleAlertsService.Enums;
using VehicleAlertsService.Configuration;

namespace VehicleAlertsService.Entities
{
    /// <summary>
    /// The email alert entity. 
    /// </summary>
    public class EmailAlert
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
        /// The variable that holds the vehicle incharge name.
        /// </summary>
        private string _vehicleInchargeName;

        /// <summary>
        /// The variable that holds the vehicle incharge email id.
        /// </summary>
        private string _emailId;

        /// <summary>
        /// The variable that holds the vehicle incharge secondary email id.
        /// </summary>
        private string _secondaryEmailId;

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
        /// Gets the email from address.
        /// </summary>
        public string From
        {
            get
            {
                return AppConfiguration.IneProximaTeamEmailId;
            }
        }

        /// <summary>
        /// Gets the email to address.
        /// </summary>
        public string To
        {
            get
            {
                // The email must be sent to the incharge email id.
                return this._emailId;
            }
        }

        /// <summary>
        /// Gets the email Cc addresses.
        /// </summary>
        public List<string> Cc
        {
            get
            {
                // If the incharge has a secondary email id, send alert as CC to that id.
                if(!string.IsNullOrEmpty(this._secondaryEmailId))
                {
                    return new List<string>
                    {
                        this._secondaryEmailId
                    };
                }

                return null;
            }
        }

        /// <summary>
        /// Gets the email Bcc addresses.
        /// </summary>
        public List<string> Bcc
        {
            get
            {
                // Keep a BCC to InEProxima team, just to confirm that the alert was sent.
                return new List<string>
                    {
                        AppConfiguration.IneProximaTeamEmailId
                    };
            }
        }

        /// <summary>
        /// Gets the email subject.
        /// </summary>
        public string Subject
        {
            get
            {
                // Generate the email subject.
                return string.Format(
                        "An alert of type: {0} has been has occurred for {1} at {2} {3}",
                        Enum.GetName(typeof(AlertType), this._alertType),
                        this._vehicleNumber,
                        this._alertOccurrenceTime.ToShortDateString(),
                        this._alertOccurrenceTime.ToShortTimeString()
                    );
            }
        }

        /// <summary>
        /// Gets the email body.
        /// </summary>
        public string Body
        {
            get
            {
                // Generate the email body.
                return string.Format(
                        "Dear {0},{1}{1}An alert of type: {2} has been has occurred for {3} at {4} {5}{1}{1}Kind Regards,{1}InEProxima Team",
                        this._vehicleInchargeName,
                        Environment.NewLine,
                        Enum.GetName(typeof(AlertType), this._alertType),
                        this._vehicleNumber,
                        this._alertOccurrenceTime.ToShortDateString(),
                        this._alertOccurrenceTime.ToShortTimeString()
                    );
            }
        }

        #endregion

        #region Constructor

        /// <summary>
        /// Creates a new instance of EmailAlert.
        /// </summary>
        /// <param name="vehicleAlertId">The vehicle alert id.</param>
        /// <param name="vehicleNumber">The vehicle number.</param>
        /// <param name="alertType">The alert type.</param>
        /// <param name="alertOccurrenceTime">The alert occurrence time.</param>
        /// <param name="vehicleInchargeName">The vehicle incharge name.</param>
        /// <param name="emailId">The vehicle incharge email id.</param>
        /// <param name="secondaryEmailId">The vehicle incharge secondary email id.</param>
        public EmailAlert(int vehicleAlertId, string vehicleNumber, AlertType alertType, DateTime alertOccurrenceTime, string vehicleInchargeName, string emailId, string secondaryEmailId)
        {
            this._vehicleAlertId = vehicleAlertId;
            this._vehicleNumber = vehicleNumber;
            this._alertType = alertType;
            this._alertOccurrenceTime = alertOccurrenceTime;
            this._vehicleInchargeName = vehicleInchargeName;
            this._emailId = emailId;
            this._secondaryEmailId = secondaryEmailId;
        }

        #endregion
    }
}
