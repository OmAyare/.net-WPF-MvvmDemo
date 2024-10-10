using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using MvvmDemo.Commands;
using MvvmDemo.Models;

namespace MvvmDemo.ViewModels
{
    public class EmployeeViewModel : INotifyPropertyChanged
    {
        #region  INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion


        EmployeeService ObjEmployeeService;
        public EmployeeViewModel()
        {
            ObjEmployeeService = new EmployeeService();
            LoadData(); 
            CurrentEmployee = new Employee();
            saveCommand = new RelayCommand(Save);
            searchCommand = new RelayCommand(Search);
            updateCommand = new RelayCommand(Update);
            deleteCommand = new RelayCommand(Delete);
        }

        #region Displayoperation
        private ObservableCollection<Employee> employeesList;
        public ObservableCollection<Employee> EmployeeList
        {
            get { return employeesList; }
            set { employeesList = value; OnPropertyChanged("EmployeeList");}
        }

        private void LoadData()
        {
            EmployeeList = new ObservableCollection<Employee>(ObjEmployeeService.GetAll());
        }
        #endregion

        private Employee currentEmployee;

        public Employee CurrentEmployee
        {
            get { return currentEmployee; }
            set { currentEmployee = value;  OnPropertyChanged("CurrentEmployee"); }
        }

        private string message;

        public string Message
        {
            get { return message; }
            set { message = value; OnPropertyChanged("Message"); }
        }

        #region SaveCommand

        private RelayCommand saveCommand;

        public RelayCommand SaveCommand
        {
            get { return saveCommand; }
        }

        public void Save()
        {
            try
            {
                var IsSaved = ObjEmployeeService.Add(CurrentEmployee);
                LoadData();
                if (IsSaved)
                    Message = "Employee Saved";
                else
                    Message = "Save Operation Failed";

            }
            catch (Exception ex)
            {
                Message = ex.Message;
            }
        }
            #endregion

        private RelayCommand searchCommand;

        public RelayCommand SearchCommand
        {
            get { return searchCommand; }
        }

        public void Search()
        {
            try
            {
                var ObjEmployee = ObjEmployeeService.Search(CurrentEmployee.Id);
                if (ObjEmployee != null)
                {
                    CurrentEmployee.Name = ObjEmployee.Name;
                    CurrentEmployee.Age = ObjEmployee.Age;
                }
                else
                {
                    Message = "Employee Not Found";

                }
            }
            catch (Exception ex)
            {
                Message = ex.Message;

            }
        }

            private RelayCommand updateCommand;

        public RelayCommand UpdateCommand
        {
            get { return updateCommand; }   
        }

        public void Update()
        {
            try
            {
                var IsUpdated = ObjEmployeeService.Update(CurrentEmployee);
                if(IsUpdated)
                {
                    Message = " Employee Updated";
                    LoadData();
                }
                else
                {
                    Message = "Operation Failed";
                }

            }
            catch (Exception ex) 
            { 
                Message = ex.Message;

            }


        }

        private RelayCommand deleteCommand;

        public RelayCommand DeleteCommand
        {
            get { return deleteCommand; }
        }

        public void Delete()
        {
            try
            {
                var IsDelete = ObjEmployeeService.Delete(CurrentEmployee.Id);
                if(IsDelete)
                {
                    Message = "Employee Deleted";
                    LoadData();
                }
                else
                {
                    Message = "Delete Operation Failed";
                }
            }
            catch (Exception ex)
            {
                Message= ex.Message;    
            }
        }

    }
}
