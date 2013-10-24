using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace TimeTrackingApp
{
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : Page
    {
        public Login()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            var model = new LoginModel();
            model.SuccessfulLogin +=new EventHandler(model_SuccessfulLogin);
            this.DataContext = model;
        }

        void model_SuccessfulLogin(object sender, EventArgs e)
        {            
            this.NavigationService.Navigate(new Uri("TimeTracking.xaml", UriKind.Relative));
        }
    }
}
