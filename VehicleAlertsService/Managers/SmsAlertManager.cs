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
    /// The sms alert manager.
    /// </summary>
    public class SmsAlertManager : IAlertManager<SmsAlert>
    {
        #region Member Variables

        /// <summary>
        /// The variable that holds a list of sms alerts.
        /// </summary>
        private static List<SmsAlert> _smsAlerts;

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
        /// The variable that holds the sms service.
        /// </summary>
        private SmsService _smsService;

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
        static SmsAlertManager()
        {
            // Initalize sms alerts collection.
            _smsAlerts = new List<SmsAlert>();

            // Initialize maximum thread count based on settings.
            _maxThreadCount = AppConfiguration.SmsMaxThreadCount;

            // Initialize the current thread count.
            _currentThreadCount = 0;
        }

        /// <summary>
        /// Creates a new instance of SmsAlertManager.
        /// </summary>
        public SmsAlertManager()
        {
            this._smsService = new SmsService();
            this._respository = new VehicleAlertsRepository();
            this._eventLogService = new EventLogService();
        }

        #endregion

        #region IAlertManager<SmsAlert> Members

        /// <summary>
        /// Add an sms alert.
        /// </summary>
        /// <param name="alert">The sms alert.</param>
        public void Add(SmsAlert alert)
        {            
            lock (_locker)
            {
                // Add the sms alert to the collection of sms alerts.
                _smsAlerts.Add(alert);
            }
        }

        /// <summary>
        /// Gets an sms alert.
        /// </summary>
        /// <returns>The sms alert.</returns>
        public SmsAlert GetAlert()
        {
            lock (_locker)
            {
                // Get the first sms alert in the collection.
                SmsAlert alert = _smsAlerts.First();

                // Remove the sms alert, as we are going to process it.
                _smsAlerts.RemoveAt(0);

                return alert;
            }
        }

        /// <summary>
        /// The total number of sms alerts.
        /// </summary>
        /// <returns>The count.</returns>
        public int GetAlertCount()
        {
            lock (_locker)
            {
                return _smsAlerts.Count;
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
        /// Process the sms alerts.
        /// </summary>
        public void ProcessAlerts()
        {
            // Process emails only if there are sms alerts in the collection.
            // Also halt processing if the current thread count has reached max count.
            while (this.GetAlertCount() > 0 && _currentThreadCount <= _maxThreadCount)
            {
                // Get an sms alert to process.
                SmsAlert alert = this.GetAlert();

                // Create a delegate to the SendSms function in sms service. 
                Func<SmsAlert, bool> sendSmsDelegate =
                    (SmsAlert x) => this._smsService.SendSms(x.ContactNumber, x.Text);

                // Increment current thead count, as we are going to start threading.
                this.IncrementCurrentThreadCount();

                // Call BeginInvoke on the thread to send the alert asynchronously.
                sendSmsDelegate.BeginInvoke(
                    alert,                          // The sms alert to be sent.
                    (IAsyncResult result) =>        // The Async Callback delegate.
                    {
                        try
                        {
                            // Get the delegate from the IAsyncResult result.
                            var del = (Func<SmsAlert, bool>)result.AsyncState;

                            // Get the result of sending the alert.
                            bool success = del.EndInvoke(result);

                            // Update the vehicle alert.
                            this._respository.Update(alert.VehicleAlertId, success, null);
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
                    sendSmsDelegate       // Pass the delegate to to be extracted in EndInvoke.
                );
            }
        }

        #endregion
    }
}
