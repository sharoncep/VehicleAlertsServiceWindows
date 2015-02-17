using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.ServiceProcess;
using System.Configuration.Install;

namespace VehicleAlertsService
{
    [RunInstaller(true)]
    public class VehicleAlertsServiceInstaller : Installer
    {
        public VehicleAlertsServiceInstaller()
        {
            var processInstaller = new ServiceProcessInstaller();
            var serviceInstaller = new ServiceInstaller();

            //set the privileges
            processInstaller.Account = ServiceAccount.LocalSystem;

            serviceInstaller.DisplayName = "Vehicle Alerts Service";
            serviceInstaller.StartType = ServiceStartMode.Manual;

            serviceInstaller.ServiceName = "Vehicle Alerts Service";

            this.Installers.Add(processInstaller);
            this.Installers.Add(serviceInstaller);
        }
    }
}
