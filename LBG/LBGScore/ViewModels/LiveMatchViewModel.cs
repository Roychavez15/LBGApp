//using LBGScore.Models;
//using LBGScore.Services;
//using System;
//using System.Collections.Generic;
//using System.Collections.ObjectModel;
//using System.ComponentModel;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace LBGScore.ViewModels
//{
//    public class LiveMatchViewModel : INotifyPropertyChanged
//    {
//        private readonly ApiService _apiService;

//        private int _carouselPosition;
//        public int CarouselPosition
//        {
//            get => _carouselPosition;
//            set
//            {
//                _carouselPosition = value;
//                OnPropertyChanged(nameof(CarouselPosition));
//            }
//        }
//        private LiveMatch _match;
//        public LiveMatch Match
//        {
//            get => _match;
//            set { _match = value; OnPropertyChanged(nameof(Match)); }
//        }
//        private bool _hasMatch;
//        public bool HasMatch
//        {
//            get => _hasMatch;
//            set
//            {
//                _hasMatch = value;
//                OnPropertyChanged(nameof(HasMatch));
//            }
//        }
//        public ObservableCollection<Auspiciante> Auspiciantes { get; set; } = new();

//        public LiveMatchViewModel()
//        {
//            _apiService = new ApiService();
//            Device.StartTimer(TimeSpan.FromSeconds(6), () =>
//            {
//                if (Auspiciantes?.Count > 1)
//                {
//                    CarouselPosition = (CarouselPosition + 1) % Auspiciantes.Count;
//                }
//                return true;
//            });
//            LoadData();
//        }

//        private async void LoadData()
//        {
//            var partidos = await _apiService.GetPartidosFechaAsync();

//            var partidoActivo = partidos.FirstOrDefault(p => p.Estado == "1");

//            Match = partidoActivo != null ? ConvertToLiveMatch(partidoActivo) : null;

//            if (Match != null)
//            {
//                HasMatch = true;
//            }
//            else
//            {
//                HasMatch = false;
//            }
//            // cargar auspiciantes
//            var sponsors = await _apiService.GetAuspiciantesAsync();

//            Auspiciantes.Clear();
//            foreach (var a in sponsors)
//                Auspiciantes.Add(a);
//        }
//        private LiveMatch ConvertToLiveMatch(Partido p)
//        {
//            return new LiveMatch
//            {
//                Dia = p.Dia,
//                Hora = p.Hora,
//                Local = p.Local,
//                Visitante = p.Visitante,
//                GolesLocal = p.GL,            // usa helper seguro
//                GolesVisitante = p.GV,        // usa helper seguro
//                Categoria = p.Categoria,
//                Tiempo = p.Tiempo,
//                LogoL = p.LogoL,
//                LogoV = p.LogoV
//            };
//        }

//        public event PropertyChangedEventHandler PropertyChanged;
//        private void OnPropertyChanged(string name) =>
//            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
//    }

//}
using LBGScore.Models;
using LBGScore.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using Microsoft.Maui.Dispatching;

namespace LBGScore.ViewModels
{
    public class LiveMatchViewModel : INotifyPropertyChanged
    {
        private readonly ApiService _apiService;

        private System.Timers.Timer _refreshTimer;

        public ObservableCollection<Auspiciante> Auspiciantes { get; set; } = new();

        private int _carouselPosition;
        public int CarouselPosition
        {
            get => _carouselPosition;
            set
            {
                _carouselPosition = value;
                OnPropertyChanged(nameof(CarouselPosition));
            }
        }

        private LiveMatch _match;
        public LiveMatch Match
        {
            get => _match;
            set { _match = value; OnPropertyChanged(nameof(Match)); }
        }

        private bool _hasMatch;
        public bool HasMatch
        {
            get => _hasMatch;
            set
            {
                _hasMatch = value;
                OnPropertyChanged(nameof(HasMatch));
            }
        }

        public LiveMatchViewModel()
        {
            _apiService = new ApiService();

            // Carrusel cada 6 segundos (lo que ya tenías)
            Device.StartTimer(TimeSpan.FromSeconds(6), () =>
            {
                if (Auspiciantes?.Count > 1)
                {
                    CarouselPosition = (CarouselPosition + 1) % Auspiciantes.Count;
                }
                return true;
            });

            InicializarTimer();  // ⬅ Añadimos el refresco cada minuto

            LoadData();
        }

        // -----------------------------------------
        // 🔄 TIMER DE REFRESCO CADA 1 MINUTO
        // -----------------------------------------
        private void InicializarTimer()
        {
            _refreshTimer = new System.Timers.Timer(60000); // 1 min
            _refreshTimer.Elapsed += async (s, e) =>
            {
                await MainThread.InvokeOnMainThreadAsync(async () =>
                {
                    await LoadDataInternal();  // Solo recarga
                });
            };
            _refreshTimer.AutoReset = true;
            _refreshTimer.Enabled = false; // La Page lo activa/desactiva
        }

        public void StartAutoRefresh() => _refreshTimer.Enabled = true;
        public void StopAutoRefresh() => _refreshTimer.Enabled = false;

        // -----------------------------------------
        // 🔽 LOAD DATA DE USO INTERNO
        // -----------------------------------------
        private async Task LoadDataInternal()
        {
            var partidos = await _apiService.GetPartidosFechaAsync();

            var partidoActivo = partidos.FirstOrDefault(p => p.Estado == "1" || p.Estado == "D");

            Match = partidoActivo != null ? ConvertToLiveMatch(partidoActivo) : null;
            HasMatch = Match != null;

            // auspiciantes
            var sponsors = await _apiService.GetAuspiciantesAsync();

            Auspiciantes.Clear();
            foreach (var a in sponsors)
                Auspiciantes.Add(a);
        }

        // -----------------------------------------
        // 🔽 LOADDATA ORIGINAL PUBLICO
        // -----------------------------------------
        private async void LoadData()
        {
            await LoadDataInternal();
        }

        private LiveMatch ConvertToLiveMatch(Partido p)
        {
            return new LiveMatch
            {
                Dia = p.Dia,
                Hora = p.Hora,
                Local = p.Local,
                Visitante = p.Visitante,
                GolesLocal = p.GL,
                GolesVisitante = p.GV,
                Categoria = p.Categoria,
                Tiempo = p.Tiempo,
                LogoL = p.LogoL,
                LogoV = p.LogoV
            };
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string name) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
