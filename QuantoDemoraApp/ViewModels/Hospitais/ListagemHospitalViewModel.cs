using Microsoft.Maui.Devices.Sensors;
using QuantoDemoraApp.Models;
using QuantoDemoraApp.Services.Hospitais;
using QuantoDemoraApp.Services.Usuarios;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace QuantoDemoraApp.ViewModels.Hospitais
{
    public class ListagemHospitalViewModel : BaseViewModel
    {
        private HospitalService hService;
        private UsuarioService uService;
        public ObservableCollection<Hospital> Hospitais { get; set; }

        public ListagemHospitalViewModel()
        {
            string token = Preferences.Get("UsuarioToken", string.Empty);
            hService = new HospitalService(token);
            uService = new UsuarioService(token);
            Hospitais = new ObservableCollection<Hospital>();
            _ = ObterHospital();
            _ = CalcularDistancia();
        }

        public async Task ObterHospital()
        {
            try
            {
                Hospitais = await hService.GetHospitaisAsync();
                await CalcularDistancia();
                Hospitais = new ObservableCollection<Hospital>(Hospitais.OrderBy(x => x.DistanciaKm));
                OnPropertyChanged(nameof(Hospitais));
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage
                    .DisplayAlert("Ops", ex.Message, "Ok");
            }
        }

        public async Task CalcularDistancia()
        {
            List<Hospital> listaHospitais = new List<Hospital>(Hospitais);

            Usuario usuario = new Usuario();
            var usuarioId = Preferences.Get("UsuarioId", usuario.IdUsuario);

            Usuario u = await
                uService.GetUsuarioAsync(usuarioId);

            double uLati = (double)u.Latitude;
            double uLong = (double)u.Longitude;
            Location usuarioLoc = new Location(uLati, uLong);

            foreach (Hospital h in listaHospitais)
            {
                if (u.Latitude != null && u.Longitude != null)
                {
                    double hLati = (double)h.Latitude;
                    double hLong = (double)h.Longitude;
                    Location hospitalLoc = new Location(hLati, hLong);

                    h.DistanciaKm = Math.Round(Location.CalculateDistance(usuarioLoc, hospitalLoc, DistanceUnits.Kilometers), 2);
                }
            }
        }

        public async Task PesquisarHospitais(string nmHospital)
        {
            try
            {
                Hospitais = await hService.GetHospitaisByNomeAsync(nmHospital);

                if (Hospitais.Count != 0) 
                {
                    await CalcularDistancia();
                    OnPropertyChanged(nameof(Hospitais));
                }
                else if (Hospitais.Count == 0 && nmHospital.Length >= 5)
                {
                    throw new Exception("Hospital não encontrado!");
                }

            }
            catch (Exception ex)
            {
                await Application.Current.MainPage
                    .DisplayAlert("Ops", ex.Message, "Ok");
                throw;
            }
        }

        public async Task ExibirInformacoesHospital()
        {
            try
            {
                await Shell.Current.GoToAsync("informacoesHospitalView");
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage
                    .DisplayAlert("Ops", ex.Message, "Ok");
            }
        }

        private Hospital hospitalSelecionado;
        public Hospital HospitalSelecionado
        {
            get
            {
                return hospitalSelecionado;
            }
            set
            {
                if (value != null)
                {
                    hospitalSelecionado = value;
                    Shell.Current
                        .GoToAsync($"informacoesHospitalView?hIdHospital={hospitalSelecionado.IdHospital}");
                }
            }
        }

        private double distanciaKm;
        public double DistanciaKm
        {
            get => distanciaKm;
            set
            {
                distanciaKm = value;
                OnPropertyChanged();
            }
        }

        private string hospitalPesquisa;
        public string HospitalPesquisa
        {
            get => hospitalPesquisa;
            set
            {
                if (!string.IsNullOrEmpty(value) && !string.IsNullOrWhiteSpace(value))
                {
                    hospitalPesquisa = value;
                    PesquisarHospitais(hospitalPesquisa);
                    OnPropertyChanged();
                    Hospitais.Clear();
                }
            }
        }
    }
}