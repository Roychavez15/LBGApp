using LBGScore.Models;
using LBGScore.Services;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;

namespace LBGScore.ViewModels
{
    public class StandingsViewModel : INotifyPropertyChanged
    {
        private readonly ApiService _apiService;

        private List<Standing> _allStandings;

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

        // Lista agrupada por categoría
        public ObservableCollection<Grouping<string, Standing>> StandingsGrouped { get; set; }
            = new ObservableCollection<Grouping<string, Standing>>();

        public StandingsViewModel()
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
            _allStandings = await _apiService.GetStandingsAsync();
            foreach (var t in _allStandings)
            {
                t.Logo = t.Logo.Replace("http://181.211.113.66/lbg1", "http://181.39.104.93:5015");
            }

            var grouped = _allStandings
                .GroupBy(x => new { x.Categoria, x.Grupo }) // 🔥 Agrupa por categoría y grupo
                .OrderBy(g => g.Key.Categoria)
                .ThenBy(g => g.Key.Grupo)
                .Select(g =>
                    new Grouping<string, Standing>(
                        $"{g.Key.Categoria} - Grupo {g.Key.Grupo}",   // 🔥 título del bloque
                        g.OrderBy(t => t.Posicion)                   // orden por posición
                    ))
                .ToList();

            StandingsGrouped.Clear();

            foreach (var group in grouped)
                StandingsGrouped.Add(group);

            OnPropertyChanged(nameof(StandingsGrouped));
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
