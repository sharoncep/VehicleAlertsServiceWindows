using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VehicleAlertsService.Managers
{
    /// <summary>
    /// The alert manager interface.
    /// </summary>
    /// <typeparam name="T">The alert.</typeparam>
    public interface IAlertManager<T>
    {
        /// <summary>
        /// Add an alert.
        /// </summary>
        /// <param name="alert">The alert.</param>
        void Add(T alert);

        /// <summary>
        /// Gets an alert.
        /// </summary>
        /// <returns>The alert.</returns>
        T GetAlert();

        /// <summary>
        /// The total number of alerts.
        /// </summary>
        /// <returns>The count.</returns>
        int GetAlertCount();

        /// <summary>
        /// Increments the current thread count.
        /// </summary>
        void IncrementCurrentThreadCount();

        /// <summary>
        /// Decrements the current thread count.
        /// </summary>
        void DecrementCurrentThreadCount();

        /// <summary>
        /// Process the alerts.
        /// </summary>
        void ProcessAlerts();
    }
}
