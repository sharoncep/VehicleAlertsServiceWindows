using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using VehicleAlertsService.Enums;

namespace VehicleAlertsService.Services
{
    /// <summary>
    /// The event log service.
    /// </summary>
    public class EventLogService
    {
        #region Member Variables

        /// <summary>
        /// The variable that holds the event logger.
        /// </summary>
        private EventLog _eventLogger;

        #endregion

        #region Constructor

        /// <summary>
        /// Creates a new instance of EventLogService.
        /// </summary>
        public EventLogService()
        {
            this._eventLogger = new EventLog();
            this._eventLogger.Source = "Vehicle Alert Service";
        }

        #endregion

        #region Member Functions

        /// <summary>
        /// Logs an event.
        /// </summary>
        /// <param name="message">The log event message.</param>
        /// <param name="eventType">The log event type.</param>
        public void LogEvent(string message, EventType eventType)
        {
            // Get the type and EventLogEntryType based on the provided eventType.
            string type;
            EventLogEntryType eventLogEntryType;

            switch (eventType)
            {
                case EventType.Success:
                    type = "Success";
                    eventLogEntryType = EventLogEntryType.SuccessAudit;
                    break;
                case EventType.Failure:
                    type = "Failure";
                    eventLogEntryType = EventLogEntryType.FailureAudit;
                    break;
                case EventType.Info:
                    type = "Info";
                    eventLogEntryType = EventLogEntryType.Information;
                    break;
                case EventType.Error:
                    type = "Error";
                    eventLogEntryType = EventLogEntryType.Error;
                    break;
                default:
                    type = "Error";
                    eventLogEntryType = EventLogEntryType.Error;
                    break;
            }

            // Log the message.
            this._eventLogger.WriteEntry(string.Format("Vehicle Alerts Service {0} Event: {1}{2}", type, Environment.NewLine, message), eventLogEntryType);
        }

        #endregion
    }
}
