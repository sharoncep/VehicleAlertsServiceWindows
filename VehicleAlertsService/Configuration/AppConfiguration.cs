using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using VehicleAlertsService.Utilities;

namespace VehicleAlertsService.Configuration
{
    /// <summary>
    /// The configuration class.
    /// </summary>
    public static class AppConfiguration
    {
        #region Member Variables

        /// <summary>
        /// The SMTP Server IP.
        /// </summary>
        public static string SMTPServerIP = ConfigurationManager.AppSettings["SMTPServerIP"];

        /// <summary>
        /// The SMTP Server Username.
        /// </summary>
        public static string SMTPServerUserName = ConfigurationManager.AppSettings["SMTPServerUserName"];

        /// <summary>
        /// The SMTP Server Password.
        /// </summary>
        public static string SMTPServerPassword = ConfigurationManager.AppSettings["SMTPServerPassword"];

        /// <summary>
        /// The InEProxima Team Email Id.
        /// </summary>
        public static string IneProximaTeamEmailId = ConfigurationManager.AppSettings["IneProximaTeamEmailId"];

        /// <summary>
        /// The Maximum Thread Count for sending Email alerts.
        /// </summary>
        public static int EmailMaxThreadCount = ConfigurationManager.AppSettings["EmailMaxThreadCount"].ToInt32();

        /// <summary>
        /// The Maximum Thread Count for sending SMS alerts.
        /// </summary>
        public static int SmsMaxThreadCount = ConfigurationManager.AppSettings["SmsMaxThreadCount"].ToInt32();

        /// <summary>
        /// The SMS Gateway API Url.
        /// </summary>
        public static string SmsGatewayApiUrl = ConfigurationManager.AppSettings["SmsGatewayApiUrl"];

        /// <summary>
        /// The Timer Interval.
        /// </summary>
        public static int TimerInterval = ConfigurationManager.AppSettings["TimerInterval"].ToInt32();

        /// <summary>
        /// The Connection string to the database.
        /// </summary>
        public static string SqlConnection = ConfigurationManager.ConnectionStrings["InEProxima"].ConnectionString;

        #endregion
    }
}
