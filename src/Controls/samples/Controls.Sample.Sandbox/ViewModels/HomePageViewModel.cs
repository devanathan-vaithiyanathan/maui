using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;

namespace Maui.Controls.Sample.ViewModels;

public partial class HomePageViewModel : ObservableObject
{
    private readonly Random _rnd = new();

    [ObservableProperty]
    private bool isBusy;

    [ObservableProperty]
    private HomePageCardItem? selectedCard;

    public ObservableCollection<HomePageCardGroup> Items { get; } = [];

    public HomePageViewModel()
    {
        LoadData();
    }

    public void UpdateRandomCards()
    {
        if (Items.Count == 0)
        {
            return;
        }

        int cardsToUpdate = _rnd.Next(1, 6);

        for (int i = 0; i < cardsToUpdate; i++)
        {
            int groupIdx = _rnd.Next(Items.Count);
            var group = Items[groupIdx];

            if (group.Count == 0)
            {
                continue;
            }

            int cardIdx = _rnd.Next(group.Count);
            var card = group[cardIdx];

            int newStatus = _rnd.Next(3);
            card.Status = getCardStatus(newStatus);
            card.BackgroundColor = getCardBg(newStatus);
        }
    }

    private string getCardStatus(int cardStatus)
    {
        return cardStatus switch
        {
            0 => "Error",
            1 => "Warning",
            2 => "Success",
            _ => "Error"
        };
    }

    private string getCardBg(int cardStatus)
    {
        return cardStatus switch
        {
            0 => "#FF5252",
            1 => "#FFA726",
            2 => "#66BB6A",
            _ => "#FF5252"
        };
    }

    [RelayCommand]
    private async Task RefreshData()
    {
        IsBusy = true;
        await Task.Delay(2000);
        LoadData();
        IsBusy = false;
    }

    [RelayCommand]
    private Task CardSelected()
    {
        if (SelectedCard is not null)
        {
            System.Diagnostics.Debug.WriteLine($"Card selected: {SelectedCard.Title}");
        }
        return Task.CompletedTask;
    }

    private void LoadData()
    {
        Items.Clear();

        for (int groupIndex = 1; groupIndex <= 15; groupIndex++)
        {
            var group = new HomePageCardGroup(
                id: Guid.NewGuid(),
                name: $"Area {groupIndex}",
                hexColor: getCardStatus(groupIndex),
                index: (ushort)groupIndex
            );

            int cardsPerGroup = _rnd.Next(22);
            for (int i = 1; i <= cardsPerGroup; i++)
            {
                var status = _rnd.Next(3);
                group.Add(new HomePageCardItem
                {
                    Title = $"Card {i} - {group.Name}",
                    Status = getCardStatus(status),
                    BackgroundColor = getCardBg(status)
                });
            }

            Items.Add(group);
        }
    }
}

public class HomePageCardGroup : ObservableCollection<HomePageCardItem>
{
    public HomePageCardGroup(Guid id, string name, string hexColor, ushort index)
    {
        Id = id;
        Name = name;
        HexColor = hexColor;
        Index = index;
        UseColors = true;
    }

    public Guid Id { get; }
    public string Name { get; }
    public string HexColor { get; }
    public ushort Index { get; }
    public bool UseColors { get; }
}

public class HomePageCardItem : CommunityToolkit.Mvvm.ComponentModel.ObservableObject
{
    private string _title = string.Empty;
    private string _status = string.Empty;
    private string _backgroundColor = string.Empty;

    public string Title
    {
        get => _title;
        set => SetProperty(ref _title, value);
    }

    public string Status
    {
        get => _status;
        set => SetProperty(ref _status, value);
    }

    public string BackgroundColor
    {
        get => _backgroundColor;
        set => SetProperty(ref _backgroundColor, value);
    }
}