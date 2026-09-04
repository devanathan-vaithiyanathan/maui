#nullable enable
using System.ComponentModel;

[Issue(IssueTracker.Github, 38188, "Setting Shadow on SearchBar throws a COMException", PlatformAffected.UWP)]
public class Issue38188 : NavigationPage
{
    public Issue38188() : base(new Issue38188ContentPage())
    {
    }

    sealed class Issue38188ContentPage : ContentPage
    {
        readonly SearchBarViewModel _viewModel = new();

        public Issue38188ContentPage()
        {
            Title = "SearchBar Shadow";
            BindingContext = _viewModel;

            var searchBar = new SearchBar
            {
                AutomationId = "Issue38188SearchBar",
                Background = Colors.White,
                Placeholder = "Search",
                PlaceholderColor = Colors.DimGray,
                TextColor = Colors.Black,
                VerticalOptions = LayoutOptions.Center
            };
            searchBar.SetBinding(ShadowProperty, nameof(SearchBarViewModel.Shadow));

            var options = new Button
            {
                AutomationId = "Issue38188Options",
                Text = "Options"
            };
            options.Clicked += NavigateToOptionsPage;

            Content = new StackLayout
            {
                Padding = 24,
                Children =
                {
                    searchBar,
                    options
                }
            };
        }

        async void NavigateToOptionsPage(object? sender, EventArgs args)
        {
            _viewModel.Reset();
            await Navigation.PushAsync(new SearchBarOptionsPage(_viewModel));
        }
    }

    sealed class SearchBarOptionsPage : ContentPage
    {
        readonly SearchBarViewModel _viewModel;

        public SearchBarOptionsPage(SearchBarViewModel viewModel)
        {
            Title = "OptionsPage";
            BindingContext = _viewModel = viewModel;

            var apply = new ToolbarItem
            {
                AutomationId = "Issue38188Apply",
                Text = "Apply"
            };
            apply.Clicked += Apply;
            ToolbarItems.Add(apply);

            var shadowButton = new RadioButton
            {
                AutomationId = "Issue38188ShadowTrueButton",
                Content = "True",
                GroupName = "Shadow"
            };
            shadowButton.CheckedChanged += SetShadow;

            Content = new StackLayout
            {
                Padding = 24,
                Children =
                {
                    new Label { Text = "Shadow:", VerticalOptions = LayoutOptions.Center },
                    shadowButton
                }
            };
        }

        async void Apply(object? sender, EventArgs args)
        {
            await Navigation.PopAsync();
        }

        void SetShadow(object? sender, CheckedChangedEventArgs args)
        {
            if (args.Value)
            {
                _viewModel.Shadow = new Shadow
                {
                    Brush = Colors.Violet,
                    Radius = 10,
                    Offset = Point.Zero,
                    Opacity = 1f
                };
            }
        }
    }

    sealed class SearchBarViewModel : INotifyPropertyChanged
    {
        Shadow? _shadow;

        public Shadow? Shadow
        {
            get => _shadow;
            set
            {
                _shadow = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Shadow)));
            }
        }

        public void Reset()
        {
            Shadow = null;
        }

        public event PropertyChangedEventHandler? PropertyChanged;
    }
}