using Syncfusion.Maui.Buttons;
using Syncfusion.Maui.DataSource;
using Syncfusion.Maui.ListView;
using Syncfusion.Maui.Popup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDoList
{
    class Behavior:Behavior<ContentPage>
    {
        SfListView listView;
        SfPopup popup;
        SfButton newToDoButton;
        Border rootBorder;
        double xPosition = 0;
        double yPosition = 0;
        ViewModel viewModel;
        protected override void OnAttachedTo(ContentPage bindable)
        {             
            base.OnAttachedTo(bindable);
            viewModel = bindable!.BindingContext as ViewModel;            
            listView = (SfListView)bindable.FindByName("listView");
            popup = (SfPopup)bindable.FindByName("ToDoSheet");
            newToDoButton = (SfButton)bindable.FindByName("btnToDo");
            rootBorder = (Border)bindable.FindByName("rootBorder");

            rootBorder.SizeChanged += OnRootBorderSizeChanged;
            newToDoButton.Clicked += OnNewToDoButtonClicked;
            viewModel!.ToDoItems.CollectionChanged += OnItemsCollectionChanged;

            listView.DataSource!.SortDescriptors.Add(new SortDescriptor()
            {
                PropertyName = "Status",
                Direction = ListSortDirection.Ascending,
            });
        }

        private async void OnItemsCollectionChanged(object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add)
            {
                await Task.Delay(100);
                listView.DataSource!.Refresh();
            }
        }

        private void OnNewToDoButtonClicked(object? sender, EventArgs e)
        {
            viewModel.PopupTitle = "New To-Do";
            popup.Show();
        }

        private void OnRootBorderSizeChanged(object? sender, EventArgs e)
        {
            //var deviceHeight = Microsoft.Maui.Devices.DeviceDisplay.MainDisplayInfo.Height;
            //var deviceWidth = Microsoft.Maui.Devices.DeviceDisplay.MainDisplayInfo.Width;
            xPosition = rootBorder.Bounds.X;
            yPosition = rootBorder.Bounds.Height - 300;

            popup.StartX = (int)xPosition;
            popup.StartY = (int)yPosition;
            popup.WidthRequest = rootBorder.Bounds.Width;
        }

        protected override void OnDetachingFrom(ContentPage bindable)
        {
            rootBorder.SizeChanged -= OnRootBorderSizeChanged;
            newToDoButton.Clicked -= OnNewToDoButtonClicked;
            viewModel!.ToDoItems.CollectionChanged -= OnItemsCollectionChanged;

            popup = null;
            listView = null;
            viewModel = null;
            rootBorder = null;
            newToDoButton = null;
            
            base.OnDetachingFrom(bindable);
        }
    }
}
