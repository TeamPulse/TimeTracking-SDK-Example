using System;
using System.Linq;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.ComponentModel;
using System.Windows;
using Telerik.TeamPulse.Sdk;
using Telerik.TeamPulse.Sdk.Models;
using System.Collections.Generic;

namespace TimeTrackingApp
{
    public class TimeTrackingModel : INotifyPropertyChanged
    {
        System.Windows.Threading.DispatcherTimer dispatcherTimer;
        private ObservableCollection<Project> projects;
        private ObservableCollection<WorkItem> myTasks;
        private TimeEntryType[] timeEntryTypes;
        private TimeEntry[] todaysTimeEntry;

        private Project selectedProject;
        private WorkItem selectedTask;
        private TimeEntryType selectedTimeEntryType;
        
        private TeamPulseApp App;
        private User currentUser;        

        private DateTime startTime;

        public TimeTrackingModel(TeamPulseApp app)
        {
            this.App = app;
            this.currentUser = App.Users.GetCurrent();

            StartTrackTimeCommand = new RelayCommand(StartTrackTimeExecute, StartTrackTimeCanExecute);
            StopTrackTimeCommand = new RelayCommand(StopTrackTimeExecute, StopTrackTimeCanExecute);

            LoadData();

            dispatcherTimer = new System.Windows.Threading.DispatcherTimer();
            dispatcherTimer.Tick += new EventHandler(dispatcherTimer_Tick);
            dispatcherTimer.Interval = new TimeSpan(0, 0, 1);
        }

        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            Time = (DateTime.Now - startTime).ToString(@"hh\:mm\:ss");
        }

        public ObservableCollection<Project> Projects
        {
            get
            {
                return projects;
            }
            private set
            {
                if (projects != value)
                {
                    projects = value;
                    OnPropertyChanged("Projects");
                }
            }
        }

        public ObservableCollection<WorkItem> MyTasks
        {
            get
            {
                return myTasks;
            }
            set
            {
                if (myTasks != value)
                {
                    myTasks = value;
                    OnPropertyChanged("MyTasks");
                }
            }
        }

        public TimeEntryType[] TimeEntryTypes
        {
            get
            {
                return timeEntryTypes;
            }
            set
            {
                if (timeEntryTypes != value)
                {
                    timeEntryTypes = value;
                    OnPropertyChanged("TimeEntryTypes");
                }
            }
        }

        public TimeEntry[] TodaysTimeEntries
        {
            get
            {
                return todaysTimeEntry;
            }
            set
            {
                if (todaysTimeEntry != value)
                {
                    todaysTimeEntry = value;
                    OnPropertyChanged("TodaysTimeEntries");
                }
            }
        }

        public Project SelectedProject
        {
            get { return selectedProject; }
            set
            {
                if (selectedProject != value)
                {
                    selectedProject = value;
                    OnPropertyChanged("SelectedProject");

                    LoadMyTasksAndTimeEntryTypes();
                }
            }
        }        

        public WorkItem SelectedTask
        {
            get { return selectedTask; }
            set
            {
                if (selectedTask != value)
                {
                    selectedTask = value;
                    OnPropertyChanged("SelectedTask");
                }
            }
        }

        public TimeEntryType SelectedTimeEntryType
        {
            get { return selectedTimeEntryType; }
            set
            {
                if (selectedTimeEntryType != value)
                {
                    selectedTimeEntryType = value;
                    OnPropertyChanged("SelectedTimeEntryType");
                }
            }
        }

        private bool isWorking;
        public bool IsWorking
        {
            get
            {
                return isWorking;
            }
            private set
            {
                if (isWorking != value)
                {
                    isWorking = value;
                    OnPropertyChanged("IsWorking");
                }
            }
        }

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

        private string time;
        public string Time
        {
            get
            {
                return time;
            }
            set
            {
                if (time != value)
                {
                    time = value;
                    OnPropertyChanged("Time");
                }
            }
        }

        public ICommand StartTrackTimeCommand { get; private set; }
        public ICommand StopTrackTimeCommand { get; private set; }

        private void LoadData()
        {
            Projects = new ObservableCollection<Project>(App.Projects.GetAll().results);

            SelectedProject = Projects.FirstOrDefault();
        }


        public void LoadTodayTimeEntries()
        {
            string format = "yyyy-MM-dd";
            string odataOptions = string.Format("$filter={0} eq datetime'{1}'", "date", DateTime.Now.Date.ToString(format));

            TodaysTimeEntries = App.TimeEntries.Get(odataOptions).results;
        }

        private void StartTrackTimeExecute(object arg)
        {
            startTime = DateTime.Now;
            Time = new TimeSpan().ToString(@"hh\:mm\:ss");
            IsWorking = true;
            Message = null;
            dispatcherTimer.Start();
        }

        private bool StartTrackTimeCanExecute(object arg)
        {
            return true;
        }

        private void StopTrackTimeExecute(object arg)
        {
            try
            {
                dispatcherTimer.Stop();
                TimeSpan ts = DateTime.Now - startTime;
                var hours = (ts.Minutes == 0 ? 1 : ts.Minutes) / (float)60;
                
                var currentTimeEntry = App.TimeEntries.GetByTask(SelectedTask.id).results.
                    FirstOrDefault(x=> x.type == SelectedTimeEntryType.name && x.date == DateTime.Now.Date);

                if (currentTimeEntry != null)
                {
                    UpdateTimeEntry(hours, currentTimeEntry);
                }
                else
                {
                    CreateTimeEntry(hours);
                }

                Message = string.Format("You've successfuly logged {0} hours to task id {1}.", hours, SelectedTask.id);
                MessageBox.Show(Message);
            }
            catch (ApplicationException ex)
            {
                Message = ex.Message;
            }
            finally
            {
                IsWorking = false;
            }
        }

        private void CreateTimeEntry(float hours)
        {
            TimeEntry te = new TimeEntry
            {
                hours = hours,
                date = DateTime.Today,
                taskId = SelectedTask.id,
                type = SelectedTimeEntryType.name,
                userId = currentUser.id
            };

            App.TimeEntries.Create(te);
        }

        private void UpdateTimeEntry(float hours, TimeEntry currentTimeEntry)
        {
            Dictionary<string, object> updatedTimeEntry = new Dictionary<string, object>();
            updatedTimeEntry.Add("hours", hours + currentTimeEntry.hours);
            updatedTimeEntry.Add("date", DateTime.Today);
            updatedTimeEntry.Add("taskId", SelectedTask.id);
            updatedTimeEntry.Add("type", SelectedTimeEntryType.name);
            updatedTimeEntry.Add("userId", currentUser.id);

            App.TimeEntries.Update(currentTimeEntry.id, updatedTimeEntry);
        }

        private bool StopTrackTimeCanExecute(object arg)
        {
            return true;
        }

        private void LoadMyTasksAndTimeEntryTypes()
        {
            LoadMyTasks();
            TimeEntryTypes = App.TimeEntryTypes.GetByProject(SelectedProject.id).results;
            SelectedTimeEntryType = TimeEntryTypes.FirstOrDefault();
        }

        private void LoadMyTasks()
        {
            string odataOptions = string.Format("?$filter={0} eq '{1}' and {2} eq {3} and {4} eq {5}",
                "type", "Task", "projectId", SelectedProject.id, "AssignedToID", currentUser.id);
            var tasks = App.WorkItems.Get(odataOptions).results;

            MyTasks = new ObservableCollection<WorkItem>(tasks);
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
    }
}
