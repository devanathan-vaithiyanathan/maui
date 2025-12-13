using System.Collections.ObjectModel;
using Microsoft.Maui.Controls;

namespace Maui.Controls.Sample.Issues
{
    [Issue(IssueTracker.None, 0, "CollectionView doesn't scroll when keyboard appears", 
           PlatformAffected.Windows)]
    public partial class IssueCollectionViewKeyboardAutoScroll : TestContentPage
    {
        public IssueCollectionViewKeyboardAutoScroll()
        {
            InitializeComponent();
            BindingContext = new IssueCollectionViewKeyboardAutoScrollViewModel();
        }

        protected override void Init()
        {
            // TestContentPage requires this method
        }
    }

    public class IssueCollectionViewKeyboardAutoScrollViewModel
    {
        public ObservableCollection<TestItem> Items { get; set; }

        public IssueCollectionViewKeyboardAutoScrollViewModel()
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