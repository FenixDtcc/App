using QuantoDemoraApp.Models;
using QuantoDemoraApp.Services.Hospitais;
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
        public ObservableCollection<Hospital> Hospitais { get; set; }
        public ListagemHospitalViewModel()
        {
            string token = Preferences.Get("UsuarioToken", string.Empty);
            hService = new HospitalService(token);
            Hospitais = new ObservableCollection<Hospital>();
            _ = ObterHospital(); 
        }

        public async Task ObterHospital()
        {
            try
            {
                Hospitais = await hService.GetHospitaisAsync();
                OnPropertyChanged(nameof(Hospitais));
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage
                    .DisplayAlert("Ops", ex.Message + " Detalhes: " + ex.InnerException, "Ok");
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
                    .DisplayAlert("Ops", ex.Message + " Detalhes: " + ex.InnerException, "Ok");
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
    }
}
