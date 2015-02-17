using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VehicleAlertsService.Services;
using VehicleAlertsService.Enums;

namespace VehicleAlertsService.Utilities
{
    /// <summary>
    /// This class holds a list of useful helper extension methods.
    /// </summary>
    public static class Extensions
    {
        #region Extensions

        /// <summary>
        /// Helper Extension method to convert string to int32.
        /// </summary>
        /// <param name="value">The string value to convert from.</param>
        /// <returns>The int32 conversion result.</returns>
        public static int ToInt32(this string value)
        {
            // The variable that will hold the converted value.
            int convertedValue = 0;

            try
            {
                Int32.TryParse(value, out convertedValue);
            }
            catch (Exception ex)
            {
                // Log the error.
                new EventLogService().LogEvent(ex.Message, EventType.Error);
            }

            return convertedValue;
        }

        #endregion
    }
}
