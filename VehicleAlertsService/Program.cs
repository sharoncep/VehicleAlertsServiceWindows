using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceProcess;

namespace VehicleAlertsService
{
    class Program : ServiceBase
    {
        static void Main(string[] args)
        {
            ServiceBase.Run(new Program());
        }

        public Program()
        {
            this.ServiceName = "Vehicle Alerts Service";
        }

        protected override void OnStart(string[] args)
        {
            base.OnStart(args);

            // Create an instance of VehicleAlertsListener.
            var listener = new VehicleAlertsListener();

            // Start listening.
            listener.StartListening();
        }

        protected override void OnStop()
        {
            base.OnStop();
        }
    }
}
