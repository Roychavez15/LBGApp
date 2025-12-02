using LBGScore.ViewModels;

namespace LBGScore.Views;

public partial class StandingsPage : ContentPage
{
	public StandingsPage()
	{
		InitializeComponent();
        BindingContext = new StandingsViewModel();
    }
}