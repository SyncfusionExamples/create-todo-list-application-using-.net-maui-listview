using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDoList
{
    public enum TaskStatus
    {
        ToDo = 0,
        Done = 1,
    }
    public class Model : INotifyPropertyChanged
    {
        string _toDoContent = string.Empty;
        bool _isDone;
        Color _itemBackground = Colors.Transparent;
        TaskStatus _taskStatus = TaskStatus.ToDo;
        public Model()
        {
        }

        public string ToDoDetails
        {
            get
            {
                return _toDoContent;
            }
            set
            {
                _toDoContent = value;
                RaisePropertyChanged("ToDoDetails");
            }
        }

        public TaskStatus Status
        {
            get { return _taskStatus; }
            set
            {
                _taskStatus = value;
                RaisePropertyChanged("Status");
            }
        }

        public bool IsDone
        {
            get { return _isDone; }
            set
            {
                _isDone = value;
                RaisePropertyChanged("IsDone");
            }
        }
        public Color ItemBackground
        {
            get { return _itemBackground; }
            set
            {
                _itemBackground = value;
                RaisePropertyChanged("ItemBackground");
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        public void RaisePropertyChanged(string propertyName)
        {
            if(PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
