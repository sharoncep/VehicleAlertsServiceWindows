using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using VehicleAlertsService.Enums;
using System.Configuration;
using VehicleAlertsService.Configuration;
using System.Text.RegularExpressions;

namespace VehicleAlertsService.Services
{
    /// <summary>
    /// The Sms service.
    /// </summary>
    public class SmsService
    {
        #region Member Variables

        /// <summary>
        /// The variable that holds the event log service.
        /// </summary>
        private EventLogService _eventLogService;

        #endregion

        #region Constructor

        /// <summary>
        /// Creates a new instance of SmsService.
        /// </summary>
        public SmsService()
        {
            this._eventLogService = new EventLogService();
        }

        #endregion

        #region Member Functions

        /// <summary>
        /// Send an sms text message.
        /// </summary>
        /// <param name="contactNumber">The number to send to.</param>
        /// <param name="text">The text to be sent.</param>
        /// <returns>The Success of failure status.</returns>
        public bool SendSms(string contactNumber, string text)
        {
            // The variable that holds the success or failure status.
            bool success = false;

            try
            {
                string encodedText = System.Web.HttpUtility.UrlEncode(text);
                string smsGatewayApiUrl = "http://alertbox.in/pushsms.php?username=Rainconcert&api_password=45c480hcdpqsppz87&sender=INSAFE&to={0}&message={1}&priority=8";

                smsGatewayApiUrl = string.Format(smsGatewayApiUrl, contactNumber,encodedText);

                //smsGatewayApiUrl = "http://alertbox.in/pushsms.php?username=Rainconcert&api_password=45c480hcdpqsppz87&sender=INSAFE&to=8089003542&message=Bus%20No%2019%20%28KL%2001%20AX%203302%29%20has%20an%20Emergency%20at%20Kazhakuttom&priority=8";

                // Create the web request. 
                HttpWebRequest request = WebRequest.Create(smsGatewayApiUrl) as HttpWebRequest;
 
                // Get response.
                using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
                {
                    // Get the response stream  
                    StreamReader reader = new StreamReader(response.GetResponseStream());

                    // If the response does not contain the following strings, then I assume its a success.
                    if (reader.ReadToEnd().ToLower().IndexOf("sender id invalid") == -1 || reader.ReadToEnd().ToLower().IndexOf("sorry") == -1)
                    {
                        success = true;
                    }
                }
            }
            catch (Exception ex)
            {
                // Log the error.
                this._eventLogService.LogEvent(ex.Message, EventType.Error);
            }

            // Return the success or failure status.
            return success;
        }

        #endregion
    }
}
