using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VehicleAlertsService.Managers;
using VehicleAlertsService.Entities;
using VehicleAlertsService.Repositories;
using System.Timers;
using VehicleAlertsService.Configuration;
using VehicleAlertsService.Services;
using VehicleAlertsService.Enums;

namespace VehicleAlertsService
{
    /// <summary>
    /// The vehicle alerts listener.
    /// </summary>
    public class VehicleAlertsListener
    {
        #region Member Variables

        /// <summary>
        /// The variable that holds the email alert manager.
        /// </summary>
        private IAlertManager<EmailAlert> _emailAlertManager;

        /// <summary>
        /// The variable that holds the sms alert manager.
        /// </summary>
        private IAlertManager<SmsAlert> _smsAlertManager;

        /// <summary>
        /// The variable that holds the vehicle alert repository.
        /// </summary>
        private IRepository<VehicleAlert> _repository;

        /// <summary>
        /// The variable that holds the event service.
        /// </summary>
        private EventLogService _eventLogService;

        /// <summary>
        /// The variable that holds the timer.
        /// </summary>
        private Timer _timer;

        #endregion

        #region Contructors

        /// <summary>
        /// Creates a new instance of VehicleAlertsListener.
        /// </summary>
        public VehicleAlertsListener()
            : this(new EmailAlertManager(), new SmsAlertManager(), new VehicleAlertsRepository())
        {
            // Initialize the timer.
            this._timer = new Timer(AppConfiguration.TimerInterval);

            // Setting autoreset to prevent timer calls from getting mixed.
            this._timer.AutoReset = false;

            // Initialize the event logger.
            this._eventLogService = new EventLogService();

            // Set the timer callback.
            this._timer.Elapsed += (x, y) => this.Read();
        }

        /// <summary>
        /// Overloaded constructor.
        /// </summary>
        /// <param name="emailAlertManager">The email alert manager.</param>
        /// <param name="smsAlertManager">The sms alert manager.</param>
        /// <param name="repository">The vehicle alert repository</param>
        public VehicleAlertsListener(IAlertManager<EmailAlert> emailAlertManager, IAlertManager<SmsAlert> smsAlertManager, IRepository<VehicleAlert> repository)
        {
            this._emailAlertManager = emailAlertManager;
            this._smsAlertManager = smsAlertManager;
            this._repository = repository;
        }

        #endregion

        #region Member Functions

        /// <summary>
        /// Starts the timer.
        /// </summary>
        public void StartListening()
        {
            // Start the timer.
            this._timer.Start();
        }

        /// <summary>
        /// Stops the timer.
        /// </summary>
        public void StopListening()
        {
            // Stop the timer.
            this._timer.Stop();
        }

        /// <summary>
        /// Reads the new/pending vehicle alerts from the the repository.
        /// </summary>
        private void Read()
        {
            try
            {
                if (this._emailAlertManager.GetAlertCount() == 0 && this._smsAlertManager.GetAlertCount() == 0)
                {
                    // Get all the vehicle alerts.
                    var alerts = this._repository.GetAll();

                    // Get email vehicle alerts, based on the alert notification type.
                    var emailAlerts = alerts
                        .Where(x => (x.AlertNotificationType == AlertNotificationType.Email || x.AlertNotificationType == AlertNotificationType.Both) && x.EmailSent == null)
                        .Select(x => new EmailAlert(x.VehicleAlertId, x.VehicleNumber, x.AlertType, x.AlertOccurrenceTime, x.VehicleInchargeName, x.EmailId, x.SecondaryEmailId))
                        .ToList();

                    // Get sms vehicle alerts, based on the alert notification type.
                    var smsAlerts = alerts
                        .Where(x => (x.AlertNotificationType == AlertNotificationType.Sms || x.AlertNotificationType == AlertNotificationType.Both) && x.SmsSent == null)
                        .Select(x => new SmsAlert(x.VehicleAlertId, x.VehicleNumber, x.AlertType, x.AlertOccurrenceTime, x.ContactNumber, x.SmsMessage))
                        .ToList();

                    // Pass the email alerts to the email alerts manager.
                    foreach (var emailAlert in emailAlerts)
                    {
                        // Add each email alert.
                        this._emailAlertManager.Add(emailAlert);
                    }

                    // Pass the sms alerts to the sms alerts manager.
                    foreach (var smsAlert in smsAlerts)
                    {
                        // Add each sms alert.
                        this._smsAlertManager.Add(smsAlert);
                    }

                    // Call Process alerts on managers.
                    this._emailAlertManager.ProcessAlerts();
                    this._smsAlertManager.ProcessAlerts();
                }
            }
            catch (Exception ex)
            {
                // Log the error.
                this._eventLogService.LogEvent(ex.Message, EventType.Error);
            }
            finally
            {
                // Start timer again.
                this._timer.Start();
            }
        }

        #endregion
    }
}
