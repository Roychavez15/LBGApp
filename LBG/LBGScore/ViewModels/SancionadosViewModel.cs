

namespace LBGScore.ViewModels
{

    using LBGScore.Models;
    using LBGScore.Services;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    public class SancionadosViewModel : INotifyPropertyChanged
    {
        private readonly ApiService _apiService;

        public ObservableCollection<Sancionado> Sancionados { get; set; } = new();

        public Command RefreshCommand { get; }

        private bool _isRefreshing;
        public bool IsRefreshing
        {
            get => _isRefreshing;
            set
            {
                _isRefreshing = value;
                OnPropertyChanged(nameof(IsRefreshing));
            }
        }

        public SancionadosViewModel()
        {
            _apiService = new ApiService();
            RefreshCommand = new Command(async () => await RefreshAsync());
            LoadData();
        }
        public async Task RefreshAsync()
        {
            try
            {
                IsRefreshing = true;
                LoadData();
            }
            finally
            {
                await Task.Delay(500);
                IsRefreshing = false;
            }
        }
        private async void LoadData()
        {
            
            var list = await _apiService.GetSancionadosAsync();

            var filtrados = list
                .Where(s => !string.IsNullOrWhiteSpace(s.Equipo))
                .ToList();

            Sancionados.Clear();

            foreach (var s in filtrados)
                Sancionados.Add(s);


        }

        public event PropertyChangedEventHandler PropertyChanged;
        void OnPropertyChanged(string name)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }

}
