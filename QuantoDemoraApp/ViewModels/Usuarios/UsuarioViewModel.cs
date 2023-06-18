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
        public ICommand AbrirLGPDCommand { get; set; }

        public void InicializarCommands()
        {
            CadastrarCommand = new Command(async () => await Cadastrar());
            AutenticarCommand = new Command(async () => await Autenticar());
            DirecionarCadastroCommand = new Command(async () => await DirecionarParaCadastro());
            AbrirLGPDCommand = new Command(async () => await AbrirUrlLGPD());
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

        private bool checkBox;
        public bool CheckBox
        {
            get { return checkBox; }
            set
            {
                checkBox = value;
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

                if (String.IsNullOrEmpty(u.NomeUsuario) || String.IsNullOrWhiteSpace(u.NomeUsuario) ||
                    String.IsNullOrEmpty(u.PasswordString) ||String.IsNullOrWhiteSpace(u.PasswordString)||
                    String.IsNullOrEmpty(u.Email) || String.IsNullOrWhiteSpace(u.Email) ||
                    String.IsNullOrEmpty(u.Cpf) || String.IsNullOrWhiteSpace(u.Cpf))
                {
                    throw new Exception("Favor informar os dados acima para efeturar o cadastro.");
                }

                if (u.NomeUsuario.Length > 25)
                {
                    throw new Exception("O nome de usuário pode conter no máximo 25 caracteres.");
                }

                if (u.Cpf.Length == 11)
                {
                    u.Cpf = u.Cpf.Insert(3, ".").Insert(7, ".").Insert(11, "-");
                }
                if (u.Cpf.Length < 14)
                {
                    throw new Exception("CPF inválido.");
                }

                char primeiroCaractere = char.Parse(u.Email.Substring(0, 1));
                char ultimoCaractere = char.Parse(u.Email.Substring(u.Email.Length - 1, 1));

                if (char.IsDigit(primeiroCaractere) || char.IsDigit(ultimoCaractere)
                    || char.IsSymbol(primeiroCaractere) || char.IsSymbol(ultimoCaractere)
                    || char.IsWhiteSpace(primeiroCaractere) || char.IsWhiteSpace(ultimoCaractere)
                    || char.IsPunctuation(primeiroCaractere) || char.IsPunctuation(ultimoCaractere))
                {
                    throw new Exception("E-mail inválido.");
                }

                if (u.Email.Length > 50)
                {
                    throw new Exception("O e-mail pode conter no máximo 50 caracteres.");
                }

                Usuario uCadastrado = await uService.PostCadastrarAsync(u);
                if (uCadastrado.IdUsuario != 0)
                {
                    string mensagem = $"Paciente {uCadastrado.NomeUsuario} registrado com sucesso.";

                    await Application.Current.MainPage.DisplayAlert("Informação", mensagem, "Ok");

                    await Shell.Current.GoToAsync("..");

                    //await Application.Current.MainPage.Navigation.PopAsync();
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
                    uLocation.Latitude = location.Latitude;
                    uLocation.Longitude = location.Longitude;

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

        public async Task<bool> AbrirUrlLGPD()
        {
            try
            {
                return await uService.AbrirLGPDAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion
    }
}