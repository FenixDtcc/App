using QuantoDemoraApp.Models;
using QuantoDemoraApp.Services.Hospitais;
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

        public InformacoesHospitalViewModel()
        {
            string token = Preferences.Get("UsuarioToken", string.Empty);
            hService = new HospitalService(token);
        }

        private string cnpj;
        private string razaoSocial;
        private string endereco;
        private string numero;
        private string complemento;
        private string bairro;
        private string cidade;
        private string uf;
        private string cep; 

        public string Cnpj
        {
            get => cnpj;
            set
            {
                cnpj = value;
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

        public string CEP
        {
            get => cep;
            set
            {
                cep = value;
                OnPropertyChanged();
            }
        }

        public async void CarregarHospital()
        {
            try
            {
                Hospital h = await hService.GetHospitalAsync(int.Parse(hospitalSelecionadoId));

                this.RazaoSocial = h.RazaoSocial;
                this.Cnpj = h.Cnpj;
                this.Endereco = h.Endereco;
                this.Numero = h.Numero;
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage
                    .DisplayAlert("Ops", ex.Message + "Detalhes: " + ex.InnerException, "Ok");
            }
        }

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

    }
}
