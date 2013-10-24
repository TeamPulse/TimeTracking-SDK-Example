using System;
using System.Linq;
using System.Windows.Navigation;

namespace TimeTrackingApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : NavigationWindow
    {
        public MainWindow()
        {
            InitializeComponent();

            this.Navigate(new Uri("Login.xaml", UriKind.Relative));
        }
    }
}
