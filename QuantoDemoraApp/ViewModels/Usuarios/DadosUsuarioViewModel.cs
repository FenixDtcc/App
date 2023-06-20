using QuantoDemoraApp.Models;
using QuantoDemoraApp.Services.Usuarios;
using QuantoDemoraApp.Views.Usuarios;
using System.ComponentModel.DataAnnotations;
using System.Windows.Input;

namespace QuantoDemoraApp.ViewModels.Usuarios
{
    internal class DadosUsuarioViewModel : BaseViewModel
    {
        UsuarioService uService;
        public ICommand AbrirAlteracaoDadosUsuarioCommand { get; set; }
        public ICommand AlterarNomeCommand { get; set; }
        public ICommand AlterarEmailCommand { get; set; }
        public ICommand AlterarSenhaCommand { get; set; }
        public ICommand DeletarCommand { get; set; }

        public DadosUsuarioViewModel()
        {
            string token = Preferences.Get("UsuarioToken", string.Empty);
            uService = new UsuarioService(token);
            InicializarCommands();
            _ = CarregarDadosUsuario();
        }

        public void InicializarCommands()
        {
            AbrirAlteracaoDadosUsuarioCommand = new Command(AbrirAlteracaoDadosUsuario);
            AlterarNomeCommand = new Command(AlterarNomeUsuario);
            AlterarEmailCommand = new Command(AlterarEmailUsuario);
            AlterarSenhaCommand = new Command(AlterarSenhaUsuario);
            DeletarCommand = new Command(DeletarUsuario);
        }

        #region AtributosPropriedades

        public int id;
        public string nome;
        public string cpf;
        public string email;
        public string senha;

        public int Id
        {
            get => id;
            set
            {
                id = value;
                OnPropertyChanged();
            }
        }

        public string Nome
        {
            get => nome;
            set
            {
                nome = value;
                OnPropertyChanged();
            }
        }

        public string Cpf
        {
            get => cpf;
            set
            {
                cpf = value;
                OnPropertyChanged();
            }
        }

        [EmailAddress]
        public string Email
        {
            get => email;
            set
            {
                email = value;
                OnPropertyChanged();
            }
        }

        public string Senha
        {
            get => senha;
            set
            {
                senha = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region Métodos

        public async Task CarregarDadosUsuario()
        {
            try
            {
                Usuario usuario = new Usuario();
                var usuarioId = Preferences.Get("UsuarioId", usuario.IdUsuario);

                Usuario u = await
                    uService.GetUsuarioAsync(usuarioId);

                this.Cpf = u.Cpf;
                this.Nome = u.NomeUsuario;
                this.Email = u.Email;
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage
                    .DisplayAlert("Ops", ex.Message, "Ok");
            }
        }

        public async void AbrirAlteracaoDadosUsuario()
        {
            try
            {
                await Shell.Current.GoToAsync("alterarDadosUsuarioView");
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage
                    .DisplayAlert("Ops", ex.Message, "Ok");
            }
        }

        public async void AlterarNomeUsuario()
        {
            try
            {
                Usuario usuario = new Usuario();
                var usuarioId = Preferences.Get("UsuarioId", usuario.IdUsuario);

                Usuario u = await
                    uService.GetUsuarioAsync(usuarioId);

                u.NomeUsuario = this.nome;

                if (u.IdUsuario == usuarioId)
                {
                    if (String.IsNullOrEmpty(u.NomeUsuario) || String.IsNullOrWhiteSpace(u.NomeUsuario))
                    {
                        throw new Exception("Nome de usuário inválido.");
                    }
                    if (u.NomeUsuario.Length > 25)
                    {
                        throw new Exception("O nome de usuário pode conter no máximo 25 caracteres.");
                    }
                    if (u.NomeUsuario.Equals(this.nome))
                    {
                        await uService.PutAlterarNomeAsync(u);
                    }
                }

                await Application.Current.MainPage
                    .DisplayAlert("Mensagem", "Dados salvos com sucesso!", "Ok");

                await Shell.Current.GoToAsync("..");
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage
                    .DisplayAlert("Ops", ex.Message, "Ok");
            }
        }

        public async void AlterarEmailUsuario()
        {
            try
            {
                Usuario usuario = new Usuario();
                var usuarioId = Preferences.Get("UsuarioId", usuario.IdUsuario);

                Usuario u = await
                    uService.GetUsuarioAsync(usuarioId);

                u.Email = this.email;

                if (u.IdUsuario == usuarioId)
                {
                    if (String.IsNullOrEmpty(u.Email) || String.IsNullOrWhiteSpace(u.Email))
                    {
                        throw new Exception("E-mail inválido.");
                    }
                    if (!u.Email.Contains('@'))
                    {
                        throw new Exception("E-mail inválido.");
                    }

                    char primeiroCaractere = char.Parse(u.Email.Substring(0, 1));
                    char ultimoCaractere = char.Parse(u.Email.Substring(u.Email.Length-1, 1));

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

                    if (u.Email.Equals(this.email))
                    {
                        await uService.PutAlterarEmailAsync(u);
                    }
                }

                await Application.Current.MainPage
                    .DisplayAlert("Mensagem", "Dados salvos com sucesso!", "Ok");

                await Shell.Current.GoToAsync("..");
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage
                    .DisplayAlert("Ops", ex.Message, "Ok");
            }
        }

        public async void AlterarSenhaUsuario()
        {
            try
            {
                Usuario usuario = new Usuario();
                var usuarioId = Preferences.Get("UsuarioId", usuario.IdUsuario);

                Usuario u = await
                    uService.GetUsuarioAsync(usuarioId);

                u.PasswordString = this.senha;

                if (u.IdUsuario == usuarioId)
                {
                    if (String.IsNullOrEmpty(u.PasswordString) || String.IsNullOrWhiteSpace(u.PasswordString))
                    {
                        throw new Exception("Senha inválida.");
                    }
                    if (u.PasswordString.Equals(this.senha))
                    {
                        await uService.PutAlterarSenhaAsync(u);
                    }
                }

                await Application.Current.MainPage
                    .DisplayAlert("Mensagem", "Dados salvos com sucesso!", "Ok");

                await Shell.Current.GoToAsync("..");
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage
                    .DisplayAlert("Ops", ex.Message, "Ok");
            }
        }

        public async void DeletarUsuario()
        {
            try
            {
                Usuario usuario = new Usuario();
                var usuarioId = Preferences.Get("UsuarioId", usuario.IdUsuario);

                Usuario u = await
                    uService.GetUsuarioAsync(usuarioId);

                if (await Application.Current.MainPage
                    .DisplayAlert("Confirmação", $"Confirma a remoção do usuário {u.NomeUsuario}?", "Sim", "Não"))
                {
                    await uService.DeletarUsuarioAsync(u.IdUsuario);

                    await Application.Current.MainPage.DisplayAlert("Mensagem",
                            $"Usuário {u.NomeUsuario} removido com sucesso!", "Ok");

                    Application.Current.Quit();
                }

                await Shell.Current.GoToAsync("..");
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage
                    .DisplayAlert("Ops", ex.Message, "Ok");
                throw;
            }
        }

        #endregion
    }
}
