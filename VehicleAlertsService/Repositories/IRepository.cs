using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VehicleAlertsService.Repositories
{
    /// <summary>
    /// The repository interface.
    /// </summary>
    /// <typeparam name="T">The entity.</typeparam>
    public interface IRepository<T>
    {
        /// <summary>
        /// Add an entity.
        /// </summary>
        /// <param name="entity">The entity</param>
        void Add(T entity);

        /// <summary>
        /// Update alert.
        /// </summary>
        /// <param name="vehicleAlertId">The vehicle alert id.</param>
        /// <param name="smsSent">Sms sent or no.</param>
        /// <param name="emailSent">Email sent or not.</param>
        void Update(int vehicleAlertId, bool? smsSent, bool? emailSent);

        /// <summary>
        /// Gets the entity with the corresponding id.
        /// </summary>
        /// <param name="id">The entity id.</param>
        /// <returns>The entity.</returns>
        T GetById(int id);

        /// <summary>
        /// Gets a list of all entities.
        /// </summary>
        /// <returns>The list of entities.</returns>
        List<T> GetAll();

        /// <summary>
        /// Delete an entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        void Delete(T entity);
    }
}
