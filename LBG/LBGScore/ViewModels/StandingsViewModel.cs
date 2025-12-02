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

        // Lista agrupada por categoría
        public ObservableCollection<Grouping<string, Standing>> StandingsGrouped { get; set; }
            = new ObservableCollection<Grouping<string, Standing>>();

        public StandingsViewModel()
        {
            _apiService = new ApiService();
            LoadData();
        }

        private async void LoadData()
        {
            _allStandings = await _apiService.GetStandingsAsync();
            foreach (var t in _allStandings)
            {
                t.Logo = t.Logo.Replace("http://181.211.113.66/lbg1", "http://181.39.104.93:5015");
            }

            var grouped = _allStandings
                .GroupBy(x => x.Categoria)                // Agrupa por categoría
                .OrderBy(g => g.Key)                      // Ordena categoría alfabéticamente
                .Select(g =>
                    new Grouping<string, Standing>(
                        g.Key,
                        g.OrderBy(t => t.Posicion)         // Ordena por posición dentro del grupo
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
