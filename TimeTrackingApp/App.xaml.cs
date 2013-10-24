using System;
using System.Linq;
using System.Windows;
using Telerik.TeamPulse.Sdk.Common;

namespace TimeTrackingApp
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public AuthenticationHelper AuthenticationHelper { get; set; }
    }
}
