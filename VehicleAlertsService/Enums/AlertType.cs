using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VehicleAlertsService.Enums
{
    /// <summary>
    /// The vehicle alert type enumeration.
    /// </summary>
    public enum AlertType
    {
        Overspeeding,
        Harshbreak,
        GeoFence,
        OverHeat,
        SOS,
        FuelEmpty
    }
}
