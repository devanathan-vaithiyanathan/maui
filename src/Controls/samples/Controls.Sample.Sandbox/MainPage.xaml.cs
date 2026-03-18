namespace Maui.Controls.Sample;

public partial class MainPage : ContentPage
{
    public MainPage()
    {
         InitializeComponent();
    }
    
    private void OnCounterClicked(object sender, EventArgs e)
    {
        if (this.Parent is ShellContent shellContent)
        {
	    var contentPage = new ContentPage();
		shellContent.Title = "New Page";
	    var label = new Label();
    
	    label.Text = "Page changed";
	    label.FontSize = 30;
	    label.HorizontalOptions = LayoutOptions.Center;
	    label.VerticalOptions = LayoutOptions.Center;
	    contentPage.Content = label;
      
            shellContent.ContentTemplate = null;
            shellContent.Content = contentPage;

		// shellContent.ContentTemplate = null;
		// shellContent.Content = contentPage;
		// contentPage.Handler = this.Handler;
    
            // there is no way to reload shell content
          }
    }
}