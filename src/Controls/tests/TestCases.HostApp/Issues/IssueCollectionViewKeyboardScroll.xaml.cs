using System.Collections.ObjectModel;
using Microsoft.Maui.Controls;

namespace Maui.Controls.Sample.Issues
{
    [Issue(IssueTracker.None, 0, "CollectionView doesn't scroll when keyboard appears", 
           PlatformAffected.Windows)]
    public partial class IssueCollectionViewKeyboardScroll : ContentPage
    {
        public IssueCollectionViewKeyboardScroll()
        {
            InitializeComponent();
            BindingContext = new IssueCollectionViewKeyboardScrollViewModel();
        }
    }

    public class IssueCollectionViewKeyboardScrollViewModel
    {
        public ObservableCollection<TestItem> Items { get; set; }

        public IssueCollectionViewKeyboardScrollViewModel()
        {
            Items = new ObservableCollection<TestItem>();
            
            // Create 30 items to ensure scrolling is needed
            for (int i = 1; i <= 30; i++)
            {
                Items.Add(new TestItem
                {
                    Label = $"Item {i}",
                    Value = $"{i}",
                    AutomationId = $"Entry{i}"
                });
            }
        }
    }

    public class TestItem
    {
        public string Label { get; set; } = string.Empty;
        public string Value { get; set; } = string.Empty;
        public string AutomationId { get; set; } = string.Empty;
    }
}