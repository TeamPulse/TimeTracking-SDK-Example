using System;
using System.Linq;
using System.Windows;
using Telerik.TeamPulse.Sdk;

namespace TimeTrackingApp
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public TeamPulseApp TeamPulseApp { get; set; }
    }
}
