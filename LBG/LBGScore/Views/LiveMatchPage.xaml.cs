using LBGScore.ViewModels;

namespace LBGScore.Views;

public partial class LiveMatchPage : ContentPage
{
	public LiveMatchPage()
	{
		InitializeComponent();
	}
    protected override void OnAppearing()
    {
        base.OnAppearing();
        (BindingContext as LiveMatchViewModel)?.StartAutoRefresh();
    }
    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        (BindingContext as LiveMatchViewModel)?.StopAutoRefresh();
    }
}