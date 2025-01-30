using Syncfusion.Maui.Buttons;
using Syncfusion.Maui.DataSource;
using Syncfusion.Maui.DataSource.Extensions;

namespace ToDoList;

public partial class HomePage : ContentPage
{
	public HomePage()
	{
		InitializeComponent();
	}

    private async void OnCheckedStateChanged(object sender, Syncfusion.Maui.Buttons.StateChangedEventArgs e)
    {
		var currentItem = (sender as SfCheckBox)!.BindingContext as Model;
		if(currentItem == null)
		{
			return;
		}
		currentItem.Status = (sender as SfCheckBox)!.IsChecked == true ? TaskStatus.Done : TaskStatus.ToDo;
		await Task.Delay(1);
        listView.DataSource!.Refresh();
    }
}