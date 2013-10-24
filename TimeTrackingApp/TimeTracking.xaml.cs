using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace TimeTrackingApp
{
    /// <summary>
    /// Interaction logic for TimeTracking.xaml
    /// </summary>
    public partial class TimeTracking : Page
    {
        public TimeTracking()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            this.DataContext = new TimeTrackingModel(((App)Application.Current).TeamPulseApp);
        }

        private void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var model = this.DataContext as TimeTrackingModel;
            if (model != null && HistoryTab.IsSelected)
            {
                ((TimeTrackingModel)this.DataContext).LoadTodayTimeEntries();
            }
        }
    }
}
