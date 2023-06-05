using QuantoDemoraApp.Models;
using QuantoDemoraApp.Models.Enuns;
using QuantoDemoraApp.Services.Especialidades;
using QuantoDemoraApp.Services.Hospitais;
using QuantoDemoraApp.Services.HospitalEspecialidades;
using QuantoDemoraApp.Services.Logradouros;
using QuantoDemoraApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace QuantoDemoraApp.ViewModels.Hospitais
{
    [QueryProperty("HospitalSelecionadoId", "hIdHospital")]

    public class InformacoesHospitalViewModel : BaseViewModel
    {
        private HospitalService hService;
        private LogradouroService lService;
        private EspecialidadeService eService;

        public ObservableCollection<Especialidade> Especialidades { get; set; }

        public ICommand AbrirMapaCommand { get; set; }

        public InformacoesHospitalViewModel()
        {
            string token = Preferences.Get("UsuarioToken", string.Empty);
            hService = new HospitalService(token);
            lService = new LogradouroService(token);
            eService = new EspecialidadeService(token);
            AbrirMapaCommand = new Command(async () => await AbrirMapaHospital());
            Especialidades = new ObservableCollection<Especialidade>();
            _ = ObterEspecialidades();

        }

        private int id;
        private string dsLogradouro;
        private string razaoSocial;
        private string endereco;
        private string numero;
        private string complemento;
        private string bairro;
        private string cidade;
        private string uf;
        private string cep;
        private double latitude;
        private double longitude;
        private string idGoogleMaps;
        //private int idEspecialidade;
        //private string dsEspecialidade;

        public int Id
        {
            get => id;
            set
            {
                id = value;
                OnPropertyChanged();
            }
        }

        public string DsLogradouro
        {
            get => dsLogradouro;
            set
            {
                dsLogradouro = value;
                OnPropertyChanged();
            }
        }

        public string RazaoSocial
        {
            get => razaoSocial;
            set
            {
                razaoSocial = value;
                OnPropertyChanged();
            }
        }

        public string Endereco
        {
            get => endereco;
            set
            {
                endereco = value;
                OnPropertyChanged();
            }
        }

        public string Numero
        {
            get => numero;
            set
            {
                numero = value;
                OnPropertyChanged();
            }
        }

        public string Complemento
        {
            get => complemento;
            set
            {
                complemento = value;
                OnPropertyChanged();
            }
        }

        public string Bairro
        {
            get => bairro;
            set
            {
                bairro = value;
                OnPropertyChanged();
            }
        }

        public string Cidade
        {
            get => cidade;
            set
            {
                cidade = value;
                OnPropertyChanged();
            }
        }

        public string Uf
        {
            get => uf;
            set
            {
                uf = value;
                OnPropertyChanged();
            }
        }

        public string Cep
        {
            get => cep;
            set
            {
                cep = value;
                OnPropertyChanged();
            }
        }

        public double Latitude
        {
            get => latitude;
            set
            {
                latitude = value;
                OnPropertyChanged();
            }
        }

        public double Longitude
        {
            get => longitude;
            set
            {
                longitude = value;
                OnPropertyChanged();
            }
        }

        public string IdGoogleMaps
        {
            get => idGoogleMaps;
            set
            {
                idGoogleMaps = value;
                OnPropertyChanged();
            }
        }

        //public int IdEspecialidade
        //{
        //    get => idEspecialidade;
        //    set
        //    {
        //        idEspecialidade = value;
        //        OnPropertyChanged();
        //    }
        //}

        //public string DsEspecialidade
        //{
        //    get => dsEspecialidade;
        //    set
        //    {
        //        dsEspecialidade = value;
        //        OnPropertyChanged();
        //    }
        //}


        private string hospitalSelecionadoId;
        public string HospitalSelecionadoId
        {
            set
            {
                if (value != null)
                {
                    hospitalSelecionadoId = Uri.UnescapeDataString(value);
                    CarregarHospital();
                }
            }
        }

        public async void CarregarHospital()
        {
            try
            {
                Hospital h = await hService.GetHospitalAsync(int.Parse(hospitalSelecionadoId));
                Logradouro l = await lService.GetLogradouroAsync(h.IdLogradouro);

                this.DsLogradouro = l.DsLogradouro;
                this.RazaoSocial = h.RazaoSocial;
                this.Endereco = h.Endereco;
                this.Numero = h.Numero;
                this.Cep = h.Cep;
                this.Bairro = h.Bairro;
                this.Cidade = h.Cidade;
                this.Uf = h.Uf;
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage
                    .DisplayAlert("Ops", ex.Message + "Detalhes: " + ex.InnerException, "Ok");
            }
        }

        public async Task<bool> AbrirMapaHospital()
        {
            try
            {
                Hospital h = await hService.GetHospitalAsync(int.Parse(hospitalSelecionadoId));
                return await hService.GetLocalizacaoAsync(h.Latitude, h.Longitude, h.IdGoogleMaps);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task ObterEspecialidades()
        {
            try
            {
                Especialidades = await eService.GetEspecialidadesAsync();
                OnPropertyChanged(nameof(Especialidades));
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage
                    .DisplayAlert("Ops", ex.Message + " Detalhes: " + ex.InnerException, "Ok");
            }
        }
    }
}
