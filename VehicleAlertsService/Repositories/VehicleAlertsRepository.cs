using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VehicleAlertsService.Entities;
using System.Data.SqlClient;
using VehicleAlertsService.Configuration;
using VehicleAlertsService.Services;
using VehicleAlertsService.Enums;
using System.Data;

namespace VehicleAlertsService.Repositories
{
    /// <summary>
    /// The vehicle alerts repository.
    /// </summary>
    public class VehicleAlertsRepository : IRepository<VehicleAlert>
    {
        #region Member Variables

        /// <summary>
        /// The variable that holds the event log service.
        /// </summary>
        private EventLogService _eventLogService;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the VehicleAlertsRepository class.
        /// </summary>
        public VehicleAlertsRepository()
        {
            // this._connection = new SqlConnection(AppConfiguration.SqlConnection);
            this._eventLogService = new EventLogService();
        }

        #endregion

        #region Member Functions

        /// <summary>
        /// Get the string value for a column in the reader.
        /// </summary>
        /// <param name="reader">The data reader.</param>
        /// <param name="column">The column name.</param>
        /// <returns>The string value.</returns>
        private string GetDbString(IDataReader reader, string column)
        {
            // The variable that will hold the result.
            string value = string.Empty;

            try
            {
                if (!string.IsNullOrEmpty((string)reader[column]))
                {
                    value = reader[column].ToString();
                }
            }
            catch (Exception ex)
            {
                // Log the error.
                this._eventLogService.LogEvent(ex.Message, EventType.Error);
            }

            return value;
        }

        /// <summary>
        /// Get the int value for a column in the reader.
        /// </summary>
        /// <param name="reader">The data reader.</param>
        /// <param name="column">The column name.</param>
        /// <returns>The int value.</returns>
        private int GetDbInt(IDataReader reader, string column)
        {
            // The variable that will hold the result.
            int value = 0;

            try
            {
                value = Int32.Parse(reader[column].ToString());
            }
            catch (Exception ex)
            {
                // Log the error.
                this._eventLogService.LogEvent(ex.Message, EventType.Error);
            }

            return value;
        }

        /// <summary>
        /// Get the datetime value for a column in the reader.
        /// </summary>
        /// <param name="reader">The data reader.</param>
        /// <param name="column">The column name.</param>
        /// <returns>The datetime value.</returns>
        private DateTime GetDbDateTime(IDataReader reader, string column)
        {
            // The variable that will hold the result.
            DateTime value = DateTime.MinValue;

            try
            {
                value = (DateTime)reader[column];
            }
            catch (Exception ex)
            {
                // Log the error.
                this._eventLogService.LogEvent(ex.Message, EventType.Error);
            }

            return value;
        }

        /// <summary>
        /// Get the bool value for a column in the reader.
        /// </summary>
        /// <param name="reader">The data reader.</param>
        /// <param name="column">The column name.</param>
        /// <returns>The int value.</returns>
        private bool? GetDbBit(IDataReader reader, string column)
        {
            // The variable that will hold the result.
            bool? value = null;

            try
            {
                value = (bool?)reader[column];
            }
            catch (Exception ex)
            {
                // Log the error.
                this._eventLogService.LogEvent(ex.Message, EventType.Error);
            }

            return value;
        }

        #endregion

        #region IRepository<VehicleAlert> Members

        /// <summary>
        /// Add a vehicle alert.
        /// </summary>
        /// <param name="entity">The vehicle alert.</param>
        public void Add(VehicleAlert entity)
        {
            // This method is not supported by this repository.
            throw new InvalidOperationException("Cannot add a vehicle alert from here.");
        }

