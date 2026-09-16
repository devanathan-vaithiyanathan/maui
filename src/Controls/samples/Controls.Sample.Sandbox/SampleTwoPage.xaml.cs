using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;


namespace Maui.Controls.Sample;

public partial class SampleTwoPage : ContentPage
{
    public SampleTwoPage()
    {
        InitializeComponent();
        BindingContext = new MainPageViewModel();
    }

    protected override void OnNavigatedFrom(NavigatedFromEventArgs args)
    {
        base.OnNavigatedFrom(args);

        ((MainPageViewModel)BindingContext).NavigatedFrom();
    }
}



public class MainPageViewModel : INotifyPropertyChanged
{
    private int _groupIdCounter;
    private int _itemIdCounter;

    public ObservableCollection<ItemGroup> GroupedItems { get; set; }

    public MainPageViewModel()
    {
        GroupedItems = new ObservableCollection<ItemGroup>();
        AddGroupsCommand = new Command(AddGroups);
        RemoveGroupCommand = new Command(RemoveGroup, () => GroupedItems.Count > 0);
        NavigateCommand = new Command(Navigate);

        GroupedItems.CollectionChanged += (_, _) => RemoveGroupCommand.ChangeCanExecute();
    }

    public Command AddGroupsCommand { get; }

    public Command RemoveGroupCommand { get; }

    public Command NavigateCommand { get; }

    public void AddGroups()
    {
        for (var i = 0; i < 10; i++)
        {
            AddGroup();
        }
    }

    private void AddGroup()
    {
        var itemGroup = new ItemGroup
        {
            GroupId = _groupIdCounter++
        };

        for (var i = 0; i < 10; i++)
        {
            itemGroup.Add(new Item
            {
                ItemId = _itemIdCounter++
            });
        }

        GroupedItems.Add(itemGroup);
    }

    public void RemoveGroup()
    {
        if (GroupedItems.Count > 0)
            GroupedItems.RemoveAt(Random.Shared.Next(GroupedItems.Count));
    }

    public void Navigate()
    {
        Shell.Current.Navigation.PushAsync(new SampleTwoPage());
    }

    public void NavigatedFrom()
    {
        GroupedItems.Clear();
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}

public class ItemGroup : ObservableCollection<Item>
{
    public required int GroupId { get; set; }
}

public class Item : INotifyPropertyChanged
{
    public Item()
    {
        ToggleHeightCommand = new Command(ToggleHeight);
    }

    public required int ItemId { get; set; }

    public Command ToggleHeightCommand { get; }

    private int _heightRequest = 100;

    public int HeightRequest
    {
        get => _heightRequest;
        set
        {
            if (_heightRequest == value)
                return;

            _heightRequest = value;
            OnPropertyChanged();
        }
    }

    public void ToggleHeight()
    {
        HeightRequest = HeightRequest == 100 ? 200 : 100;
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}