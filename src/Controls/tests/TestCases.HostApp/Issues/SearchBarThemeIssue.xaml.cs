namespace Maui.Controls.Sample.Issues
{
    public partial class SearchBarThemeIssue : ContentPage
    {
        public SearchBarThemeIssue()
        {
            InitializeComponent();
        }

        private void OnChangeThemeClicked(object sender, EventArgs e)
        {
            Application.Current.UserAppTheme = AppTheme.Dark;
            statusLabel.Text = "Current theme: Dark";
        }

        private void OnResetThemeClicked(object sender, EventArgs e)
        {
            Application.Current.UserAppTheme = AppTheme.Light;
            statusLabel.Text = "Current theme: Light";
        }
    }
}