        /// <summary>
        /// Update a vehicle alert.
        /// </summary>
        /// <param name="vehicleAlertId">The vehicle alert id.</param>
        /// <param name="smsSent">Sms sent or no.</param>
        /// <param name="emailSent">Email sent or not.</param>
        public void Update(int vehicleAlertId, bool? smsSent, bool? emailSent)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(AppConfiguration.SqlConnection))
                {
                    using (SqlCommand cmd = new SqlCommand("sp_UdateVehicleAlertNotificationHistory", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        // Add parameters.
                        var p1 = cmd.Parameters.Add("@vehicleAlertId", SqlDbType.BigInt);
                        var p2 = cmd.Parameters.Add("@smsSent", SqlDbType.Bit);
                        var p3 = cmd.Parameters.Add("@emailSent", SqlDbType.Bit);

                        // Set direction and value for parameters.
                        p1.Direction = ParameterDirection.Input;
                        p1.Value = vehicleAlertId;

                        p2.Direction = ParameterDirection.Input;
                        p2.Value = smsSent;

                        p3.Direction = ParameterDirection.Input;
                        p3.Value = emailSent;

                        // Open connection.
                        con.Open();

                        // Update the stored procedure.
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                // Log the error.
                this._eventLogService.LogEvent(ex.Message, EventType.Error);
            }
        }

        /// <summary>
        /// Gets a list of all vehicle alerts.
        /// </summary>
        /// <returns>The list of vehicle alerts.</returns>
        public List<VehicleAlert> GetAll()
        {
            // The variable that holds the return value.
            var vehicleAlerts = new List<VehicleAlert>();

            try
            {
                using (var con = new SqlConnection(AppConfiguration.SqlConnection))
                {
                    using (SqlCommand cmd = new SqlCommand("sp_GetNotificationDetailsForAlert", con))
                    {
                        // Open connection.
                        con.Open();

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var alert = new VehicleAlert();
                                alert.VehicleAlertId = this.GetDbInt(reader, "VehicleAlertId");
                                alert.VehicleInchargeName = string.Format("{1}, {0}", this.GetDbString(reader, "FirstName"), this.GetDbString(reader, "LastName"));
                                alert.EmailId = this.GetDbString(reader, "EmailId");
                                alert.SecondaryEmailId = this.GetDbString(reader, "SecondaryEmailId");
                                alert.ContactNumber = this.GetDbString(reader, "ContactNumber");
                                alert.VehicleNumber = this.GetDbString(reader, "VehicleNumber");
                                alert.AlertOccurrenceTime = this.GetDbDateTime(reader, "CreatedDate");
                                alert.SmsMessage = this.GetDbString(reader, "Message");

                                // Get Alert Type.
                                switch (this.GetDbString(reader, "ParameterSubTypeCode"))
                                {
                                    case "OS":
                                        alert.AlertType = AlertType.Overspeeding;
                                        break;
                                    case "HB":
                                        alert.AlertType = AlertType.Harshbreak;
                                        break;
                                    case "OG":
                                        alert.AlertType = AlertType.GeoFence;
                                        break;
                                    case "OH":
                                        alert.AlertType = AlertType.OverHeat;
                                        break;
                                    case "SS":
                                        alert.AlertType = AlertType.SOS;
                                        break;
                                    case "FE":
                                        alert.AlertType = AlertType.FuelEmpty;
                                        break;
                                    default:
                                        // TODO: What is the default alert type?
                                        break;
                                }

                                alert.SmsMessage += alert.VehicleNumber;
                                string type = this.GetDbString(reader, "AlertType");
                                // Get Alert Notification Type.
                                switch (this.GetDbString(reader, "AlertType").ToUpper())
                                {
                                    case "EMAIL":
                                        alert.AlertNotificationType = AlertNotificationType.Both;
                                        break;
                                    case "SMS":
                                        alert.AlertNotificationType = AlertNotificationType.Sms;
                                        break;
                                    case "BOTH":
                                        alert.AlertNotificationType = AlertNotificationType.Both;
                                        break;
                                    default:
                                        // TODO: What is the default alert notification type?
                                        break;
                                }
                                if (!string.IsNullOrEmpty(type))
                                {
                                    // Add to collection.
                                    vehicleAlerts.Add(alert);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Log the error.
                this._eventLogService.LogEvent(ex.Message, EventType.Error);
            }

            return vehicleAlerts;
        }

        /// <summary>
        /// Delete a vehicle alert.
        /// </summary>
        /// <param name="entity">The vehicle alert.</param>
        public void Delete(VehicleAlert entity)
        {
            // This method is not supported by this repository.
            throw new InvalidOperationException("Cannot delete a vehicle alert from here.");
        }

        /// <summary>
        /// Gets the alert with the corresponding id.
        /// </summary>
        /// <param name="id">The alert id.</param>
        /// <returns>The alert.</returns>
        public VehicleAlert GetById(int id)
        {
            // This method is not supported by this repository.
            throw new InvalidOperationException("Cannot get alert by id.");
        }

        #endregion
    }
}
