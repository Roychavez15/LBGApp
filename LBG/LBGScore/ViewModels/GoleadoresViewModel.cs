using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text.Json;
using LBGScore.Models;
using LBGScore.Services;

namespace LBGScore.ViewModels
{
    public class GoleadoresGroup : ObservableCollection<Goleador>
    {
        public string Key { get; set; }

        public GoleadoresGroup(string key, IEnumerable<Goleador> goleadores) : base(goleadores)
        {
            Key = key;
        }
    }

    public class GoleadoresViewModel : INotifyPropertyChanged
    {
        private readonly ApiService _apiService;

        public ObservableCollection<GoleadoresGroup> GoleadoresGrouped { get; set; } = new();

        public GoleadoresViewModel()
        {
            _apiService = new ApiService();
            LoadData();
        }

        private async void LoadData()
        {
            var all = await _apiService.GetGoleadoresAsync();

            var groups = all
                .GroupBy(g => g.Categoria)
                .Select(g =>
                {
                    var ordered = g
                        .OrderByDescending(x => x.GolesInt)
                        .Select((item, index) =>
                        {
                            item.Pos = index + 1;
                            return item;
                        })
                        .ToList();

                    return new GoleadoresGroup(g.Key, ordered);
                });

            GoleadoresGrouped.Clear();

            foreach (var group in groups)
                GoleadoresGrouped.Add(group);
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
