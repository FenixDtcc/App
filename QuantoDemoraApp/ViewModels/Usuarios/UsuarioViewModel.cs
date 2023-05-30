using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using QuantoDemoraApp.Models;
using QuantoDemoraApp.Services.Usuarios;
using QuantoDemoraApp.Views;

namespace QuantoDemoraApp.ViewModels.Usuarios
{
    public class UsuarioViewModel : BaseViewModel
    {
        private UsuarioService uService;
        public ICommand CadastrarCommand { get; set; }
        public ICommand AutenticarCommand { get; set; }
        public ICommand DirecionarCadastroCommand { get; set; }

        public void InicializarCommands()
        {
            CadastrarCommand = new Command(async () => await Cadastrar());
            AutenticarCommand = new Command(async () => await Autenticar());
            DirecionarCadastroCommand = new Command(async () => await DirecionarParaCadastro());
        }

        public UsuarioViewModel()
        {
            string token = Preferences.Get("UsuarioToken", string.Empty);
            uService = new UsuarioService(token);
            InicializarCommands();
        }

        #region AtributosPropriedades

        private string login = string.Empty;
        public string Login
        {
            get { return login; }
            set
            {
                login = value;
                OnPropertyChanged();
            }
        }

        private string senha = string.Empty;
        public string Senha
        {
            get { return senha; }
            set
            {
                senha = value;
                OnPropertyChanged();
            }
        }

        private string cpf = string.Empty;
        public string Cpf
        {
            get { return cpf; }
            set
            {
                cpf = value;
                OnPropertyChanged();
            }
        }

        private string email = string.Empty;
        [EmailAddress]
        public string Email
        {
            get { return email; }
            set
            {
                email = value;
                OnPropertyChanged();
            }
        }
        #endregion


        #region Métodos
        public async Task Cadastrar()
        {
            try
            {
                Usuario u = new Usuario();
                u.NomeUsuario = Login;
                u.PasswordString = Senha;
                u.Email = Email;
                u.Cpf = Cpf;

                if (String.IsNullOrEmpty(u.NomeUsuario) ||
                    String.IsNullOrEmpty(u.PasswordString) ||
                    String.IsNullOrEmpty(u.Email) ||
                    String.IsNullOrEmpty(u.Cpf))
                {
                    throw new Exception("Favor informar os dados acima para efeturar o cadastro.");
                }

                if (u.Cpf.Length == 11)
                {
                    u.Cpf = u.Cpf.Insert(3, ".").Insert(7, ".").Insert(11, "-");
                }
                if (u.Cpf.Length < 14)
                {
                    throw new Exception("CPF inválido.");
                }

                Usuario uCadastrado = await uService.PostCadastrarAsync(u);
                if (uCadastrado.IdUsuario != 0)
                {
                    string mensagem = $"Paciente {uCadastrado.NomeUsuario} registrado com sucesso.";
                    await Application.Current.MainPage.DisplayAlert("Informação", mensagem, "Ok");

                    await Application.Current.MainPage.Navigation.PopAsync();
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Informação", ex.Message + "Detalhes: " + ex.InnerException, "Ok");
            }
        }

        private CancellationTokenSource _cancelTokenSource;
        private bool _isCheckingLocation;
        public async Task Autenticar()
        {
            try
            {
                Usuario u = new Usuario();
                u.NomeUsuario = Login;
                u.PasswordString = Senha;

                Usuario uAutenticado = await uService.PostAutenticarAsync(u);

                if (!string.IsNullOrEmpty(uAutenticado.Token))
                {
                    string mensagem = $"Bem vindo(a) {uAutenticado.NomeUsuario}";

                    Preferences.Set("UsuarioId", uAutenticado.IdUsuario);
                    Preferences.Set("UsuarioUsername", uAutenticado.NomeUsuario);
                    Preferences.Set("UsuarioPerfil", uAutenticado.TpUsuario);
                    Preferences.Set("UsuarioToken", uAutenticado.Token);

                    _isCheckingLocation = true;
                    _cancelTokenSource = new CancellationTokenSource();
                    GeolocationRequest request = new GeolocationRequest(GeolocationAccuracy.Medium, TimeSpan.FromSeconds(10));

                    Location location = await Geolocation
                        .Default.GetLocationAsync(request, _cancelTokenSource.Token);

                    Usuario uLocation = new Usuario();
                    uLocation.IdUsuario = uAutenticado.IdUsuario;
                    uLocation.Latitude = uAutenticado.Latitude;
                    uLocation.Longitude = uAutenticado.Longitude;

                    UsuarioService uServiceLoc = new UsuarioService(uAutenticado.Token);
                    await uServiceLoc.PutAtualizarLocalizacaoAsync(uLocation);

                    await Application.Current.MainPage.DisplayAlert("Informação", mensagem, "Ok");
                    Application.Current.MainPage = new AppShell();
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert("Informação", "Dados incorretos :(", "Ok");
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Informação", ex.Message + "Detalhes" + ex.InnerException, "Ok");

            }
        }

        public async Task DirecionarParaCadastro()
        {
            try
            {
                await Application.Current.MainPage.Navigation.PushAsync(new CadastroView());
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Informação", ex.Message + "Detalhes" + ex.InnerException, "Ok");
            }
        }
        #endregion
    }
}