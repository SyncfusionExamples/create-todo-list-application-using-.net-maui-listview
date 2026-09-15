# Create a ToDO list application using .NET MAUI ListView (SfListView)

This demo explains about how to create a ToDoList application using .NET MAUI ListView (SfListView).

## Sample

```xaml
<ContentPage.Behaviors>
    <local:Behavior />
</ContentPage.Behaviors>

<ContentPage.Resources>
    <Style x:Key="CommonButtonStyle" TargetType="buttons:SfButton">
        <Setter Property="VisualStateManager.VisualStateGroups">
            <VisualStateGroupList>
                <VisualStateGroup x:Name="CommonStates">
                    <VisualState x:Name="Normal">
                        <VisualState.Setters>
                            <Setter Property="Background" Value="Transparent" />

                        </VisualState.Setters>
                    </VisualState>
                </VisualStateGroup>
            </VisualStateGroupList>
        </Setter>
    </Style>
    <local:ValueToColorConverter x:Key="boolToColorConverter" />
</ContentPage.Resources>
<ContentPage.Content>
    <Border
        x:Name="rootBorder"
        Padding="0"
        Background="#FFFBFE"
        HorizontalOptions="{OnPlatform WinUI=Center,
                                        MacCatalyst=Center,
                                        Default=Fill}"
        MaximumWidthRequest="{OnPlatform WinUI=380,
                                            MacCatalyst=400}"
        Stroke="#CAC4D0"
        StrokeThickness="{OnPlatform Default=0,
                                        MacCatalyst=1,
                                        WinUI=1}"
        VerticalOptions="{OnPlatform MacCatalyst=Center}">
        <Border.Margin>
            <OnPlatform x:TypeArguments="thickness:Thickness">
                <On Platform="MacCatalyst" Value="20" />
                <On Platform="WinUI" Value="20" />
            </OnPlatform>
        </Border.Margin>


        <Grid Margin="0" RowSpacing="0">
            <Grid.RowDefinitions>
                <RowDefinition Height="Auto" />
                <RowDefinition Height="*" />
            </Grid.RowDefinitions>

            <Grid
                x:Name="headerGrid"
                Grid.Row="0"
                Background="{StaticResource Primary}"
                HeightRequest="60">
                <Grid.ColumnDefinitions>
                    <ColumnDefinition Width="*" />
                </Grid.ColumnDefinitions>

                <Label
                    Margin="10,0,0,0"
                    FontAttributes="Bold"
                    FontFamily="Roboto-Medium"
                    FontSize="{OnPlatform Android=20,
                                            Default=20}"
                    HorizontalOptions="Start"
                    LineBreakMode="NoWrap"
                    Text="To-dos"
                    TextColor="{StaticResource White}"
                    VerticalOptions="Center" />
            </Grid>

            <listView:SfListView
                x:Name="listView"
                Grid.Row="1"
                AllowSwiping="True"
                AutoFitMode="DynamicHeight"
                DragStartMode="OnHold"
                ItemsSource="{Binding ToDoItems}"
                SelectionMode="None"
                SwipeOffset="100"
                SwipeThreshold="50"
                TapCommand="{Binding EditCommand}">
                <listView:SfListView.ItemTemplate>
                    <DataTemplate x:DataType="local:Model">
                        ...
                    </DataTemplate>
                </listView:SfListView.ItemTemplate>

                <listView:SfListView.HeaderTemplate>
                    <DataTemplate>
                        ...
                    </DataTemplate>
                </listView:SfListView.HeaderTemplate>

                <listView:SfListView.EndSwipeTemplate>
                    <DataTemplate x:DataType="local:Model">
                        ...
                    </DataTemplate>
                </listView:SfListView.EndSwipeTemplate>
            </listView:SfListView>

            <buttons:SfButton
                x:Name="btnToDo"
                Grid.Row="1"
                Margin="0,0,40,40"
                CornerRadius="30"
                FontSize="35"
                HeightRequest="60"
                HorizontalOptions="End"
                HorizontalTextAlignment="Center"
                Text="+"
                VerticalOptions="End"
                VerticalTextAlignment="{OnPlatform Default=Center,
                                                    WinUI=Justify}"
                WidthRequest="60" />
            <popup:SfPopup
                x:Name="ToDoSheet"
                Grid.Row="1"
                AnimationMode="SlideOnBottom"
                HeightRequest="300"
                IsOpen="{Binding IsOpen, Mode=TwoWay}"
                ShowFooter="False"
                ShowHeader="True">
                <popup:SfPopup.PopupStyle>
                    <popup:PopupStyle CornerRadius="30,30,0,0" HasShadow="True" />
                </popup:SfPopup.PopupStyle>
                <popup:SfPopup.HeaderTemplate>
                    <DataTemplate>
                        ...
                    </DataTemplate>
                </popup:SfPopup.HeaderTemplate>

                <popup:SfPopup.ContentTemplate>
                    <DataTemplate>
                        ...
                    </DataTemplate>
                </popup:SfPopup.ContentTemplate>
            </popup:SfPopup>
        </Grid>
    </Border>
</ContentPage.Content>
```

```c#
internal class Behavior:Behavior<ContentPage>
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
}
```

## Requirements to run the demo

* [Visual Studio 2017](https://visualstudio.microsoft.com/downloads/) or [Visual Studio for Mac](https://visualstudio.microsoft.com/vs/mac/)
* Xamarin add-ons for Visual Studio (available via the Visual Studio installer).

## Troubleshooting

### Path too long exception

If you are facing path too long exception when building this example project, close Visual Studio and rename the repository to short and build the project.

