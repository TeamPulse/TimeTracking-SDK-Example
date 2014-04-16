using System;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.ComponentModel;
using Telerik.TeamPulse.Sdk;

namespace TimeTrackingApp
{
    public class LoginModel : INotifyPropertyChanged
    {
        public LoginModel()
        {
            LogonCommand = new RelayCommand(LogonExecute, LogonCanExecute);
        }

        public string TeamPulseUrl { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Domain { get; set; }

        public event EventHandler SuccessfulLogin;

        private string message;
        public string Message
        {
            get
            {
                return message;
            }
            private set
            {
                if (message != value)
                {
                    message = value;
                    OnPropertyChanged("Message");
                }
            }
        }

        private bool authenticateWithUserAndPassword;
        public bool AuthenticateWithUserAndPassword
        {
            get
            {
                return authenticateWithUserAndPassword;
            }
            set
            {
                if (authenticateWithUserAndPassword != value)
                {
                    authenticateWithUserAndPassword = value;
                    OnPropertyChanged("AuthenticateWithUserAndPassword");
                }
            }
        }

        public ICommand LogonCommand { get; private set; }

        private void LogonExecute(object arg)
        {
            try
            {
                var settings = new TeamPulseAppSettings()
                {
                    SiteUrl = this.TeamPulseUrl,

                    UseWindowsAuth = this.AuthenticateWithUserAndPassword,
                    Username = this.UserName,
                    Password = this.Password,
                    Domain = this.Domain
                };

                var app = new TeamPulseApp(settings);
                app.Login();

                ((App)Application.Current).TeamPulseApp = app;

                OnSuccessfulLogin();
            }
            catch (ApplicationException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private bool LogonCanExecute(object arg)
        {
            return true;
        }

        #region INotifyPropertyChanged Members
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propName)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propName));
            }
        }

        #endregion

        protected void OnSuccessfulLogin()
        {
            if (SuccessfulLogin != null)
            {
                SuccessfulLogin(this, EventArgs.Empty);
            }
        }
    }
}
