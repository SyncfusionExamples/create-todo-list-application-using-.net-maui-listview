using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using System.Windows.Input;
using Syncfusion.Maui.ListView;
using ItemTappedEventArgs = Syncfusion.Maui.ListView.ItemTappedEventArgs;

namespace ToDoList
{
    public class ViewModel : INotifyPropertyChanged
    {
        string _popupTitle = "New To-Do";
        string _currentText = string.Empty;
        ObservableCollection<Model> _toDoItems;
        bool _isOpen;
        string _headerText = string.Empty;
        public ViewModel()
        {
            _toDoItems = new ObservableCollection<Model>();
            AddCommand = new Command(SaveToDoItem);
            CancelCommand = new Command(CancelToDoItem);
            EditCommand = new Command(EditToDoItem);
            DeleteCommand = new Command(DeleteToDoItem);
            _headerText = "Start organizing your day with ToDoList and take control of your life’s increasing demands!";
        }
        public string CurrentText
        {
            get { return _currentText; }
            set
            {
                _currentText = value;
                RaisePropertyChanged("CurrentText");
            }
        }

        public string Title
        {
            get { return _headerText; }
            set
            {
                _headerText = value;
                RaisePropertyChanged("Title");
            }
        }

        public string PopupTitle
        {
            get { return _popupTitle; }
            set
            {
                _popupTitle = value;
                RaisePropertyChanged("PopupTitle");
            }
        }
        public ObservableCollection<Model> ToDoItems
        {
            get { return _toDoItems; }
            set { _toDoItems = value; }
        }

        public bool IsOpen
        {
            get { return _isOpen; }
            set
            {
                _isOpen = value;
                RaisePropertyChanged("IsOpen");
            }
        }

        public ICommand AddCommand
        {
            get; set;
        }

        public ICommand CancelCommand
        {
            get; set;
        }

        public ICommand EditCommand
        {
            get; set;
        }

        public Command? DeleteCommand
        {
            get;
            set;
        }

        internal object EditItem;
        internal async void SaveToDoItem()
        {
            if (EditItem == null)
            {
                if(string.IsNullOrEmpty(CurrentText))
                {
                    return;
                }

                Random random = new Random();
                int r = random.Next(0, 9);
                var toDoText = this.CurrentText;
                var newItem = new Model() { 
                    ToDoDetails = toDoText, 
                    ItemBackground = ItemColors[r],
                    Status=TaskStatus.ToDo,
                };

                ToDoItems.Add(newItem);
            }
            else
            {
                (EditItem as Model)!.ToDoDetails = this.CurrentText;
            }

            await Task.Delay(100);
            IsOpen = false;
            CurrentText = string.Empty;
        }
        internal void CancelToDoItem()
        {
            CurrentText = string.Empty;
            IsOpen = false;
        }

        internal async void EditToDoItem(object item)
        {
            if ((item as ItemTappedEventArgs).ItemType == ItemType.Record)
            {
                object currentItem = (item as ItemTappedEventArgs).DataItem;
                CurrentText = (currentItem as Model)!.ToDoDetails;
                PopupTitle = "Edit To-Do";
                EditItem = currentItem;
                await Task.Delay(100);
                IsOpen = true;
            }
        }

        private void DeleteToDoItem(object item)
        {
            var currentItem = (Model)item;
            if (ToDoItems.Contains(currentItem))
            {
                ToDoItems.Remove(currentItem);
            }
        }

        internal Color[] ItemColors = new Color[]
        {
            new Color(1, 0.8f, 0.8f),  // Light Red
            new Color(0.8f, 1, 0.8f),  // Light Green
            new Color(0.8f, 0.8f, 1),  // Light Blue
            new Color(1, 1, 0.8f),     // Light Yellow
            new Color(0.8f, 1, 1),     // Light Cyan
            new Color(1, 0.8f, 1),     // Light Magenta
            new Color(0.85f, 0.85f, 0.85f), // Gainsboro (lighter gray-esq color)
            new Color(0.75f, 0.75f, 0.75f), // Silver
            new Color(1, 0.85f, 0.65f), // Light Orange (Peach)
            new Color(0.75f, 0.5f, 0.75f)  // Thistle (lighter purple)
        };

        public event PropertyChangedEventHandler? PropertyChanged;

        public void RaisePropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
