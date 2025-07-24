using Microsoft.Maui.Controls;

namespace Maui.Controls.Sample.Issues
{
    public partial class KeyEventsOnEmptyEntry : ContentPage
    {
        private int _eventCount = 0;
        
        public KeyEventsOnEmptyEntry()
        {
            InitializeComponent();
        }
        
        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            _eventCount++;
            KeyDownLabel.Text = $"KeyDown: {e.Key} (Count: {_eventCount})";
            UpdateEventCount();
        }
        
        private void OnKeyUp(object sender, KeyEventArgs e)
        {
            _eventCount++;
            KeyUpLabel.Text = $"KeyUp: {e.Key} (Count: {_eventCount})";
            UpdateEventCount();
        }
        
        private void UpdateEventCount()
        {
            EventCountLabel.Text = $"Total Events: {_eventCount}";
        }
        
        private void OnClearClicked(object sender, System.EventArgs e)
        {
            _eventCount = 0;
            KeyDownLabel.Text = "KeyDown: None";
            KeyUpLabel.Text = "KeyUp: None";
            EventCountLabel.Text = "Total Events: 0";
        }
    }
}