namespace LBGScore.ViewModels
{
    using LBGScore.Models;
    using LBGScore.Services;
    using System.Collections.ObjectModel;
    using System.ComponentModel;

    public class ResultadosViewModel : INotifyPropertyChanged
    {
        private readonly ApiService _apiService;

        public ObservableCollection<Partido> Partidos { get; set; } = new();

        public ResultadosViewModel()
        {
            _apiService = new ApiService();
            LoadData();
        }

        private async void LoadData()
        {
            try
            {
                var all = await _apiService.GetPartidosFechaAsync();

                var ordenados = all
                    .OrderBy(p => p.FechaHoraOrdenable)
                    .ToList();

                Partidos.Clear();
                foreach (var p in ordenados)
                    Partidos.Add(p);

                OnPropertyChanged(nameof(Partidos));
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Error cargando partidos: " + ex);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }

}
