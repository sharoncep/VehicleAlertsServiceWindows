using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VehicleAlertsService.Entities;
using VehicleAlertsService.Services;
using VehicleAlertsService.Repositories;
using System.Configuration;
using VehicleAlertsService.Enums;
using VehicleAlertsService.Configuration;

namespace VehicleAlertsService.Managers
{
    /// <summary>
    /// The email alert manager.
    /// </summary>
    public class EmailAlertManager : IAlertManager<EmailAlert>
    {
        #region Member Variables

        /// <summary>
        /// The variable that holds a list of email alerts.
        /// </summary>
        private static List<EmailAlert> _emailAlerts;

        /// <summary>
        /// The variable used for thread safe locking.
        /// </summary>
        private static readonly Object _locker = new Object();

        /// <summary>
        /// The variable that holds the maximum thread count.
        /// </summary>
        private static readonly int _maxThreadCount;

        /// <summary>
        /// The variable that holds the current thread count.
        /// </summary>
        private static int _currentThreadCount;

        /// <summary>
        /// The variable that holds the mail service.
        /// </summary>
        private MailService _mailService;

        /// <summary>
        /// The variable that holds the vehicle alerts repository.
        /// </summary>
        private VehicleAlertsRepository _respository;

        /// <summary>
        /// The variable that holds the event log service.
        /// </summary>
        private EventLogService _eventLogService;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes the static variables.
        /// </summary>
        static EmailAlertManager()
        {
            // Initalize email alerts collection.
            _emailAlerts = new List<EmailAlert>();

            // Initialize maximum thread count based on settings.
            _maxThreadCount = AppConfiguration.EmailMaxThreadCount;

            // Initialize the current thread count.
            _currentThreadCount = 0;
        }

        /// <summary>
        /// Creates a new instance of EmailAlertManager.
        /// </summary>
        public EmailAlertManager()
        {
            this._mailService = new MailService();
            this._respository = new VehicleAlertsRepository();
            this._eventLogService = new EventLogService();
        }

        #endregion

        #region IAlertManager<EmailAlert> Members

        /// <summary>
        /// Add an email alert.
        /// </summary>
        /// <param name="alert">The email alert.</param>
        public void Add(EmailAlert alert)
        {
            // Add the email alert to the collection of email alerts.
            lock (_locker)
            {
                _emailAlerts.Add(alert);
            }
        }

        /// <summary>
        /// Gets an email alert.
        /// </summary>
        /// <returns>The email alert.</returns>
        public EmailAlert GetAlert()
        {
            lock (_locker)
            {
                // Get the first email alert in the collection.
                EmailAlert alert = _emailAlerts.First();

                // Remove the email alert, as we are going to process it.
                _emailAlerts.RemoveAt(0);

                return alert;
            }
        }

        /// <summary>
        /// The total number of email alerts.
        /// </summary>
        /// <returns>The count.</returns>
        public int GetAlertCount()
        {
            lock (_locker)
            {
                return _emailAlerts.Count;
            }
        }

        /// <summary>
        /// Increments the current thread count.
        /// </summary>
        public void IncrementCurrentThreadCount()
        {
            lock (_locker)
            {
                _currentThreadCount++;
            }
        }

        /// <summary>
        /// Decrements the current thread count.
        /// </summary>
        public void DecrementCurrentThreadCount()
        {
            lock (_locker)
            {
                _currentThreadCount--;
            }
        }

        /// <summary>
        /// Process the email alerts.
        /// </summary>
        public void ProcessAlerts()
        {
            // Process emails only if there are email alerts in the collection.
            // Also halt processing if the current thread count has reached max count.
            while (this.GetAlertCount() > 0 && _currentThreadCount <= _maxThreadCount)
            {
                // Get an email alert to process.
                EmailAlert alert = this.GetAlert();

                // Create a delegate to the SendMail function in Mail service. 
                Func<EmailAlert, bool> sendEmailDelegate =
                    (EmailAlert x) => this._mailService.SendMail(x.From, x.To, x.Subject, x.Body);

                // Increment current thread count, as we are going to start threading.
                this.IncrementCurrentThreadCount();

                // Call BeginInvoke on the thread to send the alert asynchronously.
                sendEmailDelegate.BeginInvoke(
                    alert,                              // The email alert to be sent.
                    (IAsyncResult result) =>            // The Async Callback delegate.
                    {
                        try
                        {
                            // Get the delegate from the IAsyncResult result.
                            var del = (Func<EmailAlert, bool>)result.AsyncState;

                            // Get the result of sending the alert.
                            bool success = del.EndInvoke(result);

                            // Update the vehicle alert.
                            this._respository.Update(alert.VehicleAlertId, null, success);
                        }
                        catch (Exception ex)
                        {
                            // Log the error.
                            this._eventLogService.LogEvent(ex.Message, EventType.Error);
                        }
                        finally
                        {
                            // Decrement current thread count, and call process alerts recursively.
                            // This will ensure that all the alerts will get processed.
                            this.DecrementCurrentThreadCount();
                            this.ProcessAlerts();
                        }
                    },
                    sendEmailDelegate           // Pass the delegate to to be extracted in EndInvoke.
                );
            }
        }

        #endregion
    }
}
