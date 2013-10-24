using System;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.ComponentModel;
using Telerik.TeamPulse.Sdk.Common;

namespace TimeTrackingApp
{
    public class LoginModel : INotifyPropertyChanged
    {
        private const string FileNameToSaveRefreshToken = "RefreshToken.txt";
        public LoginModel()
        {
            TeamPulseUrl = "http://localhost/TeamPulse";
            UserName = "booboo";
            Password = "P@ssw0rd";
            SaveRefreshToken = true;

            LogonCommand = new RelayCommand(LogonExecute, LogonCanExecute);

            if (File.Exists(FileNameToSaveRefreshToken))
            {
                RefreshToken = File.ReadAllText(FileNameToSaveRefreshToken);
                UseRefreshToken = true;
            }
        }

        public string TeamPulseUrl { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string RefreshToken { get; set; }
        public bool SaveRefreshToken { get; set; }
        public bool UseRefreshToken { get; set; }
        public AuthenticationHelper AuthenticationHelper { get; private set; }

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
                string refreshToken = UseRefreshToken ? RefreshToken : null;
                var authenticationHelper = new AuthenticationHelper(TeamPulseUrl, refreshToken, UserName, Password, "");
                authenticationHelper.Authenticate();

                if (File.Exists(FileNameToSaveRefreshToken))
                {
                    File.Delete(FileNameToSaveRefreshToken);
                }

                if (SaveRefreshToken)
                {
                    File.WriteAllText(FileNameToSaveRefreshToken, authenticationHelper.RefreshToken);
                }

                AuthenticationHelper = authenticationHelper;
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
