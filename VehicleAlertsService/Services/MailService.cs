using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Net.Mail;
using VehicleAlertsService.Enums;
using VehicleAlertsService.Configuration;

namespace VehicleAlertsService.Services
{
    /// <summary>
    /// The mail service.
    /// </summary>
    public class MailService
    {
        #region Member Variables

        /// <summary>
        /// The variable that holds the event log service.
        /// </summary>
        private EventLogService _eventLogService;

        #endregion

        #region Constructor

        /// <summary>
        /// Creates a new instance of MailService.
        /// </summary>
        public MailService()
        {
            this._eventLogService = new EventLogService();
        }

        #endregion

        #region Member Functions

        /// <summary>
        /// Sends the mail.
        /// </summary>
        /// <param name="mailFromAddress">The mail from address.</param>
        /// <param name="mailToAddress">The mail to address.</param>
        /// <param name="subject">The subject.</param>
        /// <param name="mailBody">The mail body.</param>
        /// <returns>
        /// Success or failure status.
        /// </returns>
        public bool SendMail(string mailFromAddress, string mailToAddress, string subject, string mailBody)
        {
            // The variable that holds the success or failure status.
            bool success = false;

            // Get the SMTP details.
            string server = AppConfiguration.SMTPServerIP;
            //string userName = AppConfiguration.SMTPServerUserName;
            //string password = AppConfiguration.SMTPServerPassword;

            // Create mail message.
            MailMessage message = new MailMessage(mailFromAddress, mailToAddress);
            message.Subject = subject;
            message.Body = mailBody;

            // Create SMTP client to send mail.
            SmtpClient client = new SmtpClient(server);
            //client.Credentials = new NetworkCredential(userName, password);

            // Send mail.
            try
            {
                client.Send(message);

                success = true;
            }
            catch (Exception ex)
            {
                // Log the error.
                this._eventLogService.LogEvent(ex.Message, EventType.Error);
            }

            // Return the status.
            return success;
        }

        #endregion
    }
}
